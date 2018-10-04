using System;
using System.Data;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.LegacyDataBase.Engine
{
    public abstract class DataBase : ISourceStorage<IDbConnection>
    {
        protected IDbConnection Db;
        protected string ConnString;
        private IDbTransaction _trans;
        private bool _disposed;

        public abstract DataBaseType DbType { get; }
        public string ConnectionString
        {
            set => ConnString = value;
        }

        public SourceType SourceType => SourceType.Database;
        public abstract IDbConnection OpenConnection();

        public IDbTransaction BeginTransaction()
        {
            _trans = Db.BeginTransaction();
            return _trans;
        }

        public void Commit()
        {
            if (_trans != null)
            {
                _trans.Commit();
            }
            else
                throw new NullReferenceException("Null instance for transaction",
                    new Exception("Transaction cannot be null, please be sure that you begin the transaction"));
        }

        public void Rollback()
        {
            if (_trans != null)
                _trans.Rollback();
            else
                throw new NullReferenceException("Null instance for transaction", 
                    new Exception("Transaction cannot be null, please be sure that you begin the transaction"));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_trans != null)
                    {
                        _trans.Dispose();
                        _trans = null;
                    }
                    if (Db != null)
                    {
                        Db.Close();
                        Db.Dispose();
                        Db = null;
                    }
                }
                _disposed = true;
            }
        }
    }
}
