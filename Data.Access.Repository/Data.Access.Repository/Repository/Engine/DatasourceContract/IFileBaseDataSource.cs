using System.Collections.Generic;
using Data.Access.Repository.Configuration;

namespace Data.Access.Repository.Repository.Engine.DatasourceContract
{
    public interface IFileBaseDataSource
    {
        FileType FileType { get; }
        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable<T> GetItems<T>(string jsonPath);
        T GetItem<T>(string jsonPath);
    }
}
