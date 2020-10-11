using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Interface for Document object store implementations.
    /// </summary>
    public interface IDocumentStore : IDisposable
    {
        /// <summary>
        /// Updates the document with the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="fields">The document fields to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task SetFieldsAsync(long id, IDictionary<string, object?> fields, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the documents with the specified identifiers.
        /// </summary>
        /// <param name="ids">The document ids.</param>
        /// <param name="fields">The document fields to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task SetFieldsAsync(IEnumerable<long> ids, IDictionary<string, object?> fields, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the parent identifier.
        /// </summary>
        /// <param name="ids">The document ids.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task SetParentIdAsync(IEnumerable<long> ids, string parentId, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<long> CreateAsync(Document document, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the specified documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<long> CreateAsync(IEnumerable<Document> documents, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the document with specified identifier.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task UpdateAsync(Document document, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the document with the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task DeleteAsync(Document id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document for the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        Task<Document> FindByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All fields.</returns>
        Task<IList<Document>> FindAllAsync(DocumentQueryOptions options, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the document with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All fields.</returns>
        Task<Dictionary<long, string>> GetDocumentIdsByIdAsync(DocumentQueryOptions options, CancellationToken cancellationToken);


        /// <summary>
        /// Counts the documents.
        /// </summary>
        /// <param name="parentIds">The parent ids. Empty array means count all documents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<int> CountAsync(IEnumerable<string?> parentIds, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the counts for all parents.
        /// </summary>
        /// <returns>The document counts segmented by all existing parent ids.</returns>
        Task<Dictionary<string, int>> CountsByParentsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Determines whether the field is used.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <c>true</c> if [the field is used]; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsFieldUsedAsync(Field field, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the field usage stats. Distincts by value and counts instances.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The field usage stats. Distincts by value and counts instances.
        /// </returns>
        Task<IDictionary<OptionalValue<object?>, int>> FieldUsageStatsAsync(Field field, CancellationToken cancellationToken);
    }
}
