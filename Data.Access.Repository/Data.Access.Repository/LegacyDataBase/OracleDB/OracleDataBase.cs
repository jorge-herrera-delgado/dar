using System.Data;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyDataBase.Engine;
using Oracle.ManagedDataAccess.Client;

namespace Data.Access.Repository.LegacyDataBase.OracleDB
{
    public sealed class OracleDataBase : DataBase
    {
        public override DataBaseType DbType => DataBaseType.Oracle;

        public override IDbConnection OpenConnection()
        {
            Db = new OracleConnection(ConnString);
            Db.Open();
            return Db;
        }
    }
}
