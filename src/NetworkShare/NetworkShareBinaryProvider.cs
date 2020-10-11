using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IOFile = System.IO.File;

namespace POC.Storage.NetworkShare
{

    /// <summary>
    /// Network Share Binary Provider Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.BinaryProviderBase" />
    public class NetworkShareBinaryProvider : BinaryProviderBase
    {
        /// <summary>
        /// Gets the network share path.
        /// </summary>
        /// <value>
        /// The network share path.
        /// </value>
        public string NetworkSharePath { get; }

        /// <summary>
        /// Gets the root path.
        /// </summary>
        /// <value>
        /// The root path.
        /// </value>
        public string RootPath { get; }

        public NetworkShareBinaryProvider(string connectionString, string projectId) : base(connectionString, projectId)
        {
            NetworkSharePath = connectionString;
            RootPath = Path.Combine(NetworkSharePath, ProjectId);
            Init();
        }

        void Init()
        {
            Directory.CreateDirectory(RootPath);
            // create sub folders for file types
        }

        /// <summary>
        /// Initializes the upload.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public override string InitUpload(string fileName)
        {
            var path = Path.Combine(RootPath, fileName);
            using var file = new FileStream(path, FileMode.CreateNew);
            return path;
        }

        /// <summary>
        /// Finalizes the upload asynchronous.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async override Task FinalizeUploadAsync(string reference, Stream stream, CancellationToken cancellationToken)
        {
            using var fileStream = new FileStream(reference, FileMode.Open, FileAccess.Write);
            await stream.CopyToAsync(fileStream, cancellationToken);
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        public override Stream GetStream(string reference)
        {
            var fileStream = new FileStream(reference, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        /// <summary>
        /// Deletes the specified reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public override void Delete(string reference)
        {
            IOFile.Delete(reference);
        }
    }
}
