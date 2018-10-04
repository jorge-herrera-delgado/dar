namespace Data.Access.Repository.Repository.Engine.Connection.Model
{
    public class NonSqlSchema
    {
        public string DataBaseName { get; }
        public string CollectionName { get; }

        public NonSqlSchema(string dataBaseName, string collectionName)
        {
            DataBaseName = dataBaseName;
            CollectionName = collectionName;
        }
    }
}