using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null Metadata Provider Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.MetadataProviderBase" />
    public class NullMetadataProvider : MetadataProviderBase
    {
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

        /// <summary>
        /// Gets the field store.
        /// </summary>
        /// <value>
        /// The field store.
        /// </value>
        public IFieldStore FieldStore { get; }

        /// <summary>
        /// Gets the file store.
        /// </summary>
        /// <value>
        /// The file store.
        /// </value>
        public IFileStore FileStore { get; }

        /// <summary>
        /// Gets the document store.
        /// </summary>
        /// <value>
        /// The document store.
        /// </value>
        public IDocumentStore DocumentStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullMetadataProvider"/> class.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="connectionString"></param>
        /// <param name="binaryProvider"></param>
        /// <param name="supportedBinaryProviders"></param>
        /// <param name="indexStore"></param>
        /// <param name="auditReportProvider"></param>
        public NullMetadataProvider(string projectId, string connectionString, IBinaryProvider binaryProvider, Dictionary<string, IBinaryProvider> supportedBinaryProviders, IIndexStore indexStore, IAuditReportProvider auditReportProvider) : base(projectId, connectionString, binaryProvider, supportedBinaryProviders, indexStore, auditReportProvider)
        {
            FileStore = new NullFileStore();
            File = new FileManager(FileStore, binaryProvider, supportedBinaryProviders);
            FieldStore = new NullFieldStore();
            Field = new FieldManager(FieldStore, indexStore, auditReportProvider);
            DocumentStore = new NullDocumentStore();
            Document = new DocumentManager(DocumentStore, FieldStore, indexStore);
        }

    }
}
