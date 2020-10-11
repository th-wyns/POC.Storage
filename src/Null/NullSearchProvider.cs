using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null Search Provider Implementation 
    /// </summary>
    /// <seealso cref="POC.Storage.SearchProviderBase" />
    public class NullSearchProvider : SearchProviderBase
    {
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
        /// Initializes a new instance of the <see cref="NullSearchProvider"/> class.
        /// </summary>
        public NullSearchProvider(string storageId, string connectionString)
        {
            IndexStore = new NullIndexStore();
            QueryOrchestrator = new NullQueryOrchestrator();
        }
    }
}
