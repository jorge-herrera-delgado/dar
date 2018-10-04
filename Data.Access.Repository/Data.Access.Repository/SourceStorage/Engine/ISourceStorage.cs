using System;

namespace Data.Access.Repository.SourceStorage.Engine
{
    public interface ISourceStorage<out T> : IDisposable
    {
        SourceType SourceType { get; }
        T OpenConnection();
        new void Dispose();
    }
}
