using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null File Store Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.FileStoreBase" />
    class NullFileStore : FileStoreBase
    {
        private ConcurrentBag<File> fileList;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullFileStore"/> class.
        /// </summary>
        public NullFileStore()
        {
            this.fileList = new ConcurrentBag<File>();
        }

        public async override Task<long> CreateAsync(File binary, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.CreateAsync");
            await Task.Yield();
            fileList.Add(binary);
            return fileList.Count;
        }

        public override Task UpdateAsync(File file, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.UpdateAsync");
            return default!;
        }

        public async override Task InitAsync(long id, string providerType, string reference, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.InitAsync");
            await Task.Yield();
        }

        public async override Task FinalizeAsync(long id, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.FinalizeAsync");
            await Task.Yield();
        }

        public override Task DeleteAsync(File binary, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.DeleteAsync");
            return default!;
        }

        public override Task<IList<ContentBatchOperationDescriber>> DeleteAllAsync(IList<File> files, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.DeleteAsync");
            return default!;
        }

        public override Task<IList<File>> FindAsync(FileQueryOptions options, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.FindAsync");
            return default!;
        }

        public override Task<File> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullFileStore.FindByIdAsync");
            return default!;
        }
    }
}
