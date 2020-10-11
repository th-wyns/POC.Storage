using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{
    /// <summary>
    /// Base for Metadata provider implementations.
    /// </summary>
    public abstract class MetadataProviderBase : IMetadataProvider
    {

        /// <summary>
        /// Gets the Field manager.
        /// </summary>
        /// <value>
        /// The Field manager.
        /// </value>
        public abstract IFieldManager Field { get; }

        /// <summary>
        /// Gets the Document manager.
        /// </summary>
        /// <value>
        /// The Document manager.
        /// </value>
        public abstract IDocumentManager Document { get; }

        /// <summary>
        /// Gets the File manager.
        /// </summary>
        /// <value>
        /// The File manager.
        /// </value>
        public abstract IFileManager File { get; }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        protected string ProjectId { get; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        protected string ConnectionString { get; }

        /// <summary>
        /// Gets the index store.
        /// </summary>
        /// <value>
        /// The index store.
        /// </value>
        protected IIndexStore IndexStore { get; }

        /// <summary>
        /// Gets the audit report provider.
        /// </summary>
        /// <value>
        /// The audit report provider.
        /// </value>
        protected IAuditReportProvider AuditReportProvider { get; }

        /// <summary>
        /// Gets the binary provider.
        /// </summary>
        /// <value>
        /// The binary provider.
        /// </value>
        protected IBinaryProvider BinaryProvider { get; }

        /// <summary>
        /// Gets the supported binary providers.
        /// </summary>
        /// <value>
        /// The supported binary providers.
        /// </value>
        protected Dictionary<string, IBinaryProvider> SupportedBinaryProviders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataProviderBase"/> class.
        /// </summary>
        public MetadataProviderBase(string projectId, string connectionString, IBinaryProvider binaryProvider, Dictionary<string, IBinaryProvider> supportedBinaryProviders, IIndexStore indexStore, IAuditReportProvider auditReportProvider)
        {
            ProjectId = projectId;
            ConnectionString = connectionString;
            IndexStore = indexStore;
            AuditReportProvider = auditReportProvider;
            BinaryProvider = binaryProvider;
            SupportedBinaryProviders = supportedBinaryProviders;
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
