using System;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{

    /// <summary>
    /// Query Orchestrator Base Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IQueryOrchestrator" />
    public abstract class QueryOrchestratorBase : IQueryOrchestrator
    {
        IMetadataProvider? MetadataProvider { get; set; }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryOrchestratorBase"/> class.
        /// </summary>
        protected QueryOrchestratorBase()
        {
            MetadataProvider = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryOrchestratorBase"/> class.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        protected QueryOrchestratorBase(IMetadataProvider metadataProvider) : this()
        {
            MetadataProvider = metadataProvider;
        }

        /// <summary>
        /// Sets the metadata provider.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public void SetMetadataProvider(IMetadataProvider metadataProvider)
        {
            MetadataProvider = metadataProvider;
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
