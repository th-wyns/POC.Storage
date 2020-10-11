using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{

    /// <summary>
    /// Index Store Base Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IIndexStore" />
    public abstract class IndexStoreBase : IIndexStore
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexStoreBase"/> class.
        /// </summary>
        protected IndexStoreBase()
        {
        }

        /// <summary>
        /// Creates index.
        /// </summary>
        /// <param name="field"></param>
        public abstract Task<bool> CreateAsync(Field field);

        /// <summary>
        /// Deletes index.
        /// </summary>
        /// <param name="id"></param>
        public abstract void Delete(string id);

        /// <summary>
        /// Deletes the specified documents.
        /// </summary>
        /// <param name="items">The items.</param>
        public abstract void Delete(ref IEnumerable<ContentBatchOperationDescriber> items);

        /// <summary>
        /// Index value.
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="documentId"></param>
        /// <param name="value"></param>
        public abstract Task<bool> IndexAsync(string fieldId, long documentId, object? value);

        /// <summary>
        /// Index values.
        /// </summary>
        /// <param name="documentIds"></param>
        /// <param name="fields"></param>
        public abstract void Index(IEnumerable<long> documentIds, IDictionary<string, object?> fields);

        /// <summary>
        /// Index values.
        /// </summary>
        /// <param name="documents"></param>
        public abstract void Index(IEnumerable<Document> documents);

        /// <summary>
        /// Updates index.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        public abstract void Update(string id, object settings);

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
