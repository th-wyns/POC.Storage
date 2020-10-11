using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Binary Provider Interface
    /// </summary>
    /// <seealso cref="POC.Storage.IProvider" />
    public interface IBinaryProvider : IProvider
    {
        /// <summary>
        /// Initializes the upload.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// The reference for the binary.
        /// </returns>
        string InitUpload(string fileName);

        /// <summary>
        /// Finalizes the upload asynchronous.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task FinalizeUploadAsync(string reference, Stream stream, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        Stream GetStream(string reference);

        /// <summary>
        /// Deletes the specified reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void Delete(string reference);

        /// <summary>
        /// Gets the name of the assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the assembly qualified.
        /// </value>
        string AssemblyQualifiedName { get; }
    }
}
