using System.Collections.Generic;
using System.Linq;
using Data.Access.Repository.IoC;

namespace Data.Access.Repository.SourceStorage.Engine
{
    public static class SourceFactory
    {
        public static IEnumerable<ISourceStorage<T>> GetSourceStorage<T>(this SourceType sourceType) =>
            IoCSource.IoCGetAllInstances<ISourceStorage<T>>().Where(x => x.SourceType == sourceType);
    }
}
