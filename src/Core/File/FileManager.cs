using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{

    /// <summary>
    /// File Manager Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IFileManager" />
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Gets or sets the file store.
        /// </summary>
        /// <value>
        /// The file store.
        /// </value>
        public IFileStore FileStore { get; set; }

        /// <summary>
        /// Gets or sets the binary provider.
        /// </summary>
        /// <value>
        /// The binary provider.
        /// </value>
        public IBinaryProvider BinaryProvider { get; set; }

        /// <summary>
        /// Gets the supported binary providers.
        /// </summary>
        /// <value>
        /// The supported binary providers.
        /// </value>
        public Dictionary<string, IBinaryProvider> SupportedBinaryProviders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        /// <param name="fileStore">The file store.</param>
        /// <param name="binaryProvider">The default binary provider.</param>
        /// <param name="supportedBinaryProviders">The supported binary providers.</param>
        public FileManager(IFileStore fileStore, IBinaryProvider binaryProvider, Dictionary<string, IBinaryProvider> supportedBinaryProviders)
        {
            FileStore = fileStore;
            BinaryProvider = binaryProvider;
            SupportedBinaryProviders = supportedBinaryProviders;
        }



        /// <summary>
        /// Adds the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="stream">The binary stream.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public async Task<long> CreateAsync(File file, Stream stream, CancellationToken cancellationToken)
        {
            // use file guid instead name
            var id = await FileStore.CreateAsync(file, cancellationToken);
            var reference = BinaryProvider.InitUpload(id.ToString(CultureInfo.CurrentCulture));
            await FileStore.InitAsync(id, BinaryProvider.AssemblyQualifiedName, reference, cancellationToken);
            await BinaryProvider.FinalizeUploadAsync(reference, stream, cancellationToken);
            await FileStore.FinalizeAsync(id, cancellationToken);
            return id;
        }

        /// <summary>
        /// Updates the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="stream">The binary stream.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public async Task UpdateAsync(File file, Stream stream, CancellationToken cancellationToken)
        {
            // use file guid instead name
            var queryOptions = new FileQueryOptions() { Type = file.Type };
            queryOptions.DocumentIds.Add(file.DocumentId);
            var filesToRemove = await FileStore.FindAsync(queryOptions, cancellationToken);
            foreach (var fileToRemove in filesToRemove)
            {
                BinaryProvider.Delete(fileToRemove.Reference);
            }

            await FileStore.UpdateAsync(file, cancellationToken);
            var reference = BinaryProvider.InitUpload(file.Id.ToString(CultureInfo.CurrentCulture));
            await FileStore.InitAsync(file.Id, BinaryProvider.AssemblyQualifiedName, reference, cancellationToken);
            await BinaryProvider.FinalizeUploadAsync(reference, stream, cancellationToken);
            await FileStore.FinalizeAsync(file.Id, cancellationToken);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task DeleteAsync(File file, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the file for the specified identifier.
        /// </summary>
        /// <param name="id">The file identifier.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The file object.
        /// </returns>
        public async Task<File?> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            var file = await FileStore.FindByIdAsync(id, cancellationToken);
            file?.SetBinaryProvider(SupportedBinaryProviders.ContainsKey(file.ProviderType) ? SupportedBinaryProviders[file.ProviderType] : null);
            return file;
        }

        /// <summary>
        /// Gets the file with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The file objects.
        /// </returns>
        public async Task<IList<File>> FindAsync(FileQueryOptions options, CancellationToken cancellationToken)
        {
            var files = await FileStore.FindAsync(options, cancellationToken);
            return files;
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
