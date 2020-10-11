using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{
    /// <summary>
    /// Search Provider Base Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.ISearchProvider" />
    public abstract class SearchProviderBase : ISearchProvider
    {
        /// <summary>
        /// Gets the metadata provider.
        /// </summary>
        /// <value>
        /// The metadata provider.
        /// </value>
        protected IMetadataProvider? MetadataProvider { get; private set; }

        /// <summary>
        /// Gets or sets the index store.
        /// </summary>
        /// <value>
        /// The index store.
        /// </value>
        public abstract IIndexStore IndexStore { get; protected set; }

        /// <summary>
        /// Gets the query orchestrator.
        /// </summary>
        /// <value>
        /// The query orchestrator.
        /// </value>
        public abstract IQueryOrchestrator QueryOrchestrator { get; }

        // queryorchestrator uses fieldstore to locate indexes and validate fields

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProviderBase"/> class.
        /// </summary>
        public SearchProviderBase()
        {
        }

        /// <summary>
        /// Sets the metadata provider.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public void SetMetadataProvider(IMetadataProvider metadataProvider)
        {
            MetadataProvider = metadataProvider;
            QueryOrchestrator.SetMetadataProvider(metadataProvider);
        }

        #region Dispose
        /// <summary>
        /// Dispose flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the user manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
            }
        }
        #endregion
    }
}
