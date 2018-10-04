using System.Data;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyDataBase.Engine;
using MySql.Data.MySqlClient;

namespace Data.Access.Repository.LegacyDataBase.MySqlDb
{
    public class MySqlDataBase : DataBase
    {
        public override DataBaseType DbType => DataBaseType.MySql;

        public override IDbConnection OpenConnection()
        {
            Db = new MySqlConnection(ConnString);
            Db.Open();
            return Db;
        }
    }
}
