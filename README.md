# DAR - Data Access and Repository
We developers have seen so many times in our code that we have so many ways to add a new repository and sometimes it is a bit confusing and we ask ourselves:

- What do I have to use?
- Do I need an extra library?
- Do I need to implement commit and dispose anywhere?
- Why should it takes me longer than it is to add a new repository?
- **What about if I need to use several Data Sources?**

**My own experience:** Within a development, I was asked to start using `MongoDb` but also we had part of the data in `Oracle`. So, I was wondering myself. How will I implemnet both data sources within the same application?

So, I came up with the idea and created a simple and powerful `Data Access and Repository (DAR)` engine where we can apply for several storages (let’s say `Oracle`, `SQL`, `MongoDb` and so on) in the same application.

For sure you’re now wondering, which scenario could it be used in?

**My answer:** Well, there are several scenarios so I list some of them and there are more for sure:
- Your app/service has to change from let’s say SQL to MongoDb
- After this change(`SQL` to `MongoDb`) you need some legacy data from `SQL`
- Preventing changes in business logic

### Implementation for ORM
Currently, the framework supports `SQL`, `Oracle` and `MySql` and it is powered by [Dapper](https://dapper-tutorial.net/dapper).

In order to implement it in any Repository, we need several steps. **I promise, it is very easy to follow them.**

##### Steps:

1. We need our contract (interface) if we want to get data from several sources.

   ```c#
   public interface IPeople
   {
	   SourceType SourceType { get; }
	   IEnumerable<Person> GetPeople();
	   Person GetPerson(int id);
	   bool AddPerson(Person person);
   }
   ```

   What is `SourceType` property? 

   It is the flag that the engine will use to understand where the data is comming from (EG. `DataBase`, `NonSql`, `File`).

2. Here is where we implement our Repository. In this case, implementation for `Oracle`.
   ```c#
   public class PeopleRepository: IPeople
    {
        private readonly IDataBaseDataSource _dataBaseDataSource;
        public SourceType SourceType => SourceType.Database;

        public ServerModeRepository(IDataBaseDataSource dataBaseDataSource)
        {
            _dataBaseDataSource = dataBaseDataSource;
        }

        public IEnumerable<Person> GetPeople()
        {
            var param = new OracleDynamicParameters();
            param.Add(":po_cursor", dbType: OracleDataType.RefCursor, direction: ParameterDirection.Output);

            var result = new List<Person>();
            _dataBaseDataSource.DataTransaction.Execute(_dataBaseDataSource, r1 => result =
                r1.GetItems<Person, OracleDynamicParameters>(@"Your Stored Procedure",
                    param, CommandType.StoredProcedure).ToList()).CompleteTransaction();

            return result;
        }

        public Person GetPerson(int id)
        {
            var param = new OracleDynamicParameters();
            param.Add(":pi_person_id", id);

            var result = new Person();
            _dataBaseDataSource.DataTransaction.Execute(_dataBaseDataSource, r1 => 
            result = r1.GetItem<Person, OracleDynamicParameters>
            (@"SELECT * FROM PERSON WHERE PERSON_ID = :pi_person_id",
                    param, CommandType.Text)).CompleteTransaction();

            return result;
        }

        public bool AddPerson(Person person)
        {
            var param = new OracleDynamicParameters();
            param.Add(":pi_firstname", person.Firstname);
            param.Add(":pi_lastname", person.Lastname);
            param.Add(":pi_age", person.Age);

            var result = false;
            try
            {
                _dataBaseDataSource.DataTransaction.Execute(_dataBaseDataSource, r => result = 
				r.Insert("Your Stored Procedure", commandType: CommandType.StoredProcedure, parameters: param)).CompleteTransaction();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
    }
   ```
### Points of Interest
**IDataBaseDataSource:** This will execute the actions that we need into any method (EG. GetItem/s, Insert, Update, Delete). We will come back later with the injection of this dependency.

**SourceType:** Based on the repository that we just created the value must be `SourceType.Database`.

#### Q&A
- How do I make the implementation for `SQL` and `MySql`?
  Well, you might get surprised if I say that all of them are pretty much the same in terms of usuability with this framework.
- So, what is the difference?
  Ok, take a look into each method. We are using `OracleDynamicParameters`. Yes!!!, That is the difference.
  So, now that you know what is the difference it is up to you to use `SQL` or `MySql`
  - SQL => SqlDynamicParameters
  - MySql => MySqlDynamicParameters
  
### Implementation for NonSql
Currently, the framework supports `MongoDb` and it is powered by [MongoDb Driver](https://docs.mongodb.com/ecosystem/drivers/csharp/).

Following the implementation for **ORM**, we will implement the same repository for `MongoDb`.

##### Steps:

1. We need our contract (interface) if we want to get data from several sources. 
   See above in [Implementation for ORM](https://github.com/jorge-herrera-delgado/dar/new/master?readme=1#implementation-for-orm) 
in [Step 1](https://github.com/jorge-herrera-delgado/dar/new/master?readme=1#steps).
2. Here is where we implement our Repository. In this case, implementation for `MongoDb`.
   ```c#
   public class PeopleRepository : IPeople
    {
        public SourceType SourceType => SourceType.NonSql;
        private readonly INonSqlDataSource _nonSqlDataSource;
        private readonly NonSqlSchema _nonSqlSchema = new NonSqlSchema("DataBaseName", "CollectionName");

        public ServerModeRepository(INonSqlDataSource nonSqlDataSource)
        {
            _nonSqlDataSource = nonSqlDataSource;
        }

        public IEnumerable<Person> GetPeople()
        {
            return _nonSqlDataSource.GetAll<Person>(_nonSqlSchema);
        }

        public Person GetPerson(int id)
	    {
            return _nonSqlDataSource.GetItem(_nonSqlSchema, p => p.PersonId == id);
	    }

        public bool LogServerMode(Person person)
	    {
            return _nonSqlDataSource.Insert(_nonSqlSchema, person);
	    }
    }
   ```
### Points of Interest
**SourceType:** Based on the repository that we just created the value must be `SourceType.NonSql`.

**INonSqlDataSource:** This will execute the actions that we need into any method (EG. GetItem/s, Insert, Update, Delete). We will come back later with the injection of this dependency.

**NonSqlSchema:** this is the schema that MongoDb Driver will use to execute the actions we will ask within the repository.

##### Not difficult so far, right?!!!

### Connection String and Configuration
You might wonder, where are the connection string and configurations come up? 

Of course, we need to setup these values in order to access into our data. Let's see below how to do it.

#### General Configuration
In order to make it work, we need to call in our startup class or initializer method the `IoC` assemblies register.
```c#
IoCSource.Register();
```
It will add the assemblies into memory in order to use any of the sources that the framework currently supports.

#### Configuration ORM:
**IDataBaseDataSource:** In order to inject this dependency, we have a class that we can use or even inherit to call our own custom dependency but let's use the default class so you can understand the functionality.
```c#
new DataBaseRepositoryBase(connectionProvider, dataBaseType);
```
You might wonder, what is `connectionProvider`? Well, this is where we provide our configuration to the framework. We can create our own custome dependency but let's use the default class.
```c#
new ConnectionProvider(connection);
```
And now `connection`? Mmmmm, I know. It looks a bit messy but it is basically a list of schemas `IEnumerable<ConnectionSchema> ConnectionSchemas`. 
Later you will understand and can create your own custom class to give it more functionality but for now we will use it as below.
```c#
public class Connection
{
    public IEnumerable<ConnectionSchema> ConnectionSchemas { get; set; }
}

public class ConnectionProvider : IConnectionProvider
{
    public IEnumerable<ConnectionSchema> ConnectionSchemas { get; set; }

    public ConnectionProvider(Connection connectionSchemas)
    {
        ConnectionSchemas = connectionSchemas.ConnectionSchemas;
    }
}
```

So, now that we have all the dependencies that we need, let's put them together in a test.
```c#
[TestMethod]
public void Person_Not_Null()
{
	var connection = new Connection();
	connection.ConnectionSchemas =  new List<ConnectionSchema>();
	connection.ConnectionSchemas.Add(new ConnectionSchema{ SourceType = SourceType.Database, DataBaseType = DataBaseType.Oracle, ConnectionString = "Your ConnectionString" });
	connection.ConnectionSchemas.Add(new ConnectionSchema{ SourceType = SourceType.Database, DataBaseType = DataBaseType.Sql, ConnectionString = "Your ConnectionString" });
	
	var provider = new ConnectionProvider(connection);	
	//Oracle
	var baseRepository = new DataBaseRepositoryBase(provider, DataBaseType.Oracle);	
	IPeople peopleRepository = new Oracle.PeopleRepository(baseRepository);
	var person7 = peopleRepository.GetPerson(7);
	Assert.IsTrue(person != null);
	
	//Sql
	baseRepository = new DataBaseRepositoryBase(provider, DataBaseType.Sql);
	peopleRepository = new Sql.PeopleRepository(baseRepository);
	var person2 = peopleRepository.GetPerson(2);
	Assert.IsTrue(person != null);
}
```

So, as you can see, we were able to get data from `SQL` and `Oracle` within the same method just adding schemas into a list. But let's go a bit forward and let's add `MongoDb`.

#### Configuration NonSql (MongoDb):
**INonSqlDataSource:** In order to inject this dependency, we have a class that we can use or even inherit to call our own custom dependency but let's use the default class so you can understand the functionality.
```c#
new MongoRepositoryBase(provider);
```

For other dependencies, follow the same steps as per [Configuration ORM](https://github.com/jorge-herrera-delgado/dar/new/master?readme=1#configuration-orm).

So, now that we have all the dependencies that we need, let's see how it works for `MongoDb` in a test.

```c#
[TestMethod]
public void Person_Not_Null()
{
	var connection = new Connection();
	connection.ConnectionSchemas =  new List<ConnectionSchema>();
	connection.ConnectionSchemas.Add(new ConnectionSchema{ SourceType = SourceType.NonSql, NonSqlType = NonSqlType.MongoDb, ConnectionString = "Your ConnectionString" });
	
	var provider = new ConnectionProvider(connection);
	
	var baseRepository = new MongoRepositoryBase(provider);
	IPeople peopleRepository = new Mongo.PeopleRepository(baseRepository);
	var person3 = peopleRepository.GetPerson(3);
	Assert.IsTrue(person != null);
}
```

**Ok, looks like we are connected with several sources in the same solution.**

That is all for now, hope you enjoy using this framework. Happy coding!!!

**Note:** If you have any issue regarding configuration, please be sure that you are following the steps from [Connection String and Configuration](https://github.com/jorge-herrera-delgado/dar/new/master?readme=1#connection-string-and-configuration), 
otherwise do not hesitate to contact me so I can help you.
