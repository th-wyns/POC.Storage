using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null Index Store Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IndexStoreBase" />
    public class NullIndexStore : IndexStoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullIndexStore"/> class.
        /// </summary>
        public NullIndexStore()
        {
        }


        /// <summary>
        /// Creates index.
        /// </summary>
        /// <param name="field"></param>
        public override async Task<bool> CreateAsync(Field field)
        {
            // TODO: add parameters to trace
            Trace.WriteLine("Null.CreateAsync");
            await Task.Yield();
            return true;
        }

        /// <summary>
        /// Deletes index.
        /// </summary>
        /// <param name="id"></param>
        public override void Delete(string id)
        {
            Trace.WriteLine("Null.Delete");
        }

        /// <summary>
        /// Deletes the specified documents.
        /// </summary>
        /// <param name="items">The items.</param>
        public override void Delete(ref IEnumerable<ContentBatchOperationDescriber> items)
        {
            Trace.WriteLine("Null.Delete");
        }

        /// <summary>
        /// Index value.
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="documentId"></param>
        /// <param name="value"></param>
        public override async Task<bool> IndexAsync(string fieldId, long documentId, object? value)
        {
            Trace.WriteLine("Null.IndexAsync");
            await Task.Yield();
            return true;
        }

        /// <summary>
        /// Index values.
        /// </summary>
        /// <param name="documentIds"></param>
        /// <param name="fields"></param>
        public override void Index(IEnumerable<long> documentIds, IDictionary<string, object?> fields)
        {
            Trace.WriteLine("Null.IndexMultiple");
        }

        /// <summary>
        /// Index values.
        /// </summary>
        /// <param name="documents"></param>
        public override void Index(IEnumerable<Document> documents)
        {
            Trace.WriteLine("Null.IndexDocuments");
        }

        /// <summary>
        /// Updates index.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        public override void Update(string id, object settings)
        {
            Trace.WriteLine("Null.Update");
        }
    }
}
