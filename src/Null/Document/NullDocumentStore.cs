using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null implementation for Document object store.
    /// </summary>
    /// <seealso cref="POC.Storage.DocumentStoreBase" />
    public class NullDocumentStore : DocumentStoreBase
    {
        /// <summary>
        /// Gets the document list.
        /// </summary>
        /// <value>
        /// The document list.
        /// </value>
        public ConcurrentBag<Document> DocumentList { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullDocumentStore"/> class.
        /// </summary>
        public NullDocumentStore()
        {
            DocumentList = new ConcurrentBag<Document>();
        }

        /// <summary>
        /// Creates the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<long> CreateAsync(Document document, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullDocumentStore.Create");
            DocumentList.Add(document);
            await Task.Yield();
            return DocumentList.Count;
        }

        /// <summary>
        /// Creates the specified documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<long> CreateAsync(IEnumerable<Document> documents, CancellationToken cancellationToken)
        {
            Trace.WriteLine("NullDocumentStore.CreateMultiple");
            foreach (var document in documents)
            {
                await CreateAsync(document, cancellationToken);
            }
            return DocumentList.Count;
        }

        /// <summary>
        /// Deletes the document with the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public override async Task DeleteAsync(Document id, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.Delete");
        }

        /// <summary>
        /// Gets the document with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<IList<Document>> FindAllAsync(DocumentQueryOptions options, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.FindAll");
            return DocumentList.ToList();
        }

        /// <summary>
        /// Gets the document ids with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<Dictionary<long, string>> GetDocumentIdsByIdAsync(DocumentQueryOptions options, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.GetDocumentIdsByIdAsync");
            var result = new Dictionary<long, string>();
            if (!DocumentList.Any())
            {
                return result;
            }
            var documentIdFieldKey = DocumentList.First().Fields.Keys.FirstOrDefault(f => f.Contains("DocumentId", StringComparison.InvariantCultureIgnoreCase));
            if (string.IsNullOrWhiteSpace(documentIdFieldKey))
            {
                return result;
            }
            for (int d = 0; d < DocumentList.Count; d++)
            {
                result.Add(d, DocumentList.ToList()[d].Fields[documentIdFieldKey]?.ToString() ?? "");
            }
            return result;
        }


        /// <summary>
        /// Gets the document for the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<Document> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.FindById");
            return default!;
        }

        /// <summary>
        /// Updates the document with the specified identifier.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="fields">The document fields to update.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public override async Task SetFieldsAsync(long id, IDictionary<string, object?> fields, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.SetFields");
        }

        /// <summary>
        /// Updates the documents with the specified identifiers.
        /// </summary>
        /// <param name="ids">The document ids.</param>
        /// <param name="fields">The document fields to update.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public override async Task SetFieldsAsync(IEnumerable<long> ids, IDictionary<string, object?> fields, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.SetFieldsMultiple");
        }

        /// <summary>
        /// Sets the parent identifier.
        /// </summary>
        /// <param name="ids">The document ids.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public override async Task SetParentIdAsync(IEnumerable<long> ids, string parentId, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.SetParentId");
        }

        /// <summary>
        /// Updates the document with specified identifier.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public override async Task UpdateAsync(Document document, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.Update");
        }

        /// <summary>
        /// Counts the documents.
        /// </summary>
        /// <returns></returns>
        public override async Task<int> CountAsync(IEnumerable<string?> parentIds, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.Count");
            return 0;
        }

        /// <summary>
        /// Gets the counts for all parents.
        /// </summary>
        /// <returns>The document counts segmented by all existing parent ids.</returns>
        public override async Task<Dictionary<string, int>> CountsByParentsAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.CountsByParents");
            return new Dictionary<string, int>();
        }

        /// <summary>
        /// Determines whether the field is used.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <c>true</c> if [the field is used]; otherwise, <c>false</c>.
        /// </returns>
        public override async Task<bool> IsFieldUsedAsync(Field field, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.IsFieldUsed");
            return false;
        }

        /// <summary>
        /// Gets the field usage stats. Distincts by value and counts instances.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The field usage stats. Distincts by value and counts instances.
        /// </returns>
        public override async Task<IDictionary<OptionalValue<object?>, int>> FieldUsageStatsAsync(Field field, CancellationToken cancellationToken)
        {
            await Task.Yield();
            Trace.WriteLine("NullDocumentStore.FieldUsageStats");
            return new Dictionary<OptionalValue<object?>, int>();
        }
    }
}
