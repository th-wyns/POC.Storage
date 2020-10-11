using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace POC.Storage.MsSqlESNS.Tests
{
    public class DocumentTests
    {
        private readonly ITestOutputHelper output;

        static Connection Connection { get; }
        static public Storage Storage { get; } = new Storage(Connection.ProjectId);

        static DocumentTests()
        {
            // make sure Connection is initialized first since it is Dropping the DB
            Connection = Connection.Instance;
            Storage = new Storage(Connection.ProjectId);
        }

        public DocumentTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void InitSucceeded()
        {
            Assert.True(Connection.TableExists("Document"));
        }

        [Fact]
        public async void CreateWith1Number1FixedLengthText()
        {
            // create fields
            var fields = new[]
            {
                await Storage.Metadata.CreateRandomFieldAsync(FieldType.Number),
                await Storage.Metadata.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };

            // create document
            await Storage.Metadata.Document.CreateAsync(DocumentUtilities.GetRandomDocument(fields), CancellationToken.None); ;
        }

        [Fact]
        public async void CreateBulkWith1Number1FixedLengthTextOn1000Documents()
        {
            // create fields
            var fields = new[]
            {
                await Storage.Metadata.CreateRandomFieldAsync(FieldType.Number),
                await Storage.Metadata.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };

            // create documents
            var documents = new List<Document>();
            for (int i = 0; i < 1000; i++)
            {
                documents.Add(DocumentUtilities.GetRandomDocument(fields));
            }

            var stopper = new System.Diagnostics.Stopwatch();
            stopper.Start();
            await Storage.Metadata.Document.CreateAsync(documents, CancellationToken.None);
            output.WriteLine($"Create took: {stopper.ElapsedMilliseconds} ms");
        }

        [Fact]
        public async void SetFieldsWith1Number1FixedLengthTextOn1000Documents()
        {
            // create fields
            var fields = new[]
            {
                await Storage.Metadata.CreateRandomFieldAsync(FieldType.Number),
                await Storage.Metadata.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };
            // create documents
            var documents = new List<Document>();
            for (int i = 0; i < 1000; i++)
            {
                documents.Add(DocumentUtilities.GetRandomDocument(fields));
            }
            await Storage.Metadata.Document.CreateAsync(documents, CancellationToken.None);

            // set field value parameters
            var fieldValues = new Dictionary<string, object?>
            {
                { fields[0].Id, FieldUtilities.GetRandomValue(fields[0].Type) },
                { fields[1].Id, FieldUtilities.GetRandomValue(fields[1].Type) }
            };
            // set fields on documents
            var stopper = new System.Diagnostics.Stopwatch();
            stopper.Start();
            await Storage.Metadata.Document.SetFieldsAsync(documents.Select(document => document.Id), fieldValues, CancellationToken.None);
            output.WriteLine($"Update took: {stopper.ElapsedMilliseconds} ms");
        }
    }
}
