using Data.Access.Repository.Configuration;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.LegacyFile.Engine
{
    public abstract class FileBase : ISourceStorage<string>
    {
        protected string Path;
        public abstract FileType FileType { get; }

        public string PathFile
        {
            set => Path = string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public SourceType SourceType => SourceType.File;

        public abstract string OpenConnection();

        public void Dispose()
        {
            Path = null;
        }
    }
}
