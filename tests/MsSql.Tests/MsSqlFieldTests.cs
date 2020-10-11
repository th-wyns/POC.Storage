using System.Threading;
using POC.Storage.Null;
using Xunit;

namespace POC.Storage.MsSql.Tests
{
    public class MsSqlFieldTests
    {
        static Connection Connection { get; } = Connection.Instance;
        public MsSqlMetadataProvider MsSqlMetadataProvider { get; } =
            new MsSqlMetadataProvider(Connection.ProjectId,
                Connection.ConnectionString,
                new NullBinaryProvider(default!, default!),
                default!, new NullIndexStore(),
                new NullAuditReportProvider());

        [Fact]
        public void InitSucceeded()
        {
            Assert.True(Connection.TableExists("Field"));
        }

        [Fact]
        public async void CreateWithNumber()
        {
            await MsSqlMetadataProvider.Field.CreateAsync(FieldUtilities.GetRandomField(FieldType.Number), CancellationToken.None);
        }

        [Fact]
        public async void CreateWithCode()
        {
            var field = FieldUtilities.GetRandomField(FieldType.Code);
            await MsSqlMetadataProvider.Field.CreateAsync(field, CancellationToken.None);
            var field2 = await MsSqlMetadataProvider.Field.FindByIdAsync(field.Id, CancellationToken.None);
            Assert.True(field.CodeConfiguration.SameAs(field2.CodeConfiguration));
        }
    }
}
