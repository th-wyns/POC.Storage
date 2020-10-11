using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Elasticsearch
{

    /// <summary>
    /// Elasticsearch Search Provider Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.SearchProviderBase" />
    public class ElasticsearchSearchProvider : SearchProviderBase
    {
        internal Connection? Connection { get; private set; }


        /// <summary>
        /// Gets or sets the index store.
        /// </summary>
        /// <value>
        /// The index store.
        /// </value>
        public override IIndexStore IndexStore { get; protected set; }

        /// <summary>
        /// Gets the query orchestrator.
        /// </summary>
        /// <value>
        /// The query orchestrator.
        /// </value>
        public override IQueryOrchestrator QueryOrchestrator { get; }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <value>
        /// The storage identifier.
        /// </value>
        public string StorageId { get; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchSearchProvider"/> class.
        /// </summary>
        public ElasticsearchSearchProvider(string storageId, string connectionString)
        {
            Connection = new Connection(storageId, connectionString);
            IndexStore = new ElasticsearchIndexStore(Connection);
            QueryOrchestrator = new ElasticSearchQueryOrchestrator();
            StorageId = storageId;
            ConnectionString = connectionString;
        }
    }
}
