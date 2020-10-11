using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using POC.Storage.Null;
using Xunit;
using Xunit.Abstractions;

namespace POC.Storage.MsSql.Tests
{
    public class MsSqlDocumentTests
    {
        private readonly ITestOutputHelper output;
        static Connection Connection { get; } = Connection.Instance;
        public MsSqlMetadataProvider MsSqlMetadataProvider { get; } =
            new MsSqlMetadataProvider(Connection.ProjectId,
                Connection.ConnectionString,
                new NullBinaryProvider(default!, default!),
                default!, new NullIndexStore(),
                new NullAuditReportProvider());

        public MsSqlDocumentTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void InitSucceeded()
        {
            Assert.True(Connection.TableExists("Document"));
        }

        [Fact]
        public async void Create()
        {
            var document = DocumentUtilities.GetRandomDocument();
            var documentId = await MsSqlMetadataProvider.Document.CreateAsync(document, CancellationToken.None);
            var createdDoc = await MsSqlMetadataProvider.Document.FindByIdAsync(documentId, CancellationToken.None);

            // TODO: create helper to verify all fields!
            Assert.True(document.ParentId == createdDoc.ParentId);
        }

        [Fact]
        public async void CreateWithNumber()
        {
            var fields = new[] { await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number) };
            var document = DocumentUtilities.GetRandomDocument(fields);
            var documentId = await MsSqlMetadataProvider.Document.CreateAsync(document, CancellationToken.None);
            var createdDoc = await MsSqlMetadataProvider.Document.FindByIdAsync(documentId, CancellationToken.None);

            // TODO: create helper to verify all fields!
            var fieldId = fields[0].Id;
            Assert.True(document.ParentId == createdDoc.ParentId);
            Assert.Equal(document.Fields[fieldId], decimal.ToInt32((decimal)createdDoc.Fields[fieldId]!));
        }

        [Fact]
        public async void CreateWithCode()
        {
            var fields = new[] { await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Code) };
            var document = DocumentUtilities.GetRandomDocument(fields);
            var documentId = await MsSqlMetadataProvider.Document.CreateAsync(document, CancellationToken.None);
            var createdDoc = await MsSqlMetadataProvider.Document.FindByIdAsync(documentId, CancellationToken.None);

            // TODO: create helper to verify all fields!
            var fieldId = fields[0].Id;
            Assert.True(document.ParentId == createdDoc.ParentId);
            Assert.True(((CodeFieldValue)document.Fields[fieldId]!).SameAs((CodeFieldValue)createdDoc.Fields[fieldId]!));
        }

        [Fact]
        public async void CreateWith1Number1FixedLengthTextOn1000Documents()
        {
            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };
            // create documents
            var documentIds = new List<long>();
            for (int i = 0; i < 1000; i++)
            {
                documentIds.Add(await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(fields), CancellationToken.None));
            }
        }

        [Fact]
        public async void SetFieldsWith1Number1FixedLengthTextOn1000Documents()
        {
            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };
            // create documents
            var documentIds = new List<long>();
            for (int i = 0; i < 1000; i++)
            {
                documentIds.Add(await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(fields), CancellationToken.None));
            }
            // set field value parameters
            var fieldValues = new Dictionary<string, object?>
            {
                { fields[0].Id, FieldUtilities.GetRandomValue(fields[0].Type) },
                { fields[1].Id, FieldUtilities.GetRandomValue(fields[1].Type) }
            };
            // set fields on documents
            var stopper = new System.Diagnostics.Stopwatch();
            stopper.Start();
            await MsSqlMetadataProvider.Document.SetFieldsAsync(documentIds, fieldValues, CancellationToken.None);
            output.WriteLine($"Update took: {stopper.ElapsedMilliseconds} ms");
        }

        [Fact]
        public async void SetParentId()
        {
            // create documents
            var documentIds = new List<long>();
            for (int i = 0; i < 10; i++)
            {
                var documentId = await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(), CancellationToken.None);
                documentIds.Add(documentId);
            }
            // set parent id on documents
            var newParentId = $"UPDATED_PARENTID_{DateTime.Now.Ticks}";
            var stopper = new System.Diagnostics.Stopwatch();
            stopper.Start();
            await MsSqlMetadataProvider.Document.SetParentIdAsync(documentIds, newParentId, CancellationToken.None);
            output.WriteLine($"Update took: {stopper.ElapsedMilliseconds} ms");
            // assert
            var opts = new DocumentQueryOptions();
            opts.Ids.UnionWith(documentIds);
            var documents = await MsSqlMetadataProvider.Document.FindAllAsync(opts, CancellationToken.None);
            foreach (var document in documents)
            {
                Assert.True(document.ParentId == newParentId);
            }
        }

        [Fact]
        public async void FindAll()
        {
            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };
            // create documents
            var documentIds = new List<long>();
            var documents = new List<Document>();
            for (int i = 0; i < 25; i++)
            {
                documents.Add(DocumentUtilities.GetRandomDocument(fields));
                documentIds.Add(await MsSqlMetadataProvider.Document.CreateAsync(documents, CancellationToken.None));
            }

            // find
            // TODO: check results
            await MsSqlMetadataProvider.Document.FindAllAsync(new DocumentQueryOptions(), CancellationToken.None);
            await MsSqlMetadataProvider.Document.FindAllAsync(new DocumentQueryOptions { Offset = 10, Limit = 10 }, CancellationToken.None);

            var opts = new DocumentQueryOptions();
            opts.FieldNames.Add(fields[0].Id);
            await MsSqlMetadataProvider.Document.FindAllAsync(opts, CancellationToken.None);

            var opts2 = new DocumentQueryOptions();
            opts2.ParentIds.Add(documents[0].ParentId);
            opts2.ParentIds.Add(documents[1].ParentId);
            await MsSqlMetadataProvider.Document.FindAllAsync(opts2, CancellationToken.None);


            var opts3 = new DocumentQueryOptions { Offset = 0, Limit = 10, OrderAsc = true, OrderBy = fields[0].Id };
            opts3.FieldNames.Add(fields[0].Id);
            opts3.Ids.Add(1);
            opts3.ParentIds.Add(documents[0].ParentId);
            opts3.ParentIds.Add(documents[1].ParentId);
            await MsSqlMetadataProvider.Document.FindAllAsync(opts3, CancellationToken.None);
        }

        [Fact]
        public async void Count()
        {
            Connection.Instance.TruncateDocumentsTable();

            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };
            // create documents
            var documentIds = new List<long>();
            var documents = new List<Document>();
            for (int i = 0; i < 25; i++)
            {
                var document = DocumentUtilities.GetRandomDocument(fields);
                document.ParentId = i >= 20 ? null : i >= 10 ? "TestParent" : document.ParentId;
                var documentId = await MsSqlMetadataProvider.Document.CreateAsync(document, CancellationToken.None);
                documentIds.Add(documentId);
            }

            var count1 = await MsSqlMetadataProvider.Document.CountAsync(Array.Empty<string>(), CancellationToken.None);
            var count2 = await MsSqlMetadataProvider.Document.CountAsync(new[] { "TestParent" }, CancellationToken.None);
            var count3 = await MsSqlMetadataProvider.Document.CountAsync(new string?[] { null }, CancellationToken.None);
            var count4 = await MsSqlMetadataProvider.Document.CountAsync(new string?[] { "TestParent", null }, CancellationToken.None);
            Assert.Equal(25, count1);
            Assert.Equal(10, count2);
            Assert.Equal(5, count3);
            Assert.Equal(15, count4);
        }

        [Fact]
        public async void CountsByParents()
        {
            Connection.Instance.TruncateDocumentsTable();

            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText)
            };
            // create documents
            var documentIds = new List<long>();
            var documents = new List<Document>();
            for (int i = 0; i < 25; i++)
            {
                var document = DocumentUtilities.GetRandomDocument(fields);
                document.ParentId = i >= 20 ? null : i >= 10 ? "TestParent" : document.ParentId;
                var documentId = await MsSqlMetadataProvider.Document.CreateAsync(document, CancellationToken.None);
                documentIds.Add(documentId);
            }

            var counts = await MsSqlMetadataProvider.Document.CountsByParentsAsync(CancellationToken.None);
            Assert.Equal(25, counts.Select(countByParent => countByParent.Value).Sum());
            Assert.Equal(10, counts["TestParent"]);
            Assert.Equal(5, counts[""]);
        }

        [Fact]
        public async void IsFieldUsed()
        {
            Connection.Instance.TruncateDocumentsTable();

            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Code),
            };

            // create document without field
            await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(), CancellationToken.None);

            // assert
            Assert.False(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[0], CancellationToken.None));
            Assert.False(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[1], CancellationToken.None));
            Assert.False(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[2], CancellationToken.None));

            // create document with Number
            await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(new[] { fields[0] }), CancellationToken.None);

            // assert
            Assert.True(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[0], CancellationToken.None));
            Assert.False(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[1], CancellationToken.None));
            Assert.False(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[2], CancellationToken.None));

            // create document with text
            DocumentUtilities.GetRandomDocument();
            await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(new[] { fields[1] }), CancellationToken.None);

            // assert
            Assert.True(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[0], CancellationToken.None));
            Assert.True(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[1], CancellationToken.None));
            Assert.False(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[2], CancellationToken.None));

            // create document with Code
            DocumentUtilities.GetRandomDocument();
            await MsSqlMetadataProvider.Document.CreateAsync(DocumentUtilities.GetRandomDocument(new[] { fields[2] }), CancellationToken.None);

            // assert
            Assert.True(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[0], CancellationToken.None));
            Assert.True(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[1], CancellationToken.None));
            Assert.True(await MsSqlMetadataProvider.Document.IsFieldUsedAsync(fields[2], CancellationToken.None));
        }

        [Fact]
        public async void FieldUsageStats()
        {
            Connection.Instance.TruncateDocumentsTable();

            // create fields
            var fields = new[]
            {
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Number),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.FixedLengthText),
                await MsSqlMetadataProvider.CreateRandomFieldAsync(FieldType.Code),
            };

            // create documents
            for (int i = 0; i < 7; i++)
            {
                var document = DocumentUtilities.GetRandomDocument();
                document.Fields[fields[0].Id] = i % 2;
                document.Fields[fields[1].Id] = (i % 3).ToString(CultureInfo.InvariantCulture);
                document.Fields[fields[2].Id] = (i < 4) ? null : FieldUtilities.GetTestCodeValue();
                await MsSqlMetadataProvider.Document.CreateAsync(document, CancellationToken.None);
            }

            // get stats & assert
            var fieldStat0 = await MsSqlMetadataProvider.Document.FieldUsageStatsAsync(fields[0], CancellationToken.None);
            Assert.Equal(4, fieldStat0[0M]);
            Assert.Equal(3, fieldStat0[1M]);
            var fieldStat1 = await MsSqlMetadataProvider.Document.FieldUsageStatsAsync(fields[1], CancellationToken.None);
            Assert.Equal(3, fieldStat1["0"]);
            Assert.Equal(2, fieldStat1["1"]);
            Assert.Equal(2, fieldStat1["2"]);
            var fieldStat2 = await MsSqlMetadataProvider.Document.FieldUsageStatsAsync(fields[2], CancellationToken.None);
            Assert.Equal(4, fieldStat2[null]);
            Assert.Equal(3, fieldStat2.First().Value);

        }
    }
}
