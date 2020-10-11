using System;
using System.IO;

namespace POC.Storage
{
    /// <summary>
    /// File object model.
    /// </summary>
    public class File : BaseStorageContent
    {
        /// <summary>
        /// Gets or sets the document id.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public long DocumentId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public string DocumentIdentifier { get; set; } = default!;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public FileType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        /// <value>
        /// The file size.
        /// </value>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        /// <value>
        /// The provider type.
        /// </value>
        public string ProviderType { get; } = default!;

        /// <summary>
        /// Gets or sets the file reference.
        /// </summary>
        /// <value>
        /// The file reference.
        /// </value>
        public string Reference { get; private set; } = default!;

        /// <summary>
        /// Gets or sets the file upload state.
        /// </summary>
        /// <value>
        /// True is the file is uploaded; otherwise false.
        /// </value>
        public bool IsFinalized { get; } = false;

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTimeOffset CreatedDate { get; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        /// <value>
        /// The modification date.
        /// </value>
        public DateTimeOffset ModifiedDate { get; }

        /// <summary>
        /// Gets or sets the PageId.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string PageId { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        public File()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="File" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="documentIdentifier"></param>
        /// <param name="providerType">Type of the provider.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="isFinalized">if set to <c>true</c> [is finalized].</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="modifiedDate">The modified date.</param>
        public File(long id, string documentIdentifier, string providerType, string reference, bool isFinalized, DateTimeOffset createdDate, DateTimeOffset modifiedDate)
        {
            Id = id;
            DocumentIdentifier = documentIdentifier;
            ProviderType = providerType;
            Reference = reference;
            IsFinalized = isFinalized;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            if (!IsFinalized)
            {
                throw new InvalidOperationException($"Upload is in progress.");
            }
            if (binaryProvider == null)
            {
                throw new NotSupportedException($"Binary provider '{ProviderType}' is not supported by the current configuration.");
            }
            return binaryProvider.GetStream(Reference);
        }

        IBinaryProvider? binaryProvider;
        internal void SetBinaryProvider(IBinaryProvider? binaryProvider)
        {
            this.binaryProvider = binaryProvider;
        }
    }

}
