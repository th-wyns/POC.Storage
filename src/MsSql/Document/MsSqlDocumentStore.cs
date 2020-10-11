using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Newtonsoft.Json;

namespace POC.Storage.MsSql
{
    /// <summary>
    /// MsSql implementation for Document object store.
    /// </summary>
    /// <seealso cref="POC.Storage.IDocumentStore" />
    class MsSqlDocumentStore : DocumentStoreBase
    {
        Connection Connection { get; set; }

        MsSqlFieldStore FieldStore { get; }


        internal MsSqlDocumentStore(Connection connection, MsSqlFieldStore fieldStore)
        {
            Connection = connection;
            FieldStore = fieldStore;
            InitAsync(CancellationToken.None).Wait();
        }

        internal Task<int> InitAsync(CancellationToken cancellationToken)
        {
            return Connection.ExecuteNonQueryAsync(DocumentSqlScripts.CreateDocumentTableSqlCommand, cancellationToken);
        }

        public override async Task<long> CreateAsync(Document document, CancellationToken cancellationToken)
        {
            // TODO: cache
            var schema = (await FieldStore.FindAllAsync(cancellationToken)).ToDictionary(field => field.Id);
            var dynFields = document.Fields.Where(prop => schema[prop.Key].Type.GetSqlDbType() != null).Select(prop => $"{prop.Key}").ToList();
            var dynColNames = string.Join("", dynFields.Select(dynField => $",[{dynField}]"));
            var dynParamNames = string.Join("", dynFields.Select(dynField => $",@{dynField}"));

            var parameters = new List<SqlParameter>
            {
                Connection.CreateCommandParameter("@CreatedDate", SqlDbType.DateTimeOffset, DateTime.UtcNow),
                Connection.CreateCommandParameter("@ModifiedDate", SqlDbType.DateTimeOffset, DateTime.UtcNow),
                Connection.CreateCommandParameter("@ParentId", SqlDbType.NVarChar, document.ParentId ?? string.Empty),
            };
            parameters.AddRange(DocumentHelpers.CreateCommandParameters(document, schema));

            var cmdText = $@"INSERT INTO [Document] (
                                        [CreatedDate],
                                        [ModifiedDate],
                                        [ParentId]
                                        {dynColNames}
                                     ) VALUES (
                                        @CreatedDate,
                                        @ModifiedDate,
                                        @ParentId
                                        {dynParamNames}
                                     );
                             SELECT SCOPE_IDENTITY();";

            var result = await Connection!.ExecuteScalarAsync(Connection.CreateCommand(cmdText, parameters), cancellationToken);
            var id = decimal.ToInt64((decimal)result);
            return id;
        }

        public override async Task<long> CreateAsync(IEnumerable<Document> documents, CancellationToken cancellationToken)
        {
            // TODO: multi row insert
            // http://www.sqlservertutorial.net/sql-server-basics/sql-server-insert-multiple-rows/

            await Task.Yield();
            throw new NotImplementedException();
        }

        // TODO: add parameter to define view (e.g.: Id, OwnerId, DynProp1, DynProp4)
        public override async Task<Document> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            var options = new DocumentQueryOptions();
            options.Ids.Add(id);
            return (await FindAllAsync(options, cancellationToken)).SingleOrDefault();
        }

        // TODO: add parameter to define view (e.g.: Id, OwnerId, DynProp1, DynProp4)
        public override async Task<IList<Document>> FindAllAsync(DocumentQueryOptions options, CancellationToken cancellationToken)
        {
            // TODO: cache
            var schema = (await FieldStore.FindAllAsync(cancellationToken)).Where(field => !options.FieldNames.Any() || options.FieldNames.Contains(field.Id)).ToDictionary(field => field.Id);
            var dynColNames = string.Join("", schema.Keys.Select(dynField => $",[{dynField}]"));
            var dynParamNames = string.Join("", schema.Keys.Select(dynField => $",@{dynField}"));

            var whereClauses = new List<string>();
            var cmdParameters = new List<SqlParameter>();
            if (options.Ids.Any())
            {
                var idsString = string.Join(',', options.Ids);
                whereClauses.Add(" Id IN (SELECT convert(int, value) FROM string_split(@ids, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@ids", SqlDbType.NVarChar, idsString));
            }
            if (options.ParentIds.Any())
            {
                var parentIdsString = string.Join(',', options.ParentIds);
                whereClauses.Add(" ParentId IN (SELECT value FROM string_split(@parentIds, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@parentIds", SqlDbType.NVarChar, parentIdsString));
            }

            var orderClause = $" ORDER BY {(string.IsNullOrEmpty(options.OrderBy) ? "Id" : options.OrderBy)} {(options.OrderAsc ? string.Empty : "DESC")}";
            var skipClause = options.Offset > 0 || options.Limit > 0 ? $" OFFSET {options.Offset} ROWS " : string.Empty;
            var takeClause = options.Limit > 0 ? $" FETCH NEXT {options.Limit} ROWS ONLY " : string.Empty;

            var cmdText = $@"SELECT [Id],
                                    [CreatedDate],
                                    [ModifiedDate],
                                    [ParentId]
                                    {dynColNames}
                             FROM [Document]
                             {(whereClauses.Any() ? $" WHERE {string.Join(" AND ", whereClauses)}" : string.Empty)}
                             {orderClause} {skipClause} {takeClause}";

            var documents = await Connection!.ExecuteQueryAsync(Connection.CreateCommand(cmdText, cmdParameters), (sqlReader) => DocumentHelpers.ReadDocuments(sqlReader, schema.Values), cancellationToken);
            return documents;
        }

        /// <summary>
        /// Gets the document ids with the specified options.
        /// </summary>
        /// <param name="options">The query options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<Dictionary<long, string>> GetDocumentIdsByIdAsync(DocumentQueryOptions options, CancellationToken cancellationToken)
        {
            // TODO: cache
            var documentIdField = (await FieldStore.FindAllAsync(cancellationToken)).First(field => field.Name == Field.DocumentIdFieldName);

            var whereClauses = new List<string>();
            var cmdParameters = new List<SqlParameter>();
            if (options.Ids.Any())
            {
                var idsString = string.Join(',', options.Ids);
                whereClauses.Add(" Id IN (SELECT convert(int, value) FROM string_split(@ids, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@ids", SqlDbType.NVarChar, idsString));
            }
            if (options.ParentIds.Any())
            {
                var parentIdsString = string.Join(',', options.ParentIds);
                whereClauses.Add(" ParentId IN (SELECT value FROM string_split(@parentIds, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@parentIds", SqlDbType.NVarChar, parentIdsString));
            }

            var orderClause = $" ORDER BY {(string.IsNullOrEmpty(options.OrderBy) ? "Id" : options.OrderBy)} {(options.OrderAsc ? string.Empty : "DESC")}";
            var skipClause = options.Offset > 0 || options.Limit > 0 ? $" OFFSET {options.Offset} ROWS " : string.Empty;
            var takeClause = options.Limit > 0 ? $" FETCH NEXT {options.Limit} ROWS ONLY " : string.Empty;

            var cmdText = $@"SELECT [Id], [{documentIdField.Id}]
                             FROM [Document]
                             {(whereClauses.Any() ? $" WHERE {string.Join(" AND ", whereClauses)}" : string.Empty)}
                             {orderClause} {skipClause} {takeClause}";

            var documentIdsById = await Connection!.ExecuteQueryAsync(Connection.CreateCommand(cmdText, cmdParameters), (sqlReader) =>
            {
                var documentIdsById = new Dictionary<long, string>();
                while (sqlReader.Read())
                {
                    documentIdsById.Add(sqlReader.GetInt64(0), sqlReader.GetString(1));
                }
                return documentIdsById;
            }, cancellationToken);
            return documentIdsById;
        }

        public override async Task<int> CountAsync(IEnumerable<string?> parentIds, CancellationToken cancellationToken)
        {
            var whereClauses = new List<string>();
            var cmdParameters = new List<SqlParameter>();
            if (parentIds.Any())
            {
                var parentIdsString = string.Join(',', parentIds);
                whereClauses.Add(" ParentId IN (SELECT value FROM string_split(@parentIds, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@parentIds", SqlDbType.NVarChar, parentIdsString));
            }

            var cmdText = $@"SELECT COUNT(1)
                             FROM [Document]
                             {(whereClauses.Any() ? $" WHERE {string.Join(" AND ", whereClauses)}" : string.Empty)}";

            var count = await Connection!.ExecuteScalarAsync(Connection.CreateCommand(cmdText, cmdParameters), cancellationToken);
            return (int)count;
        }

        public override async Task<Dictionary<string, int>> CountsByParentsAsync(CancellationToken cancellationToken)
        {
            var cmdText = $@"SELECT ParentId, COUNT(1)
                             FROM Document
                             GROUP BY ParentId";

            var countByFolders = await Connection!.ExecuteQueryAsync(Connection.CreateCommand(cmdText), (sqlReader) => DocumentHelpers.ReadCountsByParents(sqlReader), cancellationToken);
            return countByFolders;
        }

        public override Task UpdateAsync(Document document, CancellationToken cancellationToken)
        {
            var fields = document.Fields ?? new Dictionary<string, object?>();
            if (!fields.ContainsKey("ParentId"))
            {
                fields.Add("ParentId", document.ParentId);
            }
            return SetFieldsAsync(document.Id, fields, cancellationToken);
        }

        public override Task SetFieldsAsync(long id, IDictionary<string, object?> fields, CancellationToken cancellationToken)
        {
            return SetFieldsAsync(new List<long> { id }, fields, cancellationToken);
        }

        public override async Task SetFieldsAsync(IEnumerable<long> ids, IDictionary<string, object?> fields, CancellationToken cancellationToken)
        {
            var schema = (await FieldStore.FindAllAsync(cancellationToken)).ToDictionary(field => field.Id);
            var cmdParameters = new List<SqlParameter>();
            var setStr = new StringBuilder();
            var count = 0;
            foreach (var field in fields)
            {
                var sqlFieldType = schema[field.Key].Type.GetSqlDbType();
                if (sqlFieldType == null)
                {
                    continue;
                }
                if (count > 0)
                {
                    _ = setStr.Append(",");
                }
                _ = setStr.Append($"{field.Key}=@P{count}");

                object? fieldValue;
                switch (schema[field.Key].Type)
                {
                    case FieldType.Code:
                        fieldValue = JsonConvert.SerializeObject(field.Value);
                        break;
                    default:
                        fieldValue = field.Value;
                        break;
                }

                cmdParameters.Add(Connection.CreateCommandParameter($"@P{count++}", sqlFieldType.Value, fieldValue));
            }

            var fieldSettings = setStr.ToString();
            //var fieldSettings = string.Join(",", fields.Select(field => $"{field.Key} = {field.Value}"));
            var idsStr = string.Join(',', ids);
            var cmdText = $@"UPDATE Document
                             SET {fieldSettings}
                             WHERE Id IN (SELECT convert(int, value) FROM string_split(@ids, ','))";
            //WHERE Id in ({idsStr})";

            cmdParameters.Add(Connection.CreateCommandParameter("@ids", SqlDbType.NVarChar, idsStr));
            await Connection!.InTransactionAsync(async (transaction) =>
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                await Connection.ExecuteNonQueryAsync(Connection.CreateCommand(cmdText, cmdParameters), transaction.Connection, cancellationToken, transaction);
                sw.Stop();
                System.Diagnostics.Debug.WriteLine($"MU2:{sw.Elapsed.TotalSeconds}");
                // TODO: audit
            }, cancellationToken);
        }

        public override async Task SetParentIdAsync(IEnumerable<long> ids, string parentId, CancellationToken cancellationToken)
        {
            var cmdText = $@"UPDATE Document
                             SET ParentId = @ParentId
                             WHERE Id IN (SELECT convert(int, value) FROM string_split(@ids, ','))";
            var cmdParameters = new List<SqlParameter>
            {
                Connection.CreateCommandParameter("@ids", SqlDbType.NVarChar, string.Join(',', ids)),
                Connection.CreateCommandParameter("@ParentId", SqlDbType.NVarChar, parentId)
            };
            await Connection!.InTransactionAsync(async (transaction) =>
            {
                await Connection.ExecuteNonQueryAsync(Connection.CreateCommand(cmdText, cmdParameters), transaction.Connection, cancellationToken, transaction);
                // TODO: audit
            }, cancellationToken);
        }

        public override async Task DeleteAsync(Document id, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new NotImplementedException();
        }

        public override async Task<bool> IsFieldUsedAsync(Field field, CancellationToken cancellationToken)
        {
            var cmdText = $@"SELECT IIF (EXISTS (SELECT 1 FROM [Document] WHERE [{field.Id}] IS NOT NULL), CAST(1 AS BIT), CAST(0 AS BIT))";
            var isFieldUsed = (bool)(await Connection!.ExecuteScalarAsync(Connection.CreateCommand(cmdText), cancellationToken));
            return isFieldUsed;
        }

        public override async Task<IDictionary<OptionalValue<object?>, int>> FieldUsageStatsAsync(Field field, CancellationToken cancellationToken)
        {
            var cmdText = $@"SELECT DISTINCT [{field.Id}], COUNT(1)
                             FROM [Document]
                             GROUP BY [{field.Id}]";
            var fieldUsageStats = await Connection!.ExecuteQueryAsync(Connection.CreateCommand(cmdText), (sqlReader) => DocumentHelpers.ReadFieldUsageStats(sqlReader, field), cancellationToken);
            return fieldUsageStats;
        }

        public void RebuildIndex()
        {
            // time-to-time rebuild index to speed up queries
            // after column instertion? does it fragment the index?
            // ALTER INDEX ALL ON Documents REORGANIZE;
        }
    }
}
