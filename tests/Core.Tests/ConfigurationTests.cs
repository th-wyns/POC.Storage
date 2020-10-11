using Microsoft.Extensions.Configuration;
using POC.Storage.Elasticsearch;
using POC.Storage.MsSql;
using POC.Storage.NetworkShare;
using POC.Storage.Null;
using Xunit;

namespace POC.Storage.Tests
{
    public class ConfigurationTests
    {
        readonly string ProjectId = "POC.Storage.Tests";

        [Fact]
        public void AppSettingsBasedConfigurationWithDefaultAndNullProvider()
        {
            var configuration = new AppSettingsBasedConfiguration(ProjectId, true).Init();
            Assert.True(configuration.DbConnectionString == "TEST_DB_CONNECTIONSTRING");
            Assert.True(configuration.IndexConnectionString == "TEST_INDEX_CONNECTIONSTRING");
            Assert.True(configuration.BinaryConnectionString == "TEST_BINARY_CONNECTIONSTRING");
            Assert.True(configuration.MetadataProviderAssemblyQualifiedName == "POC.Storage.Null.NullMetadataProvider, POC.Storage.Null");
            Assert.True(configuration.SearchProviderAssemblyQualifiedName == "POC.Storage.Null.NullSearchProvider, POC.Storage.Null");
            Assert.True(configuration.AuditReportProviderAssemblyQualifiedName == "POC.Storage.Null.NullAuditReportProvider, POC.Storage.Null");
            Assert.True(configuration.MetadataProvider.GetType() == typeof(NullMetadataProvider));
            Assert.True(configuration.SearchProvider.GetType() == typeof(NullSearchProvider));
            Assert.True(configuration.AuditReportProvider.GetType() == typeof(NullAuditReportProvider));
            Assert.True(configuration.BinaryProvider.GetType() == typeof(NullBinaryProvider));
        }

        [Fact]
        public void AppSettingsBasedConfigurationWithOverrideAndMsSqlEsProvider()
        {
            var configuration = new AppSettingsBasedConfiguration(ProjectId, "appsettings_mssql-es-ns.json", true).Init();
            Assert.Contains("Data Source=.;Integrated Security=true;MultipleActiveResultSets=true", configuration.DbConnectionString, System.StringComparison.InvariantCulture);
            Assert.True(configuration.IndexConnectionString == "http://localhost:9200");
            Assert.True(configuration.BinaryConnectionString == "\\\\localhost\\C$");
            Assert.True(configuration.MetadataProviderAssemblyQualifiedName == "POC.Storage.MsSql.MsSqlMetadataProvider, POC.Storage.MsSql");
            Assert.True(configuration.SearchProviderAssemblyQualifiedName == "POC.Storage.Elasticsearch.ElasticsearchSearchProvider, POC.Storage.Elasticsearch");
            Assert.True(configuration.AuditReportProviderAssemblyQualifiedName == "POC.Storage.MsSql.MsSqlAuditReportProvider, POC.Storage.MsSql");
            Assert.True(configuration.MetadataProvider.GetType() == typeof(MsSqlMetadataProvider));
            Assert.True(configuration.SearchProvider.GetType() == typeof(ElasticsearchSearchProvider));
            Assert.True(configuration.AuditReportProvider.GetType() == typeof(MsSqlAuditReportProvider));
            Assert.True(configuration.BinaryProvider.GetType() == typeof(NetworkShareBinaryProvider));
        }

        [Fact]
        public void AppSettingsBasedConfigurationWithProvidedConfigAndNullProvider()
        {
            var c = new ConfigurationBuilder().AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            // TODO: make a performance test with 1000000 iterations
            //for (int i = 0; i < 1000000; i++)
            //{
            var configuration = new AppSettingsBasedConfiguration(ProjectId, c).Init();
            Assert.True(configuration.DbConnectionString == "TEST_DB_CONNECTIONSTRING");
            Assert.True(configuration.IndexConnectionString == "TEST_INDEX_CONNECTIONSTRING");
            Assert.True(configuration.BinaryConnectionString == "TEST_BINARY_CONNECTIONSTRING");
            Assert.True(configuration.MetadataProviderAssemblyQualifiedName == "POC.Storage.Null.NullMetadataProvider, POC.Storage.Null");
            Assert.True(configuration.SearchProviderAssemblyQualifiedName == "POC.Storage.Null.NullSearchProvider, POC.Storage.Null");
            Assert.True(configuration.AuditReportProviderAssemblyQualifiedName == "POC.Storage.Null.NullAuditReportProvider, POC.Storage.Null");
            Assert.True(configuration.MetadataProvider.GetType() == typeof(NullMetadataProvider));
            Assert.True(configuration.SearchProvider.GetType() == typeof(NullSearchProvider));
            Assert.True(configuration.AuditReportProvider.GetType() == typeof(NullAuditReportProvider));
            Assert.True(configuration.BinaryProvider.GetType() == typeof(NullBinaryProvider));
            //}
        }
    }
}
