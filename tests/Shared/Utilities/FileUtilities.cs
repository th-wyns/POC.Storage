using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using POC.Storage.NetworkShare;
using POC.Storage.Tests;
using Xunit;

namespace POC.Storage
{
    static class FileUtilities
    {
        internal static File GetRandomFile(long documentId, FileType type, int index = 1)
        {
            return new File()
            {
                FileName = RndUtilities.NextString(100),
                DocumentId = documentId,
                DocumentIdentifier = RndUtilities.NextString(10),
                Index = index,
                Size = RndUtilities.Next(1024, 1024 * 1024),
                Type = type
            };
        }

        internal static async Task<File> CreateRandomFileAsync(this IMetadataProvider metadataProvider, long documentId, FileType type, int index = 1, bool findAndCheck = true)
        {
            var file = GetRandomFile(documentId, type, index);
            var inStream = BinaryUtilities.GenerateRandomStream((int)file.Size);
            var fileId = await metadataProvider.File.CreateAsync(file, inStream, CancellationToken.None);
            Assert.True(fileId > 0);

            if (findAndCheck)
            {
                var createdFile = await metadataProvider.File.FindByIdAsync(fileId, CancellationToken.None);
                Assert.NotNull(createdFile);
                Assert.True(file.FileEquals(createdFile!));
                Assert.True(createdFile!.Id > 0);
                Assert.True(BinaryUtilities.StreamEquals(inStream, createdFile.GetStream()));
                return createdFile!;
            }

            return new File() { Id = fileId };
        }

        internal static async Task<File> CreateRandomFileAsync(this Storage storage, long documentId, FileType type, int index = 1, bool findAndCheck = true)
        {
            return await storage.Metadata.CreateRandomFileAsync(documentId, type, index, findAndCheck: findAndCheck);
        }

        internal static bool FileEquals(this File file1, File file2)
        {
            return file1.FileName == file2.FileName && file1.DocumentId == file2.DocumentId && file1.DocumentIdentifier == file2.DocumentIdentifier &&
                file1.Index == file2.Index && file1.Size == file2.Size && file1.Type == file2.Type;
        }
    }
}
