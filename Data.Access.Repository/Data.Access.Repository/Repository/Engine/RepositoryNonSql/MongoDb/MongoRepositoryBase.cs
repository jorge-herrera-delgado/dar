using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.Connection.Model;
using Data.Access.Repository.Repository.Engine.DatasourceContract;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Data.Access.Repository.Repository.Engine.RepositoryNonSql.MongoDb
{
    public class MongoRepositoryBase :  RepositoryBaseNonSql<MongoClient>, INonSqlDataSource
    {
        private readonly MongoClient _mongoClient;
        public MongoRepositoryBase(IConnectionProvider connectionProvider) : base(connectionProvider, NonSqlType.MongoDb)
        {
            _mongoClient = NonSqlBaseRepo.OpenConnection();
        }

        #region Public Methods

        public IEnumerable<T> GetAll<T>(NonSqlSchema nonSqlSchema)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.Find(FilterDefinition<T>.Empty).ToList();
        }

        public Task GetAllAsync<T>(NonSqlSchema nonSqlSchema)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.FindAsync(FilterDefinition<T>.Empty);
        }

        public T GetItem<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.Find(filterExpression).FirstOrDefault();
        }

        public IMongoQueryable<T> GetMongoQueryable<T>(NonSqlSchema nonSqlSchema)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.AsQueryable();
        }

        public Task GetItemsAsync<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.FindAsync(filterExpression);
        }

        public IEnumerable<T> GetItems<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.Find(filterExpression).ToList();
        }

        public IEnumerable<TNewProjection> GetItems<T, TNewProjection>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filterExpression, Expression<Func<T, TNewProjection>> projectionExpression)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName) ;
            return collection.Find(filterExpression).Project(projectionExpression).ToList();
        }

        public bool Insert<T>(NonSqlSchema nonSqlSchema, T obj)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName) ;
            collection.InsertOne(obj);
            return true;
        }

        public Task InsertAsync<T>(NonSqlSchema nonSqlSchema, T obj)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.InsertOneAsync(obj);
        }

        public bool InsertRange<T>(NonSqlSchema nonSqlSchema, IEnumerable<T> objs)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            collection.InsertMany(objs);
            return true;
        }

        public Task InsertRangeAsync<T>(NonSqlSchema nonSqlSchema, IEnumerable<T> objs)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.InsertManyAsync(objs);
        }

        public bool DeleteRange<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName) ;
            var status = collection.DeleteMany(filter);
            return status.IsAcknowledged && status.DeletedCount > 0;
        }

        public Task DeleteRangeAsync<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.DeleteManyAsync(filter);
        }

        public bool Delete<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            var status = collection.DeleteOne(filter);
            return status.IsAcknowledged && status.DeletedCount > 0;
        }

        public Task DeleteAsync<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName);
            return collection.DeleteOneAsync(filter);
        }

        public void Update<T>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter, IDictionary<string, object> updateParameters)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName) ;
            collection.UpdateOne(filter, GetUpdateDefinition<T>(updateParameters));
        }

        public void Update<T, TField>(NonSqlSchema nonSqlSchema, Expression<Func<T, bool>> filter, Expression<Func<T, TField>> updateExp, TField value)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<T>(nonSqlSchema.CollectionName) ;
            collection.UpdateOne(filter, GetUpdateDefinition(updateExp, value));
        }

        public bool Replace<TDocument>(NonSqlSchema nonSqlSchema, Expression<Func<TDocument, bool>> filter, TDocument newDocument)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<TDocument>(nonSqlSchema.CollectionName);
            var status = collection.ReplaceOne(filter, newDocument);
            return status.IsAcknowledged && status.ModifiedCount > 0;
        }

        public Task ReplaceAsync<TDocument>(NonSqlSchema nonSqlSchema, Expression<Func<TDocument, bool>> filter, TDocument newDocument)
        {
            var mongoDb = _mongoClient.GetDatabase(nonSqlSchema.DataBaseName);
            var collection = mongoDb.GetCollection<TDocument>(nonSqlSchema.CollectionName);
            return collection.ReplaceOneAsync(filter, newDocument);
        }

        #endregion

        #region Private Methods

        private static UpdateDefinition<T> GetUpdateDefinition<T>(IDictionary<string, object> updateParameters)
        {
            var updateDefinitionBuilder = Builders<T>.Update;
            if (updateParameters != null && updateParameters.Count > 0)
            {
                var fieldsToBeUpdated = updateParameters.Select(entry => updateDefinitionBuilder.Set(entry.Key, entry.Value)).ToList();
                return updateDefinitionBuilder.Combine(fieldsToBeUpdated);
            }
            throw new ArgumentNullException(nameof(updateParameters));
        }

        private static UpdateDefinition<T> GetUpdateDefinition<T, TField>(Expression<Func<T, TField>> filter, TField value)
        {
            var updateDefinitionBuilder = Builders<T>.Update;
            return filter != null
                ? updateDefinitionBuilder.Set(filter, value)
                : throw new ArgumentNullException(nameof(value));
        }

        #endregion
    }
}