using Xunit;

namespace POC.Storage.Elasticsearch.Tests
{
    static class IndexHelper
    {
        internal static void DropAndCreate(this ElasticsearchSearchProvider search, string fieldName)
        {
            var connection = Connection.Instance;
            var field = new Field(fieldName);
            connection.DropIndex(field.Name);
            Assert.True(search.IndexStore.CreateAsync(field).Result);
            Assert.True(connection.IndexExists(field.Id));
        }
    }
}
