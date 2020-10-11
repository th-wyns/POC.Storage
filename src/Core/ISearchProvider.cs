using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Search Provider Interface
    /// </summary>
    /// <seealso cref="POC.Storage.IProvider" />
    public interface ISearchProvider : IProvider
    {

        /// <summary>
        /// Gets the index store.
        /// </summary>
        /// <value>
        /// The index store.
        /// </value>
        public IIndexStore IndexStore { get; } // contains single field query executor

        /// <summary>
        /// Gets the query orchestrator.
        /// </summary>
        /// <value>
        /// The query orchestrator.
        /// </value>
        public IQueryOrchestrator QueryOrchestrator { get; } // parses, splits, executes single field queries and joins result

        /// <summary>
        /// Sets the metadata provider.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public void SetMetadataProvider(IMetadataProvider metadataProvider);

    }
}
