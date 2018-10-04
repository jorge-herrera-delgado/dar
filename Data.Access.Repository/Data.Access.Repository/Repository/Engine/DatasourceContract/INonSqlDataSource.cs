using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.Repository.Engine.Connection.Model;
using MongoDB.Driver.Linq;

namespace Data.Access.Repository.Repository.Engine.DatasourceContract
{
    public interface INonSqlDataSource
    {
        NonSqlType NonSqlType { get; }
        IEnumerable<T> GetAll<T>(NonSqlSchema nonSqlSchema);
        Task GetAllAsync<T>(NonSqlSchema nonSqlSchema);
        T GetItem<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression);
        IMongoQueryable<T> GetMongoQueryable<T>(NonSqlSchema nonSqlSchema);
        IEnumerable<T> GetItems<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression);
        Task GetItemsAsync<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression);
        IEnumerable<TNewProjection> GetItems<T, TNewProjection>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TNewProjection>> projectionExpression);
        bool Insert<T>(NonSqlSchema nonSqlSchema, T obj);
        bool InsertRange<T>(NonSqlSchema nonSqlSchema, IEnumerable<T> objs);
        Task InsertAsync<T>(NonSqlSchema nonSqlSchema, T obj);
        Task InsertRangeAsync<T>(NonSqlSchema nonSqlSchema, IEnumerable<T> objs);
        bool Delete<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter);
        Task DeleteAsync<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter);
        bool DeleteRange<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter);
        Task DeleteRangeAsync<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter);
        void Update<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter, IDictionary<string, object> updateParameters);
        void Update<T, TField>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter, Expression<Func<T, TField>> updateExp, TField value);
        bool Replace<TDocument>(NonSqlSchema nonSqlSchema, Expression<Func<TDocument, bool>> filter, TDocument newDocument);
        Task ReplaceAsync<TDocument>(NonSqlSchema nonSqlSchema, Expression<Func<TDocument, bool>> filter, TDocument newDocument);
    }
}
