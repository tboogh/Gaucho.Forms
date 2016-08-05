using System;
using System.Threading.Tasks;
using Gaucho.Forms.Core.Services;
using Gaucho.Forms.iOS.Services;
using NUnit.Framework;

namespace Gaucho.Forms.iOS.UnitTests {
    [TestFixture]
    public class TestFileService {
        private TestFileSystem _testFileSystem;
        protected readonly byte[] Bytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
        protected const string Filename = "Testfile.file";
        protected const string TextFilename = "testfile.json";
        protected string TestString = "{\"Name\":\"TestName\",\"TestObject\":{\"Id\":\"0123456789\",\"Name\":\"TestObjectName\"}}";

        private readonly IFileService _fileService;

        public TestFileService() {
            _testFileSystem = new TestFileSystem();
            _fileService = new FileService(new FileSystem());
        }

        [SetUp]
        public void Init() {
            DeleteTestFiles();
        }

        [Test]
        public async Task ReadBytesAsyncResource() {
            var bytes = await _fileService.ReadBytesAsync(Filename, StorageLocation.AppResource);
            Assert.NotNull(bytes);
        }

        [Test]
        public async Task ReadBytesAsyncDocuments() {
            await ReadBytesAsync(StorageLocation.Documents);
        }

        [Test]
        public async Task ReadBytesAsyncCache() {
            await ReadBytesAsync(StorageLocation.Cache);
        }

        [Test]
        public void WriteBytesesource() {
            Assert.Throws<NotSupportedException>(() => { _fileService.WriteBytes(Filename, Bytes, StorageLocation.AppResource); });
        }

        [Test]
        public void WriteBytesDocuments() {
            WriteBytes(StorageLocation.Documents);
        }

        [Test]
        public void WriteBytesCache() {
            WriteBytes(StorageLocation.Cache);
        }

        [Test]
        public async Task ReadStringAsyncResourceNotExists() {
            await ReadStringAsyncNotExists(StorageLocation.AppResource);
        }

        [Test]
        public async Task ReadStringAsyncDocumentsNotExists() {
            await ReadStringAsyncNotExists(StorageLocation.Documents);
        }

        [Test]
        public async Task ReadStringAsyncCacheNotExists() {
            await ReadStringAsyncNotExists(StorageLocation.Cache);
        }

        [Test]
        public async Task ReadBytesAsyncResourceNotExists() {
            await ReadBytesAsyncNotExists(StorageLocation.AppResource);
        }

        [Test]
        public async Task ReadBytesAsyncDocumentsNotExists() {
            await ReadBytesAsyncNotExists(StorageLocation.Documents);
        }

        [Test]
        public async Task ReadBytesAsyncCacheNotExists() {
            await ReadBytesAsyncNotExists(StorageLocation.Cache);
        }

        [Test]
        public async Task ReadStringAsyncResource() {
            var text = await _fileService.ReadStringAsync(TextFilename, StorageLocation.AppResource);
            Assert.NotNull(text);
        }

        [Test]
        public async Task ReadStringAsyncDocuments() {
            await ReadStringAsync(StorageLocation.Documents);
        }

        [Test]
        public async Task ReadStringAsyncCache() {
            await ReadStringAsync(StorageLocation.Cache);
        }

        [Test]
        public async Task WriteStringAsyncResource() {
            Exception exception = null;
            try {
                await _fileService.WriteStringAsync(Filename, TestString, StorageLocation.AppResource);
            } catch (Exception e) {
                exception = e;
            }
            Assert.NotNull(exception);
            Assert.True(exception.GetType() == typeof(NotSupportedException));
        }

        [Test]
        public async Task WriteStringAsyncDocuments() {
            await WriteString(StorageLocation.Documents);
        }

        [Test]
        public async Task WriteStringAsyncCache() {
            await WriteString(StorageLocation.Cache);
        }

        private async Task ReadBytesAsync(StorageLocation storageLocation) {
            await CreateTestByteFile(storageLocation);
            var bytes = await _fileService.ReadBytesAsync(Filename, storageLocation);
            Assert.NotNull(bytes);
        }

        private void WriteBytes(StorageLocation storageLocation) {
            _fileService.WriteBytes(Filename, Bytes, storageLocation);
            Assert.True(CheckFileExists(storageLocation));
        }

        private async Task WriteString(StorageLocation storageLocation) {
            await _fileService.WriteStringAsync(Filename, TestString, storageLocation);
            Assert.True(CheckFileExists(storageLocation));
        }

        private async Task ReadStringAsync(StorageLocation location) {
            await CreateTestTextFile(location);
            var bytes = await _fileService.ReadStringAsync(Filename, location);
            Assert.NotNull(bytes);
        }

        private async Task ReadStringAsyncNotExists(StorageLocation location) {
            var text = await _fileService.ReadStringAsync("BadFile.file", location);
            Assert.Null(text);
        }

        private async Task ReadBytesAsyncNotExists(StorageLocation location) {
            var data = await _fileService.ReadBytesAsync("BadFile.file", location);
            Assert.Null(data);
        }

        public Task CreateTestTextFile(StorageLocation location) {
            return _testFileSystem.CreateTestTextFile(location);
        }

        public Task CreateTestByteFile(StorageLocation location) {
            return _testFileSystem.CreateTestByteFile(location);
        }

        public bool CheckFileExists(StorageLocation location) {
            return _testFileSystem.CheckFileExists(location);
        }

        public void DeleteTestFiles() {
            _testFileSystem.DeleteTestFiles();
        }
    }
}
