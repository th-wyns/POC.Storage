using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.MsSql
{
    /// <summary>
    /// MsSql implementation for Metadata provider.
    /// </summary>
    /// <seealso cref="POC.Storage.IMetadataProvider" />
    public class MsSqlMetadataProvider : MetadataProviderBase
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        internal Connection Connection { get; }

        /// <summary>
        /// Gets the Field manager.
        /// </summary>
        /// <value>
        /// The Field manager.
        /// </value>
        public override IFieldManager Field { get; }
        /// <summary>
        /// Gets the Document manager.
        /// </summary>
        /// <value>
        /// The Document manager.
        /// </value>
        public override IDocumentManager Document { get; }

        /// <summary>
        /// Gets the File manager.
        /// </summary>
        /// <value>
        /// The File manager.
        /// </value>
        public override IFileManager File { get; }

        IFieldStore FieldStore { get; }
        IFileStore FileStore { get; }
        IDocumentStore DocumentStore { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlMetadataProvider"/> class.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="binaryProvider">The default binary provider.</param>
        /// <param name="supportedBinaryProviders">The supported binary providers.</param>
        /// <param name="indexStore">The index store.</param>
        /// <param name="auditReportProvider">The audit report provider.</param>
        public MsSqlMetadataProvider(string projectId, string connectionString, IBinaryProvider binaryProvider, Dictionary<string, IBinaryProvider> supportedBinaryProviders, IIndexStore indexStore, IAuditReportProvider auditReportProvider) : base(projectId, connectionString, binaryProvider, supportedBinaryProviders, indexStore, auditReportProvider)
        {
            Connection = new Connection(projectId, connectionString);
            FieldStore = new MsSqlFieldStore(Connection);
            Field = new FieldManager(FieldStore, indexStore, auditReportProvider);
            FileStore = new MsSqlFileStore(Connection);
            File = new FileManager(FileStore, binaryProvider, supportedBinaryProviders);
            DocumentStore = new MsSqlDocumentStore(Connection, (MsSqlFieldStore)FieldStore);
            Document = new DocumentManager(DocumentStore, FieldStore, indexStore);
        }
    }
}
