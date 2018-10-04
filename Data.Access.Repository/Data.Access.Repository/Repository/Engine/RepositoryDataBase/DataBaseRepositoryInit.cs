using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyDataBase.Engine;
using Data.Access.Repository.Repository.Engine.Connection.Model;

namespace Data.Access.Repository.Repository.Engine.RepositoryDataBase
{
    public sealed class DataBaseRepositoryInit
    {
        public DataBase GetRepositoryInit(DataBaseType dbType, ConnectionSchema connectDetail)
        {
            var dataBaseRepo = dbType.GetDataBase();
            dataBaseRepo.ConnectionString = connectDetail.ConnectionString;
            return dataBaseRepo;
        }
    }
}
