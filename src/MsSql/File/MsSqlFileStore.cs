using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage.MsSql
{
    /// <summary>
    /// MsSql File Store Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.FileStoreBase" />
    class MsSqlFileStore : FileStoreBase
    {
        Connection Connection { get; set; }

        internal MsSqlFileStore(Connection connection)
        {
            Connection = connection;
            InitAsync(CancellationToken.None).Wait();
        }

        /// <summary>
        /// Initializes the MSSQL file store.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        internal Task<int> InitAsync(CancellationToken cancellationToken)
        {
            return Connection.ExecuteNonQueryAsync(FileSqlScripts.CreateTable, cancellationToken);
        }

        public override async Task<long> CreateAsync(File file, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@DocumentId", SqlDbType.BigInt, file.DocumentId),
                Connection.CreateCommandParameter("@Type", SqlDbType.NVarChar, file.Type.ToString()),
                Connection.CreateCommandParameter("@FileName", SqlDbType.NVarChar, file.FileName),
                Connection.CreateCommandParameter("@Index", SqlDbType.Int, file.Index),
                Connection.CreateCommandParameter("@Size", SqlDbType.BigInt, file.Size),
                Connection.CreateCommandParameter("@ProviderType", SqlDbType.NVarChar, file.ProviderType ?? SqlString.Null),
                Connection.CreateCommandParameter("@Reference", SqlDbType.NVarChar, file.Reference ?? SqlString.Null),
                Connection.CreateCommandParameter("@CreatedDate", SqlDbType.DateTimeOffset, DateTimeOffset.UtcNow),
                Connection.CreateCommandParameter("@PageId", SqlDbType.NVarChar, file.PageId ?? SqlString.Null),
                Connection.CreateCommandParameter("@DocumentIdentifier", SqlDbType.NVarChar, file.DocumentIdentifier ?? SqlString.Null)
            };
            var result = await Connection!.ExecuteScalarAsync(Connection.CreateCommand(FileSqlScripts.Create, parameters), cancellationToken);
            return decimal.ToInt64((decimal)result);
        }

        public override async Task UpdateAsync(File file, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.BigInt, file.Id),
                Connection.CreateCommandParameter("@DocumentId", SqlDbType.BigInt, file.DocumentId),
                Connection.CreateCommandParameter("@DocumentIdentifier", SqlDbType.NVarChar, file.DocumentIdentifier ?? SqlString.Null),
                Connection.CreateCommandParameter("@Type", SqlDbType.NVarChar, file.Type.ToString()),
                Connection.CreateCommandParameter("@FileName", SqlDbType.NVarChar, file.FileName),
                Connection.CreateCommandParameter("@Index", SqlDbType.Int, file.Index),
                Connection.CreateCommandParameter("@Size", SqlDbType.BigInt, file.Size),
                Connection.CreateCommandParameter("@ModifiedDate", SqlDbType.DateTimeOffset, DateTimeOffset.UtcNow),
                Connection.CreateCommandParameter("@PageId", SqlDbType.NVarChar, file.PageId ?? SqlString.Null)
            };
            _ = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FileSqlScripts.Update, parameters), cancellationToken);
        }

        public override async Task InitAsync(long id, string providerType, string reference, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.BigInt, id),
                Connection.CreateCommandParameter("@ProviderType", SqlDbType.NVarChar, providerType),
                Connection.CreateCommandParameter("@Reference", SqlDbType.NVarChar, reference),
                Connection.CreateCommandParameter("@ModifiedDate", SqlDbType.DateTimeOffset, DateTimeOffset.UtcNow),
            };
            var result = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FileSqlScripts.Init, parameters), cancellationToken);
            if (result != 1)
            {
                // TODO: create test
                throw new ApplicationException($"Init failed for Id: {id}");
            }
        }

        public override async Task FinalizeAsync(long id, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.BigInt, id),
                Connection.CreateCommandParameter("@IsFinalized", SqlDbType.Bit, true),
                Connection.CreateCommandParameter("@ModifiedDate", SqlDbType.DateTimeOffset, DateTimeOffset.UtcNow),
            };
            var result = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FileSqlScripts.Finalize, parameters), cancellationToken);
            if (result != 1)
            {
                // TODO: create test
                throw new ApplicationException($"Init failed for Id: {id}");
            }
        }

        public override async Task DeleteAsync(File file, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new NotImplementedException();
        }

        public override async Task<IList<ContentBatchOperationDescriber>> DeleteAllAsync(IList<File> files, CancellationToken cancellationToken)
        {
            var result = new List<ContentBatchOperationDescriber>();
            var currentFile = default(File)!;
            try
            {
                foreach (var file in files)
                {
                    currentFile = file;
                    var parameters = new[]
                    {
                        Connection.CreateCommandParameter("@Id", SqlDbType.BigInt, file.Id),
                    };
                    _ = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FileSqlScripts.DeleteFromTableById, parameters), cancellationToken);
                    result.Add(new ContentBatchOperationDescriber()
                    {
                        Content = file,
                        State = ContentDeletionStateEnum.SucceededFileDeletion
                    });
                }
            }
            catch (Exception exception) when (exception is SystemException)
            {
                result.Add(new ContentBatchOperationDescriber()
                {
                    Content = currentFile,
                    State = ContentDeletionStateEnum.ErrorFileDeletion,
                    Error = exception
                });
            }

            return result;
        }

        public override async Task<File> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.BigInt, id),
            };
            return (await Connection!.ExecuteQueryAsync(Connection.CreateCommand(FileSqlScripts.FindById, parameters), FileHelpers.ReadFiles, cancellationToken)).SingleOrDefault();
        }

        public override async Task<IList<File>> FindAsync(FileQueryOptions options, CancellationToken cancellationToken)
        {
            var whereClauses = new List<string>();
            var cmdParameters = new List<SqlParameter>();

            if (options.Ids.Any())
            {
                var idsString = string.Join(',', options.Ids);
                whereClauses.Add(" [Id] IN (SELECT convert(int, value) FROM string_split(@ids, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@ids", SqlDbType.NVarChar, idsString));
            }
            if (options.DocumentIds.Any())
            {
                var documentIdsString = string.Join(',', options.DocumentIds);
                whereClauses.Add(" [DocumentId] IN (SELECT value FROM string_split(@documentIds, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@documentIds", SqlDbType.NVarChar, documentIdsString));
            }
            if (options.Type != null)
            {
                whereClauses.Add(" [Type] = @type ");
                cmdParameters.Add(Connection.CreateCommandParameter("@type", SqlDbType.NVarChar, options.Type));
            }
            if (options.Indexes.Any())
            {
                var indexesString = string.Join(',', options.Indexes);
                whereClauses.Add(" [Index] IN (SELECT value FROM string_split(@indexes, ',')) ");
                cmdParameters.Add(Connection.CreateCommandParameter("@indexes", SqlDbType.NVarChar, indexesString));
            }
            var skipClause = options.Offset > 0 || options.Limit > 0 ? $" OFFSET {options.Offset} ROWS " : string.Empty;
            var takeClause = options.Limit > 0 ? $" FETCH NEXT {options.Limit} ROWS ONLY " : string.Empty;
            var orderByClause = skipClause.Length > 0 || takeClause.Length > 0 ? " ORDER BY Id ASC" : string.Empty;

            var findQuery = $"{FileSqlScripts.Find} { (whereClauses.Any() ? $" WHERE {string.Join(" AND ", whereClauses)}" : string.Empty)} {orderByClause} {skipClause} {takeClause}";
            return await Connection!.ExecuteQueryAsync(Connection.CreateCommand(findQuery, cmdParameters), FileHelpers.ReadFiles, cancellationToken);
        }
    }
}
