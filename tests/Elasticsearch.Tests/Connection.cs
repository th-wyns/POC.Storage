using System;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using Xunit;

namespace POC.Storage.Elasticsearch.Tests
{
    class Connection
    {
        internal const string ProjectId = "POC.Storage.Tests";
        IConfigurationRoot Configuration { get; }
        internal string ConnectionString { get; }
        internal ElasticClient Client { get; }
        internal static Connection Instance = new Connection();

        Connection()
        {
            Configuration = GetConfiguration();
            ConnectionString = GetConnectionString();
            Client = InitClient();
        }

        IConfigurationRoot GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return configurationBuilder.Build();
        }

        string GetConnectionString()
        {
            return Configuration.GetConnectionString("POC.Storage.Index");
        }

        ElasticClient InitClient()
        {
            // TODO: research connection pooling
            // https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/connection-pooling.html

            using var connection = new ConnectionSettings(new Uri(ConnectionString));
            DisableCertificateValidation(connection);
            return new ElasticClient(connection);
        }

        internal bool IndexExists(string fieldId)
        {
            var indexId = GetIndexId(fieldId);
            var result = Client.Indices.Get(indexId);
            return result.Indices.Count > 0;
        }

        internal bool DropIndex(string fieldId)
        {
            if (IndexExists(fieldId))
            {
                var indexId = GetIndexId(fieldId);
                var result = Client.Indices.Delete(indexId);
                return !IndexExists(fieldId);
            }
            return true;
        }

        internal string GetIndexId(string fieldId)
        {
            return $"{ProjectId}__{fieldId}__field".ToLower(CultureInfo.InvariantCulture);
        }

        static void DisableCertificateValidation(ConnectionSettings connectionSettings)
        {
            connectionSettings.ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                              .ServerCertificateValidationCallback(CertificateValidations.AllowAll);
        }
    }
}
