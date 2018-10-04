using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Access.Repository.Repository.Engine
{
    public interface IRepositoryValue<TEntity> where TEntity: class
    {
        TEntity GetEntity(TEntity value);
        IEnumerable<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate);
        bool Exists(TEntity value);
    }
}
