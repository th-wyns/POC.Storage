using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{

    /// <summary>
    /// Document Manager Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IDocumentManager" />
    public class DocumentManager : IDocumentManager
    {

        /// <summary>
        /// Gets or sets the document store.
        /// </summary>
        /// <value>
        /// The document store.
        /// </value>
        public IDocumentStore DocumentStore { get; set; }


        /// <summary>
        /// Gets or sets the field store.
        /// </summary>
        /// <value>
        /// The field store.
        /// </value>
        public IFieldStore FieldStore { get; set; }

        /// <summary>
        /// Gets the index store.
        /// </summary>
        /// <value>
        /// The index store.
        /// </value>
        public IIndexStore IndexStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentManager" /> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <param name="fieldStore">The field store.</param>
        /// <param name="indexStore"></param>
        public DocumentManager(IDocumentStore documentStore, IFieldStore fieldStore, IIndexStore indexStore)
        {
            DocumentStore = documentStore;
            FieldStore = fieldStore;
            IndexStore = indexStore;
        }


        /// <summary>
        /// Updates the document with the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="fields">The document fields to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task SetFieldsAsync(long id, IDictionary<string, object?> fields, CancellationToken cancellationToken)
        {
            // TODO: check / validate fields existence (FieldStore.FindAll)
            await DocumentStore.SetFieldsAsync(id, fields, cancellationToken);
            IndexStore.Index(new[] { id }, fields);
            // TODO: set indexed to true
        }

        /// <summary>
        /// Updates the documents with the specified identifiers.
        /// </summary>
        /// <param name="ids">The document ids.</param>
        /// <param name="fields">The document fields to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task SetFieldsAsync(IEnumerable<long> ids, IDictionary<string, object?> fields, CancellationToken cancellationToken)
        {
            await DocumentStore.SetFieldsAsync(ids, fields, cancellationToken);
            IndexStore.Index(ids, fields);
            // TODO: set indexed to true
        }

        /// <summary>
        /// Sets the parent identifier.
        /// </summary>
        /// <param name="ids">The document ids.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task SetParentIdAsync(IEnumerable<long> ids, string parentId, CancellationToken cancellationToken)
        {
            await DocumentStore.SetParentIdAsync(ids, parentId, cancellationToken);
        }


        /// <summary>
        /// Creates the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The document id.</returns>
        public async Task<long> CreateAsync(Document document, CancellationToken cancellationToken)
        {
            var id = await DocumentStore.CreateAsync(document, cancellationToken);
            IndexStore.Index(new[] { id }, document.Fields);
            // TODO: set indexed to true
            return id;
        }


        /// <summary>
        /// Creates the specified documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<long> CreateAsync(IEnumerable<Document> documents, CancellationToken cancellationToken)
        {
            foreach (var document in documents)
            {
                // TODO: replace with mass insert imlpementation
                _ = await DocumentStore.CreateAsync(document, cancellationToken);
            }
            IndexStore.Index(documents);
            // TODO: set indexed to true

            return 0;
        }


        /// <summary>
        /// Updates the document with specified identifier.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task UpdateAsync(Document document, CancellationToken cancellationToken)
        {
            await DocumentStore.UpdateAsync(document, cancellationToken);
            IndexStore.Index(new[] { document.Id }, document.Fields);
            // TODO: set indexed to true
        }


        /// <summary>
        /// Deletes the document with the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task DeleteAsync(Document id, CancellationToken cancellationToken)
        {
            await DocumentStore.DeleteAsync(id, cancellationToken);
        }

        /// <summary>
        /// Gets the document for the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Document> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await DocumentStore.FindByIdAsync(id, cancellationToken);
        }


        /// <summary>
        /// Gets the document with the specified identifier.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IList<Document>> FindAllAsync(DocumentQueryOptions options, CancellationToken cancellationToken)
        {
            return await DocumentStore.FindAllAsync(options, cancellationToken);
        }

        /// <summary>
        /// Gets the document with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All fields.</returns>
        public async Task<Dictionary<long, string>> GetDocumentIdsByIdAsync(DocumentQueryOptions options, CancellationToken cancellationToken)
        {
            return await DocumentStore.GetDocumentIdsByIdAsync(options, cancellationToken);
        }

        /// <summary>
        /// Counts the documents.
        /// </summary>
        /// <param name="parentIds">The parent ids. Empty array means count all documents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<int> CountAsync(IEnumerable<string?> parentIds, CancellationToken cancellationToken)
        {
            return await DocumentStore.CountAsync(parentIds, cancellationToken);
        }

        /// <summary>
        /// Gets the counts for all parents.
        /// </summary>
        /// <returns>The document counts segmented by all existing parent ids.</returns>
        public async Task<Dictionary<string, int>> CountsByParentsAsync(CancellationToken cancellationToken)
        {
            return await DocumentStore.CountsByParentsAsync(cancellationToken);
        }

        /// <summary>
        /// Determines whether the field is used.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <c>true</c> if the field is used; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsFieldUsedAsync(Field field, CancellationToken cancellationToken)
        {
            return await DocumentStore.IsFieldUsedAsync(field, cancellationToken);
        }

        /// <summary>
        /// Gets the field usage stats. Distincts by value and counts instances.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IDictionary<OptionalValue<object?>, int>> FieldUsageStatsAsync(Field field, CancellationToken cancellationToken)
        {
            return await DocumentStore.FieldUsageStatsAsync(field, cancellationToken);
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
