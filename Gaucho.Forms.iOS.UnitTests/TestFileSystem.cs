using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Gaucho.Forms.Core.FileSystem;
using Gaucho.Forms.Core.Services;
using Gaucho.Forms.iOS.Services;
using NUnit.Framework;
using FileMode = Gaucho.Forms.Core.FileSystem.FileMode;

namespace Gaucho.Forms.iOS.UnitTests {
    [TestFixture]
    public class TestFileSystem {
        protected readonly byte[] TestBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
        protected const string Filename = "Testfile.file";
        private FileSystem _fileSystem;
        protected string TestString = "{\"Name\":\"TestName\",\"TestObject\":{\"Id\":\"0123456789\",\"Name\":\"TestObjectName\"}}";

        [SetUp]
        public void Init() {
            _fileSystem = new FileSystem();
            DeleteTestFiles();
        }

        [Test]
        public void OpenFileReadResource() {
            var stream = _fileSystem.OpenFile(Filename, StorageLocation.AppResource);
            Assert.NotNull(stream);
        }

        [Test]
        public async Task OpenFileReadStreamDocuments() {
            await CreateTestTextFile(StorageLocation.Documents);
            var stream = _fileSystem.OpenFile(Filename, StorageLocation.Documents);
            Assert.NotNull(stream);
        }

        [Test]
        public async Task OpenFileReadStreamCache() {
            await CreateTestTextFile(StorageLocation.Cache);
            var stream = _fileSystem.OpenFile(Filename, StorageLocation.Cache);
            Assert.NotNull(stream);
        }

        [Test]
        public void OpenFileWriteResource() {
            Assert.Throws<NotSupportedException>(() => _fileSystem.OpenFile(Filename, StorageLocation.AppResource, FileMode.Write));
        }

        [Test]
        public void OpenFileWriteStreamDocuments() {
            var stream = _fileSystem.OpenFile(Filename, StorageLocation.Documents, FileMode.Write);
            Assert.NotNull(stream);
        }

        [Test]
        public void OpenFileWriteStreamCache() {
            var stream = _fileSystem.OpenFile(Filename, StorageLocation.Cache, FileMode.Write);
            Assert.NotNull(stream);
        }

        [Test]
        public void DeleteFileReadAppResource() {
            Assert.Throws<NotSupportedException>(() => _fileSystem.DeleteFile(Filename, StorageLocation.AppResource));
        }

        [Test]
        public async Task DeleteFileDocuments() {
            await TestDeleteFile(StorageLocation.Documents);
        }

        [Test]
        public async Task DeleteFileCache() {
            await TestDeleteFile(StorageLocation.Cache);
        }

        [Test]
        public void FileExistsResources() {
            Assert.True(_fileSystem.FileExists(Filename, StorageLocation.AppResource));
        }

        [Test]
        public async Task FileExistsDocuments() {
            await TestFileExists(StorageLocation.Documents);
        }

        [Test]
        public async Task FileExistsCache() {
            await TestFileExists(StorageLocation.Cache);
        }

        private async Task TestDeleteFile(StorageLocation storageLocation) {
            await CreateTestTextFile(storageLocation);
            _fileSystem.DeleteFile(Filename, storageLocation);

            Assert.False(CheckFileExists(storageLocation));
        }

        private async Task TestFileExists(StorageLocation storageLocation) {
            await CreateTestTextFile(storageLocation);

            Assert.True(CheckFileExists(storageLocation));
        }

        public async Task CreateTestTextFile(StorageLocation location) {
            switch (location) {
                case StorageLocation.AppResource:
                    throw new NotImplementedException();
                case StorageLocation.Documents:
                    await CreateTestByteFileInternal(Encoding.UTF8.GetBytes(TestString));
                    break;
                case StorageLocation.Cache:
                    await CreateTestByteFileCache(Encoding.UTF8.GetBytes(TestString));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        public async Task CreateTestByteFile(StorageLocation location) {
            switch (location) {
                case StorageLocation.AppResource:
                    throw new NotImplementedException();
                    break;
                case StorageLocation.Documents:
                    await CreateTestByteFileInternal(TestBytes);
                    break;
                case StorageLocation.Cache:
                    await CreateTestByteFileCache(TestBytes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        public bool CheckFileExists(StorageLocation location) {
            string filesDirPath = null;
            switch (location) {
                case StorageLocation.AppResource:
                    filesDirPath = NSBundle.MainBundle.PathForResource(Filename, null);
                    break;
                case StorageLocation.Documents:
                    filesDirPath = GetDocumentPath();
                    break;
                case StorageLocation.Cache:
                    var cacheUrls = GetCachePath();
                    filesDirPath = GetCachePath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
            if (filesDirPath != null) {
                var path = Path.Combine(filesDirPath, Filename);
                return File.Exists(path);
            }
            return false;
        }

        private static string GetCachePath() {
            var cacheUrls = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
            return cacheUrls[0].Path;
        }

        private static string GetDocumentPath() {
            var docUrls = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
            return docUrls[0].Path;
        }

        private async Task CreateTestByteFileCache(byte[] bytes) {
            var cachePath = GetCachePath();
            var path = Path.Combine(cachePath, Filename);

            NSFileManager.DefaultManager.CreateFile(path, NSData.FromArray(bytes), (NSDictionary) null);
        }

        private async Task CreateTestByteFileInternal(byte[] bytes) {
            var cachePath = GetDocumentPath();
            var path = Path.Combine(cachePath, Filename);

            NSFileManager.DefaultManager.CreateFile(path, NSData.FromArray(bytes), (NSDictionary) null);
        }

        public void DeleteTestFiles() {
            DeleteFile(StorageLocation.Documents);
            DeleteFile(StorageLocation.Cache);
        }

        private void DeleteFile(StorageLocation storageLocation) {
            string filesDirPath = null;
            switch (storageLocation) {
                case StorageLocation.AppResource:
                    throw new NotSupportedException();
                case StorageLocation.Documents:
                    filesDirPath = GetDocumentPath();
                    break;
                case StorageLocation.Cache:
                    filesDirPath = GetCachePath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageLocation), storageLocation, null);
            }

            if (filesDirPath != null) {
                var path = Path.Combine(filesDirPath, Filename);
                if (File.Exists(path)) {
                    File.Delete(path);
                }
            }
        }
    }
}