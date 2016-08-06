using System;
using System.IO;
using Android.App;
using Gaucho.Forms.Core.FileSystem;
using Gaucho.Forms.Core.Services;
using FileMode = Gaucho.Forms.Core.FileSystem.FileMode;

namespace Gaucho.Forms.Android.Services {
    public class FileSystem : IFileSystem {
        public Stream OpenFile(string filename, StorageLocation storageLocation = StorageLocation.Documents, FileMode fileMode = FileMode.Read) {
            string filesDirPath = null;
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    if (fileMode == FileMode.Write || fileMode == FileMode.CreateNew) {
                        throw new NotSupportedException();
                    }
                    return OpenAssetFileStream(filename);
                case StorageLocation.Documents:
                    filesDirPath = Application.Context.FilesDir.AbsolutePath;
                    break;
                case StorageLocation.Cache:
                    filesDirPath = Application.Context.CacheDir.AbsolutePath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            if (filesDirPath != null) {
                var path = Path.Combine(filesDirPath, filename);
                return OpenFileStream(path, fileMode);
            }
            return null;
        }

        public void DeleteFile(string filename, StorageLocation storageLocation = StorageLocation.Documents) {
            string filesDirPath = null;
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    throw new NotSupportedException();
                case StorageLocation.Documents:
                    filesDirPath = Application.Context.FilesDir.AbsolutePath;
                    break;
                case StorageLocation.Cache:
                    filesDirPath = Application.Context.CacheDir.AbsolutePath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            
            if (filesDirPath != null) {
                var path = Path.Combine(filesDirPath, filename);
                File.Delete(path);
            }
        }

        public bool FileExists(string filename, StorageLocation storageLocation = StorageLocation.Documents) {
            string filesDirPath = null;
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    var assetFileDescriptor = Application.Context.Assets.OpenFd(filename);
                    return assetFileDescriptor != null;
                case StorageLocation.Documents:
                    filesDirPath = Application.Context.FilesDir.AbsolutePath;
                    break;
                case StorageLocation.Cache:
                    filesDirPath = Application.Context.CacheDir.AbsolutePath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            if (filesDirPath != null) {
                var path = Path.Combine(filesDirPath, filename);
                return File.Exists(path);
            }
            return false;
        }

        private Stream OpenAssetFileStream(string filename) {
            try {
                return Application.Context.Assets.Open(filename);
            } catch (Exception e) {
                return null;
            }
        }

        private Stream OpenFileStream(string path, FileMode fileMode) {
            switch (fileMode) {
                case FileMode.Read:
                    try {
                        return File.OpenRead(path);
                    } catch (Exception) {
                        return null;
                    }
                    
                case FileMode.Write:
                    try {
                        return File.OpenWrite(path);
                    } catch (Exception) {
                        return null;
                    }
                case FileMode.CreateNew:
                    try {
                        return File.Open(path, System.IO.FileMode.CreateNew, FileAccess.Write);
                    } catch (Exception e) {
                        return null;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileMode), fileMode, null);
            }
        }
    }
}