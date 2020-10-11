
namespace POC.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentBatchOperationDescriber
    {
        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public BaseStorageContent Content { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ContentBatchOperationDescriber"/> is succes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if succes; otherwise, <c>false</c>.
        /// </value>
        public ContentDeletionStateEnum State { get; set; } = default; 

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public object? Error { get; set; }
    }
}
