using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Data.Access.Repository.LegacyDataBase.OracleDB
{
    internal sealed class ParamInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
        public OracleDbType? DbType { get; set; }
        public int? Size { get; set; }
        public IDbDataParameter AttachedParam { get; set; }
    }
}
