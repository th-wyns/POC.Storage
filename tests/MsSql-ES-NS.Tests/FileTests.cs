using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using POC.Storage.Elasticsearch;
using POC.Storage.NetworkShare;
using POC.Storage.Null;
using Xunit;

namespace POC.Storage.MsSqlESNS.Tests
{
    public class FileTests
    {
        static Connection Connection { get; }
        static public Storage Storage { get; }

        static FileTests()
        {
            // make sure Connection is initialized first since it is Dropping the DB
            Connection = Connection.Instance;
            Storage = new Storage(Connection.ProjectId);
        }

        [Fact]
        public void InitSucceeded()
        {
            Assert.True(Connection.TableExists("File"));
        }

        [Fact]
        public async void Create()
        {
            await Storage.CreateRandomFileAsync(RndUtilities.NextInt32(), FileType.Native, findAndCheck: false);
        }

        [Fact]
        public async void FindById()
        {
            await Storage.CreateRandomFileAsync(RndUtilities.NextInt32(), FileType.Native);
        }

        [Fact]
        public async void Find()
        {
            // create files
            var docId1 = 1001; // be unqiue in the test file
            var docId2 = 1002; // be unqiue in the test file
            var files = new List<File>();
            files.Add(await Storage.CreateRandomFileAsync(docId1, FileType.Native));
            files.Add(await Storage.CreateRandomFileAsync(docId2, FileType.Native));
            for (int i = 1; i <= 10; i++)
            {
                files.Add(await Storage.CreateRandomFileAsync(docId1, FileType.Image));
            }
            for (int i = 1; i <= 5; i++)
            {
                files.Add(await Storage.CreateRandomFileAsync(docId2, FileType.Image));
            }

            // test options
            // DocumentIds
            var opts1 = new FileQueryOptions
            {
                DocumentIds = { docId1 }
            };
            var q1 = await Storage.Metadata.File.FindAsync(opts1, CancellationToken.None);
            Assert.Equal(q1.Select(f => f.Id), files.Where(f => f.DocumentId == docId1).Select(f => f.Id));

            // FileIds
            var fileIds = files.Select(f => f.Id).Take(2).ToList();
            var opts2 = new FileQueryOptions
            {
                Ids = { fileIds[0], fileIds[1] }
            };
            var q2 = await Storage.Metadata.File.FindAsync(opts2, CancellationToken.None);
            Assert.Equal(q2.Select(f => f.Id), files.Where(f => fileIds.Contains(f.Id)).Select(f => f.Id));

            // FileType
            var opts3 = new FileQueryOptions
            {
                Type = FileType.Native
            };
            var q3 = await Storage.Metadata.File.FindAsync(opts3, CancellationToken.None);
            Assert.Equal(q3.Select(f => f.Id), files.Where(f => f.Type == FileType.Native).Select(f => f.Id));

            // Indexes
            var indexes = new[] { 1, 2, 3 };
            var opts4 = new FileQueryOptions
            {
                Type = FileType.Image,
                Indexes = { 1, 2, 3 }
            };
            var q4 = await Storage.Metadata.File.FindAsync(opts4, CancellationToken.None);
            Assert.Equal(q4.Select(f => f.Id), files.Where(f => f.Type == FileType.Image && indexes.Contains(f.Index)).Select(f => f.Id));

            // Take, Skip
            var opts5 = new FileQueryOptions
            {
                Offset = 5,
                Limit = 5
            };
            var q5 = await Storage.Metadata.File.FindAsync(opts5, CancellationToken.None);
            Assert.Equal(q5.Select(f => f.Id), files.Skip(5).Take(5).Select(f => f.Id));
        }


        // TODO: move all test helpers to a seperate core project, since these are used by multiple test projects
        internal static Stream GenerateRandomStream(int? length = null)
        {
            var rnd = new System.Random();
            length = length ?? rnd.Next(1, 1024);
            var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, leaveOpen: true);
            writer.Write(NextString(rnd, length.Value));
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        internal static string NextString(System.Random rnd, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(System.Linq.Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        internal static bool StreamEquals(Stream self, Stream other)
        {
            if (self == other)
            {
                return true;
            }

            if (self == null || other == null)
            {
                throw new System.ArgumentNullException(self == null ? "self" : "other");
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

            return true;
        }
    }
}
