using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyDataBase.Extension;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.DatasourceContract;
using Data.Access.Repository.Repository.Engine.RepositoryDataBase.DataTransactionRepository;

namespace Data.Access.Repository.Repository.Engine.RepositoryDataBase
{
    public class DataBaseRepositoryBase : RepositoryBase, IDataBaseDataSource
    {
        public DataBaseRepositoryBase(IConnectionProvider connectionProvider, DataBaseType dataBaseType) 
            : base(connectionProvider, dataBaseType, new DataTransaction(connectionProvider, dataBaseType))
        { }

        public IEnumerable<T> GetItems<T, TParams>(string sql, TParams parameters = null, CommandType? commandType = null) where TParams: class
        {
            return Conn.Query<T>(sql, parameters, commandType: commandType, transaction: Trans);
        }

        public IEnumerable<T> GetItems<T>(string sql, CommandType? commandType = null)
        {
            return Conn.Query<T>(sql, commandType: commandType, transaction: Trans);
        }

        public string GetItemJson<T, TParams>(string sql, TParams parameters, CommandType? commandType = null) where TParams : class
        {
            return Conn.QueryToJson<T>(sql, parameters, commandType: commandType, transaction: Trans);
        }

        public T GetItem<T, TParams>(string sql, TParams parameters = null, CommandType? commandType = null) where TParams : class
        {
            return Conn.Query<T>(sql, parameters, commandType: commandType, transaction: Trans).FirstOrDefault();
        }

        public bool Insert<TParams>(string sql, TParams parameters = null, CommandType? commandType = null) where TParams : class
        {
            return Conn.Execute(sql, parameters, Trans, commandType: commandType) > 0;
        }

        public bool Insert(string sql, CommandType? commandType = null)
        {
            return Conn.Execute(sql, Trans, commandType: commandType) > 0;
        }

        public bool Delete<TParams>(string sql, TParams parameters = null, CommandType? commandType = null) where TParams : class
        {
            return Conn.Execute(sql, parameters, Trans, commandType: commandType) > 0;
        }

        public void Update<TParams>(string sql, TParams parameters = null, CommandType? commandType = null) where TParams : class
        {
            Conn.Execute(sql, parameters, Trans, commandType: commandType);
        }
    }
}
