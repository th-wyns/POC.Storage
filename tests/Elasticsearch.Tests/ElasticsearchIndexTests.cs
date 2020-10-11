using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace POC.Storage.Elasticsearch.Tests
{
    public class ElasticsearchIndexTests
    {
        static Connection Connection { get; } = Connection.Instance;
        public ElasticsearchSearchProvider Search { get; } = new ElasticsearchSearchProvider(Connection.ProjectId, Connection.ConnectionString);

        [Fact]
        public void Create()
        {
            var fieldName = "create-test";
            Search.DropAndCreate(fieldName);
        }

        [Fact]
        public async void Index()
        {
            var fieldName = "index-test";
            Search.DropAndCreate(fieldName);
            await Search.IndexStore.IndexAsync(fieldName, 1, "alma");
        }

        [Fact]
        public void IndexMass()
        {
            var docIds = Enumerable.Range(0, 50).Select(x => (long)x);
            var fieldIdPrefix = "mass-index-test";
            var fieldIds = new List<string>();
            var fields = new Dictionary<string, object?>();

            foreach (var fieldNum in Enumerable.Range(0, 5))
            {
                var fieldName = $"{fieldIdPrefix}-{fieldNum}";
                Search.DropAndCreate(fieldName);
                fields.Add(fieldName, $"value_{fieldName}");
                fieldIds.Add(fieldName);
            }
            Search.IndexStore.Index(docIds, fields);

            foreach (var fieldId in fieldIds)
            {
                Assert.True(Connection.IndexExists(fieldId));
            }
        }
    }
}
