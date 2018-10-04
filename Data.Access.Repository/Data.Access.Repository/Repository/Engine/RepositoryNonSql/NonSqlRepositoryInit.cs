using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyNonSql.Engine;
using Data.Access.Repository.LegacyNonSql.Model;
using Data.Access.Repository.Repository.Engine.Connection.Model;

namespace Data.Access.Repository.Repository.Engine.RepositoryNonSql
{
    public sealed class NonSqlRepositoryInit
    {
        public NonSqlBase<TNonSqlDataBase> GetRepositoryInit<TNonSqlDataBase>(NonSqlType nonSqlType, ConnectionSchema connectDetail)
        {
            var nonSqlBaseRepo = nonSqlType.GetNonSqlBase<TNonSqlDataBase>();
            nonSqlBaseRepo.ClientBase = new ClientModel
            {
                ConnectionString = connectDetail.ConnectionString
            };
            return nonSqlBaseRepo;
        }
    }
}
