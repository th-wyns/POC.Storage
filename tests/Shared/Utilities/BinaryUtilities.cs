using System;
using System.IO;
using System.Threading;

namespace POC.Storage
{
    internal static class BinaryUtilities
    {
        static Random rnd = new Random();
        const int maxFileSize = 1024 * 1024 * 10;

        internal static Stream GenerateRandomStream(int? length = null)
        {
            length = length ?? rnd.Next(1, maxFileSize);
            var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, leaveOpen: true);
            writer.Write(RndUtilities.NextString(length.Value));
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        internal static long GetSize(string path)
        {
            return new FileInfo(path).Length;
        }

        internal static Stream GetStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        internal static bool Exists(string path)
        {
            return new FileInfo(path).Exists;
        }

        internal static string CreateRandomBinary(this BinaryProviderBase provider, string? documentIdentifier = null, string? fileName = null, FileType fileType = FileType.TextExtract, Stream? stream = null)
        {
            fileName = fileName ?? Guid.NewGuid().ToString();
            documentIdentifier = documentIdentifier ?? Guid.NewGuid().ToString();
            stream = stream ?? GenerateRandomStream(maxFileSize);
            var reference = provider.InitUpload(fileName);
            provider.FinalizeUploadAsync(reference, stream, CancellationToken.None).Wait();
            return reference;
        }

        internal static bool StreamEquals(this Stream self, Stream other)
        {
            self.Seek(0, SeekOrigin.Begin);
            other.Seek(0, SeekOrigin.Begin);

            if (self == other)
            {
                return true;
            }

            if (self == null || other == null)
            {
                throw new ArgumentNullException(self == null ? "self" : "other");
            }

            if (self.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < self.Length; i++)
            {
                int aByte = self.ReadByte();
                int bByte = other.ReadByte();
                if (aByte.CompareTo(bByte) != 0)
                {
                    return false;
                }
            }

            self.Seek(0, SeekOrigin.Begin);
            other.Seek(0, SeekOrigin.Begin);

            return true;
        }
    }
}
