using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Interface for File object store implementations.
    /// </summary>
    public interface IFileStore : IDisposable
    {
        /// <summary>
        /// Adds the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task<long> CreateAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task UpdateAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Finalizes the specified file.
        /// </summary>
        /// <param name="id">The file id.</param>
        /// <param name="providerType">The assembly qualified name of the provider.</param>
        /// <param name="reference">The file reference.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task InitAsync(long id, string providerType, string reference, CancellationToken cancellationToken);

        /// <summary>
        /// Finalizes the specified file.
        /// </summary>
        /// <param name="id">The file id.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task FinalizeAsync(long id, CancellationToken cancellationToken);


        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task DeleteAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IList<ContentBatchOperationDescriber>> DeleteAllAsync(IList<File> files, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the file for the specified identifier.
        /// </summary>
        /// <param name="id">The file identifier.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The file object.</returns>
        Task<File> FindByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The file objects.</returns>
        Task<IList<File>> FindAsync(FileQueryOptions options, CancellationToken cancellationToken);
    }
}
