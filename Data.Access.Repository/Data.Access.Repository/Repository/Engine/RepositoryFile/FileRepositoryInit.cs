using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyFile.Engine;
using Data.Access.Repository.Repository.Engine.Connection.Model;

namespace Data.Access.Repository.Repository.Engine.RepositoryFile
{
    public class FileRepositoryInit
    {
        public FileBase GetRepositoryInit(FileType fileType, ConnectionSchema connectionSchema)
        {
            var fileRepo = fileType.GetFileBase();
            fileRepo.PathFile = connectionSchema.ConnectionString;
            return fileRepo;
        }
    }
}
