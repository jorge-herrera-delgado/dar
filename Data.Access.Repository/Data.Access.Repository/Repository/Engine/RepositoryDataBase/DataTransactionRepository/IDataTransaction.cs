using System;
using System.Collections.Generic;
using System.Data;
using Data.Access.Repository.Repository.Engine.DatasourceContract;

namespace Data.Access.Repository.Repository.Engine.RepositoryDataBase.DataTransactionRepository
{
    public interface IDataTransaction : IDisposable
    {
        IDbTransaction Transaction { get; }
        void CompleteTransaction();
        IDataTransaction Execute(Action<IDataBaseDataSource> action);
        IDataTransaction Execute(IDataBaseDataSource dataBaseDataSource, Action<IDataBaseDataSource> action);
        IDataTransaction ExecuteRange(IEnumerable<Action<IDataBaseDataSource>> actions);
        IDataTransaction ExecuteRange(IDataBaseDataSource dataBaseDataSource, IEnumerable<Action<IDataBaseDataSource>> actions);
    }
}
