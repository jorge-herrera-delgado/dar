using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyNonSql.Engine;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.Connection.Model;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.Repository.Engine.RepositoryNonSql
{
    public abstract class RepositoryBaseNonSql<TNonSqlDataBase>
    {
        protected readonly NonSqlBase<TNonSqlDataBase> NonSqlBaseRepo;
        protected readonly ConnectionSchema ConnectionSchema;
        public NonSqlType NonSqlType { get; }

        protected RepositoryBaseNonSql(IConnectionProvider connectionProvider, NonSqlType nonSqlType)
        {
            NonSqlType = nonSqlType;
            ConnectionSchema = connectionProvider.ConnectionSchemas.FirstOrDefault(x => x.SourceType == SourceType.NonSql && x.NonSqlType == nonSqlType);
            NonSqlBaseRepo = new NonSqlRepositoryInit().GetRepositoryInit<TNonSqlDataBase>(nonSqlType, ConnectionSchema);
        }
    }
}
