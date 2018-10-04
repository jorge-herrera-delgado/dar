using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyNonSql.Engine;
using MongoDB.Driver;

namespace Data.Access.Repository.LegacyNonSql.MongoDB
{
    public sealed class MongoDataBase : NonSqlBase<MongoClient>
    {
        public override NonSqlType NonSqlType => NonSqlType.MongoDb;

        public override MongoClient OpenConnection() 
            => new MongoClient(Client.ConnectionString);
    }
}
