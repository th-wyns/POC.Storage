using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;

namespace POC.Storage.Elasticsearch
{
    /// <summary>
    /// Elasticsearch Index Store Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IndexStoreBase" />
    public class ElasticsearchIndexStore : IndexStoreBase
    {
        Connection Connection { get; }

        internal ElasticsearchIndexStore(Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Creates the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        public async override Task<bool> CreateAsync(Field field)
        {
            var state = new IndexState();
            //var settings = state.Settings;
            //settings.NumberOfReplicas = 2;
            //settings.NumberOfShards = 1;
            //settings.Add("merge.policy.merge_factor", "10");
            //settings.Add("search.slowlog.threshold.fetch.warn", "1s");
            var fieldIndexId = Connection.GetIndexId(field.Id);
            var createIndexResponse = await Connection.Client.Indices
                .CreateAsync(fieldIndexId, c => c.Index(fieldIndexId).InitializeUsing(state));

            if (createIndexResponse.ApiCall.Success && createIndexResponse.IsValid)
            {
                return true;
            }
            else
            {
                Trace.Fail($"Cannot create index for field: {field.Id} because {createIndexResponse.DebugInformation}", createIndexResponse.OriginalException.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public override void Delete(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified documents.
        /// </summary>
        /// <param name="items">The items.</param>
        public override void Delete(ref IEnumerable<ContentBatchOperationDescriber> items)
        {
            var descriptor = new BulkDescriptor();
            _ = descriptor.Refresh(Refresh.WaitFor);

            foreach (var item in items)
            {
                item.State = ContentDeletionStateEnum.SucceededIndexDeletion;
                var document = (item.Content as Document)!;
                foreach (var field in document.Fields)
                {
                    var indexId = Connection.GetIndexId(field.Key);
                    _ = descriptor.Delete<object>(op => op
                         .Index(indexId)
                         .Id(document.Id)
                     );
                }
            }

            var response = Connection.Client.Bulk(descriptor);
            if (response.Errors && response.ItemsWithErrors.Any())
            {
                foreach (var errorItem in response.ItemsWithErrors)
                {
                    var item = items.First(i => i.Content.Id.ToString(CultureInfo.InvariantCulture) == errorItem.Id && errorItem.Type == "_doc");
                    item.State = ContentDeletionStateEnum.ErrorIndexDeletion;
                    item.Error = errorItem.Error;
                }
            }
        }

        /// <summary>
        /// Indexes the specified field identifier.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="value">The value.</param>
        public override async Task<bool> IndexAsync(string fieldId, long documentId, object? value)
        {
            dynamic updateDoc = new System.Dynamic.ExpandoObject();
            updateDoc.value = value;
            var response = await Connection.Client.IndexAsync((object)updateDoc, i => i.Index(Connection.GetIndexId(fieldId)));
            if (response.ApiCall.Success && response.IsValid)
            {
                return true;
            }
            else
            {
                Trace.Fail($"Cannot index field {fieldId} for document: {documentId} because {response.DebugInformation}", response.OriginalException.ToString());
                return false;
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        public async Task<bool> DeleteIndexAsync(string name)
        {
            var response = await Connection.Client.Indices.DeleteAsync(name);
            if (response.ApiCall.Success && response.IsValid)
            {
                return true;
            }
            else
            {
                Trace.Fail($"Cannot delete index: {name} because {response.DebugInformation}", response.OriginalException.ToString());
                return false;
            }
        }

        /// <summary>
        /// Indexes the specified document ids.
        /// </summary>
        /// <param name="documentIds">The document ids.</param>
        /// <param name="fields">The fields.</param>
        public override void Index(IEnumerable<long> documentIds, IDictionary<string, object?> fields)
        {
            var descriptor = new BulkDescriptor();
            // https://www.elastic.co/guide/en/elasticsearch/reference/master/docs-refresh.html
            // Refresh.WaitFor: waits until the indexing completed and changes are visisble for search
            _ = descriptor.Refresh(Refresh.WaitFor);

            foreach (var field in fields)
            {
                var indexId = Connection.GetIndexId(field.Key);
                dynamic updateDoc = new System.Dynamic.ExpandoObject();
                updateDoc.value = field.Value;

                foreach (var documentId in documentIds)
                {
                    _ = descriptor.Index<object>(op => op
                        .Index(indexId)
                        .Id(documentId)
                        .Document(updateDoc)
                    );
                }
            }
            var result = Connection.Client.Bulk(descriptor);
        }


        /// <summary>
        /// Indexes the specified documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        public override void Index(IEnumerable<Document> documents)
        {
            var descriptor = new BulkDescriptor();
            _ = descriptor.Refresh(Refresh.WaitFor);

            var docId = 0;

            foreach (var document in documents)
            {
                foreach (var field in document.Fields)
                {
                    var indexId = Connection.GetIndexId(field.Key);
                    dynamic updateDoc = new System.Dynamic.ExpandoObject();
                    updateDoc.value = field.Value;

                    _ = descriptor.Index<object>(op => op
                        .Index(indexId)
                    // TODO: docIds should come from document object, this is only for testing purposes
                        .Id(document.Id > 0 ? document.Id : docId++)
                        .Document(updateDoc)
                    );
                }
            }
            var result = Connection.Client.Bulk(descriptor);
        }


        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="settings">The settings.</param>
        public override void Update(string id, object settings)
        {

        }
    }
}
