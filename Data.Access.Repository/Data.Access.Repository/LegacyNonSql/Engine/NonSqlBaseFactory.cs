using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.LegacyNonSql.Engine
{
    public static class NonSqlBaseFactory
    {
        public static NonSqlBase<TDataBase> GetNonSqlBase<TDataBase>(this NonSqlType nonSqlType) =>
            SourceType.NonSql.GetSourceStorage<TDataBase>()
                .Select(x => (NonSqlBase<TDataBase>)x)
                .FirstOrDefault(x => x.NonSqlType == nonSqlType);
    }
}
