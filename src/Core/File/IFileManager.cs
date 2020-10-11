using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Interface for File object store implementations.
    /// </summary>
    public interface IFileManager : IDisposable
    {
        /// <summary>
        /// Adds the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="stream">The binary stream.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task<long> CreateAsync(File file, Stream stream, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="stream">The binary stream.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task UpdateAsync(File file, Stream stream, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task DeleteAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the file for the specified identifier.
        /// </summary>
        /// <param name="id">The file identifier.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The file object.</returns>
        Task<File?> FindByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the file with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The file objects.</returns>
        Task<IList<File>> FindAsync(FileQueryOptions options, CancellationToken cancellationToken);
    }
}
