using Dapper;
using System.Data;

namespace Data.Access.Repository.LegacyDataBase.SqlDb
{
    public class SqlDynamicParameters : DynamicParameters
    {
        public new void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            base.AddParameters(command, identity);
        }
    }
}
