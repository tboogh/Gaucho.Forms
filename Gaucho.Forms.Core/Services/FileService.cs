using System;
using System.IO;
using System.Threading.Tasks;

namespace Gaucho.Forms.Core.Services {
    public class FileService : IFileService {
        private readonly IFileSystem _fileSystem;

        public FileService(IFileSystem fileSystem) {
            _fileSystem = fileSystem;
        }

        public string ReadString(string filename, StorageLocation storageLocation = StorageLocation.Documents) {
            try {
                var stream = _fileSystem.OpenFile(filename, storageLocation);
                if (stream == null) {
                    return null;
                }
                using (var sr = new StreamReader(stream)) {
                    var text = sr.ReadToEnd();
                    return text;
                }
            } catch (Exception) {
                return null;
            }
        }

        public async Task<string> ReadStringAsync(string fileName, StorageLocation storageLocation = StorageLocation.Documents) {
            try {
                var stream = _fileSystem.OpenFile(fileName, storageLocation);
                if (stream == null) {
                    return null;
                }
                using (var sr = new StreamReader(stream)) {
                    var text = await sr.ReadToEndAsync();
                    return text;
                }
            } catch (Exception) {
                return null;
            }
        }

        public async Task<byte[]> ReadBytesAsync(string fileName, StorageLocation storageLocation = StorageLocation.Documents) {
            try {
                var stream = _fileSystem.OpenFile(fileName, storageLocation);
                if (stream == null) {
                    return null;
                }
                using (var ms = new MemoryStream()) {
                    await stream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            } catch (Exception) {
                return null;
            }
        }

        public async Task WriteStringAsync(string fileName, string text, StorageLocation storageLocation = StorageLocation.Documents) {
            var stream = _fileSystem.OpenFile(fileName, storageLocation, FileMode.CreateNew);
            if (stream == null) {
                return;
            }
            using (var sw = new StreamWriter(stream)) {
                await sw.WriteAsync(text);
            }
        }
        
        public void WriteString(string fileName, string text, StorageLocation storageLocation = StorageLocation.Documents) {
            var stream = _fileSystem.OpenFile(fileName, storageLocation, FileMode.CreateNew);
            if (stream == null) {
                return;
            }
            using (var sw = new StreamWriter(stream)) {
                sw.Write(text);
            }
        }

        public void WriteBytes(string fileName, byte[] data, StorageLocation storageLocation = StorageLocation.Documents) {
            var stream = _fileSystem.OpenFile(fileName, storageLocation, FileMode.CreateNew);
            if (stream == null) {
                return;
            }
            using (var bw = new BinaryWriter(stream)) {
                bw.Write(data);
            }
        }

        public bool FileExists(string fileName, StorageLocation storageLocation = StorageLocation.Documents) {
            return _fileSystem.FileExists(fileName, storageLocation);
        }

        public void DeleteFile(string fileName, StorageLocation storageLocation = StorageLocation.Documents) {
            _fileSystem.DeleteFile(fileName, storageLocation);
        }
	}
}