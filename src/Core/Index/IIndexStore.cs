using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Interface for Index store implementations.
    /// </summary>
    public interface IIndexStore : IDisposable
    {
        /// <summary>
        /// Creates index.
        /// </summary>
        Task<bool> CreateAsync(Field field);

        /// <summary>
        /// Updates index.
        /// </summary>
        void Update(string id, object settings);

        /// <summary>
        /// Deletes index.
        /// </summary>
        void Delete(string id);

        /// <summary>
        /// Deletes the specified documents.
        /// </summary>
        /// <param name="items">The items.</param>
        void Delete(ref IEnumerable<ContentBatchOperationDescriber> items);

        /// <summary>
        /// Index value.
        /// </summary>
        Task<bool> IndexAsync(string fieldId, long documentId, object? value);

        /// <summary>
        /// Index values.
        /// </summary>
        void Index(IEnumerable<long> documentIds, IDictionary<string, object?> fields);

        /// <summary>
        /// Index values.
        /// </summary>
        void Index(IEnumerable<Document> documents);
    }
}
