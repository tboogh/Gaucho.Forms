using System.IO;

namespace Gaucho.Forms.Core.Services {
    public enum StorageLocation {
        AppResource,
        Documents,
        Cache
    }

    public enum FileMode {
        Read,
        Write,
        CreateNew
    }

    public interface IFileSystem {
        Stream OpenFile(string filename, StorageLocation storageLocation = StorageLocation.Documents, FileMode fileMode = FileMode.Read);
        void DeleteFile(string filename, StorageLocation storageLocation = StorageLocation.Documents);
        bool FileExists(string filename, StorageLocation storageLocation = StorageLocation.Documents);
    }
}
