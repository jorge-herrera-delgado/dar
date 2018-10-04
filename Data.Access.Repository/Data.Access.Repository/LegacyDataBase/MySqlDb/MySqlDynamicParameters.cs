using Dapper;
using System.Data;

namespace Data.Access.Repository.LegacyDataBase.MySqlDb
{
    public class MySqlDynamicParameters : DynamicParameters
    {
        public new void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            base.AddParameters(command, identity);
        }
    }
}
