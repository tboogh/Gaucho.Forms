using System;
using System.IO;
using Foundation;
using Gaucho.Forms.Core.Services;
using FileMode = Gaucho.Forms.Core.Services.FileMode;

namespace Gaucho.Forms.iOS.Services {
    public class FileSystem : IFileSystem {
        public Stream OpenFile(string filename, StorageLocation storageLocation = StorageLocation.Documents, FileMode fileMode = FileMode.Read) {
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    if (fileMode == FileMode.Write || fileMode == FileMode.CreateNew) {
                        throw new NotSupportedException();
                    }
                    return OpenFileStream(GetBundlePath(filename), fileMode);
                case StorageLocation.Documents:
                case StorageLocation.Cache:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            return OpenFileStream(GetFilePath(storageLocation, filename), fileMode);
        }

        public void DeleteFile(string filename, StorageLocation storageLocation = StorageLocation.Documents) {
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    throw new NotSupportedException();
                case StorageLocation.Documents:
                case StorageLocation.Cache:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            var path = GetFilePath(storageLocation, filename);
            if (NSFileManager.DefaultManager.FileExists(path)) {
                NSError error = null;
                NSFileManager.DefaultManager.Remove(path, out error);
                if (error != null) {
                    throw new Exception(error.Description);
                }
            }
        }

        public bool FileExists(string filename, StorageLocation storageLocation = StorageLocation.Documents) {
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    return GetBundlePath(filename) != null;
                case StorageLocation.Documents:
                case StorageLocation.Cache:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            var path = GetFilePath(storageLocation, filename);
            return NSFileManager.DefaultManager.FileExists(path);
        }

        private static string GetFilePath(StorageLocation storageLocation, string filename) {
            string rootPath = null;
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    throw new NotSupportedException();
                case StorageLocation.Documents:
                    rootPath = GetDocumentPath();
                    break;
                case StorageLocation.Cache:
                    rootPath = GetCachePath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }
            return Path.Combine(rootPath, filename);
        }

        private Stream OpenFileStream(string filePath, FileMode fileMode) {
            switch (fileMode) {
                case FileMode.Read:
                    return File.OpenRead(filePath);
                case FileMode.Write:
                    return File.OpenWrite(filePath);
                case FileMode.CreateNew:
                    return File.Open(filePath, System.IO.FileMode.CreateNew, FileAccess.Write);
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileMode), fileMode, null);
            }
        }

        private static string GetBundlePath(string filename) {
            return NSBundle.MainBundle.PathForResource(filename, null);
        }

        private static string GetCachePath() {
            var cacheUrls = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
            return cacheUrls[0].Path;
        }

        private static string GetDocumentPath() {
            var docUrls = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
            return docUrls[0].Path;
        }
    }
}
