using System.Data;
using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.RepositoryDataBase.DataTransactionRepository;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.Repository.Engine.RepositoryDataBase
{
    public abstract class RepositoryBase
    {
        protected readonly IDbTransaction Trans;
        protected readonly IDbConnection Conn;

        public IDataTransaction DataTransaction { get; }
        public DataBaseType DataBaseType { get; }

        protected RepositoryBase(IConnectionProvider connectionProvider, DataBaseType dataBaseType, IDataTransaction dataTransaction = null)
        {
            DataBaseType = dataBaseType;
            var connect = connectionProvider.ConnectionSchemas.FirstOrDefault(x => x.SourceType == SourceType.Database && x.DataBaseType == dataBaseType);
            if (dataTransaction == null)
                Conn = new DataBaseRepositoryInit().GetRepositoryInit(dataBaseType, connect).OpenConnection();
            else
            {
                Conn = dataTransaction.Transaction.Connection;
                Trans = dataTransaction.Transaction;
            }
            DataTransaction = dataTransaction;
        }
    }
}
