using System.Data;
using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.LegacyDataBase.Engine
{
    public static class DataBaseFactory
    {
        public static DataBase GetDataBase(this DataBaseType dbType) =>
            SourceType.Database.GetSourceStorage<IDbConnection>()
                .Select(x => (DataBase)x)
                .FirstOrDefault(x => x.DbType == dbType);
    }
}
