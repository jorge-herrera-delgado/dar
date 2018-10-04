using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyDataBase.Engine;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.DatasourceContract;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.Repository.Engine.RepositoryDataBase.DataTransactionRepository
{
    public class DataTransaction : IDataTransaction
    {
        private readonly IConnectionProvider _provider;
        private readonly DataBaseType _dataBaseType;
        private readonly DataBase _db;

        public IDbTransaction Transaction { get; }

        public DataTransaction(IConnectionProvider provider, DataBaseType dataBaseType)
        {
            _provider = provider;
            _dataBaseType = dataBaseType;
            var connect = _provider.ConnectionSchemas.FirstOrDefault(x => x.SourceType == SourceType.Database && x.DataBaseType == _dataBaseType); 
            _db = new DataBaseRepositoryInit().GetRepositoryInit(_dataBaseType, connect);
            _db.OpenConnection();
            Transaction = _db.BeginTransaction();
        }

        public IDataTransaction Execute(Action<IDataBaseDataSource> action)
        {
            action.Invoke(GetDataBaseDataSource());
            return this;
        }

        public IDataTransaction Execute(IDataBaseDataSource dataBaseDataSource, Action<IDataBaseDataSource> action)
        {
            action.Invoke(dataBaseDataSource);
            return this;
        }

        private IDataBaseDataSource GetDataBaseDataSource()
        {
            switch (_dataBaseType)
            {
                case DataBaseType.Oracle: case DataBaseType.Sql:
                    return new DataBaseRepositoryBase(_provider, _dataBaseType);
                case DataBaseType.NoValid:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IDataTransaction ExecuteRange(IEnumerable<Action<IDataBaseDataSource>> actions)
        {
            foreach (var action in actions)
                Execute(action);
            return this;
        }

        public IDataTransaction ExecuteRange(IDataBaseDataSource dataBaseDataSource, IEnumerable<Action<IDataBaseDataSource>> actions)
        {
            foreach (var action in actions)
                Execute(dataBaseDataSource, action);
            return this;
        }

        public void CompleteTransaction()
        {
            try
            {
                _db.Commit();
            }
            catch
            {
                _db.Rollback();
                throw;
            }
            finally
            {
                _db.Dispose();
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        ~DataTransaction()
        {
            _db.Dispose(false);
        }
    }
}
