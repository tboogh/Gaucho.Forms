using System.Threading.Tasks;
using Gaucho.Forms.Core.FileSystem;

namespace Gaucho.Forms.Core.Services {
    public interface IFileService {
        string ReadString(string filename, StorageLocation storageLocation = StorageLocation.Documents);
        Task<string> ReadStringAsync(string fileName, StorageLocation storageLocation = StorageLocation.Documents);
        Task<byte[]> ReadBytesAsync(string fileName, StorageLocation storageLocation = StorageLocation.Documents);
        Task WriteStringAsync(string fileName, string text, StorageLocation storageLocation = StorageLocation.Documents);
        void WriteString(string fileName, string text, StorageLocation storageLocation = StorageLocation.Documents);
        void WriteBytes(string fileName, byte[] data, StorageLocation storageLocation = StorageLocation.Documents);
        bool FileExists(string fileName, StorageLocation storageLocation = StorageLocation.Documents);
        void DeleteFile(string fileName, StorageLocation storageLocation = StorageLocation.Documents);
    }
}
