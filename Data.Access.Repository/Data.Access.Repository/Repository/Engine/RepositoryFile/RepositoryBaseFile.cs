using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyFile.Engine;
using Data.Access.Repository.Repository.Engine.Connection;
using Data.Access.Repository.Repository.Engine.Connection.Model;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.Repository.Engine.RepositoryFile
{
    public abstract class RepositoryBaseFile
    {
        protected readonly FileBase FileBaseRepo;
        protected readonly ConnectionSchema ConnectionSchema;

        public FileType FileType { get; }

        protected RepositoryBaseFile(IConnectionProvider connectionProvider, FileType fileType)
        {
            FileType = fileType;
            ConnectionSchema = connectionProvider.ConnectionSchemas.FirstOrDefault(x => x.SourceType == SourceType.File && x.FileType == fileType);
            FileBaseRepo = new FileRepositoryInit().GetRepositoryInit(fileType, ConnectionSchema);
        }
    }
}
