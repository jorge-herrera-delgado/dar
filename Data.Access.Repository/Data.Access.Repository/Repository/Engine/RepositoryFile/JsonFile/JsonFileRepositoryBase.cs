using System.Collections.Generic;
using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.Helper;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.DatasourceContract;
using Newtonsoft.Json.Linq;

namespace Data.Access.Repository.Repository.Engine.RepositoryFile.JsonFile
{
    public class JsonFileRepositoryBase : RepositoryBaseFile, IFileBaseDataSource
    {
        private readonly string _fileContent;
        public JsonFileRepositoryBase(IConnectionProvider connectionProvider, FileType fileType) : base(connectionProvider, fileType)
        {
            _fileContent = FileBaseRepo.OpenConnection();
        }

        public IEnumerable<T> GetAll<T>() where T: class 
            => new JsonMapper(_fileContent).GetModel<IEnumerable<T>>();

        public IEnumerable<T> GetItems<T>(string jsonPath)
        {
            var obj = JObject.Parse(_fileContent);
            var values = obj.SelectTokens(jsonPath);
            return values.Select(v => v.ToObject<T>());
        }

        public T GetItem<T>(string jsonPath)
        {
            var obj = JObject.Parse(_fileContent);
            var value = obj.SelectToken(jsonPath);
            return value.ToObject<T>();
        }
    }
}
