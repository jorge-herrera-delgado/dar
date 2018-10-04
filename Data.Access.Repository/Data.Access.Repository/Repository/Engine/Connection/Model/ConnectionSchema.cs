using Data.Access.Repository.Configuration;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.Repository.Engine.Connection.Model
{
    public class ConnectionSchema
    {
        public SourceType SourceType { get; set; }
        public DataBaseType DataBaseType { get; set; } = DataBaseType.NoValid;
        public NonSqlType NonSqlType { get; set; } = NonSqlType.NoValid;
        public FileType FileType { get; set; } = FileType.NoValid;
        public string ConnectionString { get; set; }
    }
}
