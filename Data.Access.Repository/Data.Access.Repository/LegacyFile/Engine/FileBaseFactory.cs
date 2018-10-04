using System.Linq;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.SourceStorage.Engine;

namespace Data.Access.Repository.LegacyFile.Engine
{
    public static class FileBaseFactory
    {
        public static FileBase GetFileBase(this FileType fileType) =>
            SourceType.File.GetSourceStorage<string>()
                .Select(x => (FileBase)x)
                .FirstOrDefault(x => x.FileType == fileType);
    }
}
