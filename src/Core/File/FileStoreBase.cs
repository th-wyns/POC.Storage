using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{
    /// <summary>
    /// File Store Base Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IFileStore" />
    public abstract class FileStoreBase : IFileStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStoreBase"/> class.
        /// </summary>
        public FileStoreBase()
        {
        }

        /// <summary>
        /// Adds the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public abstract Task<long> CreateAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public abstract Task UpdateAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Initializes the file store base.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public abstract Task InitAsync(long id, string provider, string reference, CancellationToken cancellationToken);

        /// <summary>
        /// Finalizes the specified file.
        /// </summary>
        /// <param name="id">The file id.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public abstract Task FinalizeAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public abstract Task DeleteAsync(File file, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public abstract Task<IList<ContentBatchOperationDescriber>> DeleteAllAsync(IList<File> files, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The file objects.
        /// </returns>
        public abstract Task<IList<File>> FindAsync(FileQueryOptions options, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the file for the specified identifier.
        /// </summary>
        /// <param name="id">The file identifier.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The file object.
        /// </returns>
        public abstract Task<File> FindByIdAsync(long id, CancellationToken cancellationToken);

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
