using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage.MsSql
{
    class Connection
    {
        internal const string MetadataConnectionStringKey = "POC.Storage.DB";

        string MasterConnectionString { get; }
        internal string ConnectionString { get; }
        internal string StorageId { get; }
        internal Connection(string storageId, string masterConnectionString)
        {
            StorageId = storageId;
            MasterConnectionString = masterConnectionString;
            ConnectionString = GetConnectionString();
            EnsureDatabaseCreatedAsync(CancellationToken.None).Wait();
        }

        string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(MasterConnectionString)
            {
                InitialCatalog = StorageId
            };
            return builder.ConnectionString;
        }

        SqlConnection GetMasterConnection()
        {
            return new SqlConnection(MasterConnectionString);
        }

        SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        async Task OpenConnectionAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
            }
        }

        async Task CloseConnectionAsync(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }

        internal async Task<int> ExecuteNonQueryAsync(string cmdText, CancellationToken cancellationToken)
        {
            return await ExecuteNonQueryAsync(CreateCommand(cmdText), cancellationToken);
        }

        internal async Task<int> ExecuteNonQueryAsync(SqlCommand cmd, CancellationToken cancellationToken)
        {
            using var connection = GetConnection();
            await OpenConnectionAsync(connection, cancellationToken);
            var result = await ExecuteNonQueryAsync(cmd, connection, cancellationToken);
            await CloseConnectionAsync(connection);
            return result;
        }

        internal async Task<int> ExecuteNonQueryAsync(SqlCommand cmd, SqlConnection connection, CancellationToken cancellationToken, SqlTransaction? transaction = null)
        {
            using var command = cmd;
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            var result = await command.ExecuteNonQueryAsync(cancellationToken);
            return result;
        }

        internal async Task<object> ExecuteScalarAsync(SqlCommand cmd, CancellationToken cancellationToken)
        {
            using var connection = GetConnection();
            await OpenConnectionAsync(connection, cancellationToken);
            var result = await ExecuteScalarAsync(cmd, connection, cancellationToken);
            await CloseConnectionAsync(connection);
            return result;
        }

        internal async Task<object> ExecuteScalarAsync(SqlCommand cmd, SqlConnection connection, CancellationToken cancellationToken, SqlTransaction? transaction = null)
        {
            using var command = cmd;
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            var result = await command.ExecuteScalarAsync(cancellationToken);
            if (transaction != null)
            {
                connection.Close();
            }
            return result;
        }

        internal async Task<T> ExecuteQueryAsync<T>(SqlCommand cmd, Func<SqlDataReader, T> readResultFunc, CancellationToken cancellationToken)
        {
            T result;
            using (var connection = GetConnection())
            {
                await OpenConnectionAsync(connection, cancellationToken);
                result = await ExecuteQueryAsync<T>(cmd, readResultFunc, connection, null);
                await CloseConnectionAsync(connection);
            }
            return result;
        }

        internal async Task<T> ExecuteQueryAsync<T>(SqlCommand cmd, Func<SqlDataReader, T> readResultFunc, SqlConnection connection, SqlTransaction? transaction = null)
        {
            T result;
            using (var command = cmd)
            {
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                using var reader = await command.ExecuteReaderAsync();
                result = readResultFunc(reader);
            }
            return result;
        }

        public async Task InTransactionAsync(Func<SqlTransaction, Task> executeActions, CancellationToken cancellationToken)
        {
            using var connection = GetConnection();
            SqlTransaction? transaction = null;
            try
            {
                await OpenConnectionAsync(connection, cancellationToken);
                using (transaction = connection.BeginTransaction())
                {
                    await executeActions(transaction);
                    await transaction.CommitAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                // Attempt to roll back the transaction.

                // TODO: log

                try
                {
                    transaction?.RollbackAsync(cancellationToken);
                }
                catch (InvalidOperationException)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.

                    // TODO: log
                    // TODO: escalate, notify?
                }

                throw;
            }
        }

        internal static SqlParameter CreateCommandParameter(string name, SqlDbType type, object? value)
        {
            return new SqlParameter(name, type)
            {
                Value = value ?? DBNull.Value
            };
        }

        internal static SqlCommand CreateCommand(string cmdText, IEnumerable<SqlParameter>? parameters = null)
        {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            var cmd = new SqlCommand(cmdText);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    _ = cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        async Task<bool> DatabaseExists(CancellationToken cancellationToken)
        {
            var cmdText = $@"SELECT COUNT(1) FROM sys.databases WHERE name = N'{StorageId}'";
            using var connection = GetMasterConnection();
            await connection.OpenAsync(cancellationToken);
            var result = (int)(await ExecuteScalarAsync(CreateCommand(cmdText), connection, cancellationToken)) == 1;
            await connection.CloseAsync();
            return result;
        }

        internal async Task<int> EnsureDatabaseCreatedAsync(CancellationToken cancellationToken)
        {
            if (!await DatabaseExists(cancellationToken))
            {
                var cmdText = GetCreateDatabaseSqlCommand(StorageId);
                using var connection = GetMasterConnection();
                await connection.OpenAsync(cancellationToken);
                var result = await ExecuteNonQueryAsync(CreateCommand(cmdText), connection, cancellationToken);
                await connection.CloseAsync();

                if (!await DatabaseExists(cancellationToken))
                {
                    throw new ApplicationException($"Unable to create database for storage: {StorageId}");
                }

                return result;
            }
            return 0;
        }

        static string GetCreateDatabaseSqlCommand(string storageId)
        {
            return string.Format(CultureInfo.InvariantCulture,
                @"IF NOT EXISTS
                  (
                      SELECT [name]
                      FROM sys.databases
                      WHERE [name] = N'{0}'
                  )
                  CREATE DATABASE [{0}];", storageId);
        }
    }
}
