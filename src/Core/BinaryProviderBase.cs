using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{

    /// <summary>
    /// Binary Provider Base Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IBinaryProvider" />
    public abstract class BinaryProviderBase : IBinaryProvider
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        protected string ConnectionString { get; }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        protected string ProjectId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryProviderBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="projectId">The project identifier.</param>
        public BinaryProviderBase(string connectionString, string projectId)
        {
            ConnectionString = connectionString;
            ProjectId = projectId;
        }


        /// <summary>
        /// Initializes the upload.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// The reference for the binary.
        /// </returns>
        public abstract string InitUpload(string fileName);

        /// <summary>
        /// Finalizes the upload asynchronous.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public abstract Task FinalizeUploadAsync(string reference, Stream stream, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        public abstract Stream GetStream(string reference);

        /// <summary>
        /// Deletes the specified reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public abstract void Delete(string reference);


        private string? _assemblyQualifiedName = null;

        /// <summary>
        /// Gets the name of the assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the assembly qualified.
        /// </value>
        public string AssemblyQualifiedName
        {
            get
            {
                if (_assemblyQualifiedName == null)
                {
                    var type = GetType();
                    _assemblyQualifiedName = $"{type.FullName}, {type.Assembly.GetName().Name}";
                }
                return _assemblyQualifiedName;
            }
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
