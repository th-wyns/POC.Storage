using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Interface for Metadata providers.
    /// </summary>
    public interface IMetadataProvider : IProvider
    {
        /// <summary>
        /// Gets the Field manager.
        /// </summary>
        /// <value>
        /// The Field store.
        /// </value>
        public IFieldManager Field { get; }

        /// <summary>
        /// Gets the Document manager.
        /// </summary>
        /// <value>
        /// The Document manager.
        /// </value>
        public IDocumentManager Document { get; }

        /// <summary>
        /// Gets the File manager.
        /// </summary>
        /// <value>
        /// The File manager.
        /// </value>
        public IFileManager File { get; }
    }
}
