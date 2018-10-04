using System.Collections.Generic;
using System.Data;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.Repository.Engine.RepositoryDataBase.DataTransactionRepository;

namespace Data.Access.Repository.Repository.Engine.DatasourceContract
{
    public interface IDataBaseDataSource
    {
        DataBaseType DataBaseType { get; }
        IDataTransaction DataTransaction { get; }
        IEnumerable<T> GetItems<T, TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class;
        IEnumerable<T> GetItems<T>(string sql, CommandType? commandType = null);
        string GetItemJson<T, TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class;
        T GetItem<T, TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class;
        bool Insert<TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class;
        bool Insert(string sql, CommandType? commandType = null);
        bool Delete<TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class;
        void Update<TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class;
    }
}
