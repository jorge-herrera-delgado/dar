using System.IO;
using Data.Access.Repository.Configuration;
using Data.Access.Repository.LegacyFile.Engine;

namespace Data.Access.Repository.LegacyFile.JsonFile
{
    public sealed class JsonFileBase : FileBase
    {
        public override FileType FileType => FileType.Json;

        public override string OpenConnection()
        {
            return File.ReadAllText(Path);
        }
    }
}
