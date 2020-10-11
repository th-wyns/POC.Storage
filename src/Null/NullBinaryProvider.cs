using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{

    /// <summary>
    /// Null Binary Provider Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.BinaryProviderBase" />
    public class NullBinaryProvider : BinaryProviderBase
    {
        private ConcurrentDictionary<string, byte[]> fileDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullBinaryProvider"/> class.
        /// </summary>
        public NullBinaryProvider(string connectionString, string projectId) : base(connectionString, projectId)
        {
            this.fileDictionary = new ConcurrentDictionary<string, byte[]>();
        }

        /// <summary>
        /// Initializes the upload.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// The reference for the binary.
        /// </returns>
        public override string InitUpload(string fileName)
        {
            Trace.WriteLine("NullBinaryProvider.InitUpload");
            this.fileDictionary.TryAdd(fileName, Array.Empty<byte>());
            return fileName ?? "null";
        }

        /// <summary>
        /// Finalizes the upload asynchronous.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public override async Task FinalizeUploadAsync(string reference, Stream stream, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullBinaryProvider.FinalizeUploadAsync");

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms, cancellationToken);
            ms.Seek(0, SeekOrigin.Begin);
            this.fileDictionary.AddOrUpdate(reference, (k) => ms.ToArray(), (k, v) => ms.ToArray());
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        public override Stream GetStream(string reference)
        {
            Trace.WriteLine("NullBinaryProvider.GetStream");
            return default!;
        }

        /// <summary>
        /// Deletes the specified reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public override void Delete(string reference)
        {
            Trace.WriteLine($"NullBinaryProvider.Delete");
        }
    }
}
