using System.IO;
using System.Threading;
using POC.Storage.Null;
using Xunit;

namespace POC.Storage.MsSql.Tests
{
    public class MsSqlFileTests
    {
        static Connection Connection { get; } = Connection.Instance;
        public MsSqlMetadataProvider MsSqlMetadataProvider { get; } = new MsSqlMetadataProvider(Connection.ProjectId, Connection.ConnectionString, new NullBinaryProvider(default!, default!), default!, new NullIndexStore(), new NullAuditReportProvider());

        [Fact]
        public void InitSucceeded()
        {
            Assert.True(Connection.TableExists("File"));
        }

        [Fact]
        public async void Create()
        {
            var file = new File()
            {
                FileName = "alma",
                DocumentId = 2,
                Index = 1,
                Size = 2100,
                Type = FileType.Native
            };
            var id = await MsSqlMetadataProvider.File.CreateAsync(file, Stream.Null, CancellationToken.None);
            Assert.True(id > 0);
        }
    }
}
