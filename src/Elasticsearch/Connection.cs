using System;
using Elasticsearch.Net;
using Nest;

namespace POC.Storage.Elasticsearch
{
    class Connection
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        internal string ConnectionString { get; }

        /// <summary>
        /// Gets the storage identifier.
        /// </summary>
        /// <value>
        /// The storage identifier.
        /// </value>
        internal string StorageId { get; }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        internal ElasticClient Client { get; }

        internal Connection(string storageId, string connectionString)
        {
            StorageId = storageId;
            ConnectionString = connectionString;
            Client = InitClient();
        }

        ElasticClient InitClient()
        {
            // TODO: research connection pooling
            // https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/connection-pooling.html

            using var connection = new ConnectionSettings(new Uri(ConnectionString));
            // TODO: remove add config to disable cert validation for development purposes
            DisableCertificateValidation(connection);
            return new ElasticClient(connection);
        }

        internal string GetIndexId(string fieldId)
        {
            return $"{StorageId}__{fieldId}__Field".ToLowerInvariant();
        }

        static void DisableCertificateValidation(ConnectionSettings connectionSettings)
        {
            connectionSettings.ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                              .ServerCertificateValidationCallback(CertificateValidations.AllowAll);
        }
    }
}
