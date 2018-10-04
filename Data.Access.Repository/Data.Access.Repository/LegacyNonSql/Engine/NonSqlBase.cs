using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyNonSql.Model;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.LegacyNonSql.Engine
{
    public abstract class NonSqlBase<T> : ISourceStorage<T>
    {
        protected ClientModel Client;
        public ClientModel ClientBase
        {
            set => Client = value == default(ClientModel) ? null : value;
        }

        public SourceType SourceType => SourceType.NonSql;
        public abstract NonSqlType NonSqlType { get; }
        public abstract T OpenConnection();

        public void Dispose()
        { }
    }
}
