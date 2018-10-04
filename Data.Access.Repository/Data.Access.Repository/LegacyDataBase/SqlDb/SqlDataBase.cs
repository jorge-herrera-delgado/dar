using System.Data;
using System.Data.SqlClient;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyDataBase.Engine;

namespace Data.Access.Repository.LegacyDataBase.SqlDb
{
    public sealed class SqlDataBase : DataBase
    {
        public override DataBaseType DbType => DataBaseType.Sql;

        public override IDbConnection OpenConnection()
        {
            Db = new SqlConnection(ConnString);
            Db.Open();
            return Db;
        }
    }
}
