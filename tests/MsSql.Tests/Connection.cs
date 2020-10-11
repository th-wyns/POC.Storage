using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Xunit;

namespace POC.Storage.MsSql.Tests
{
    class Connection
    {
        internal const string ProjectId = "POC.Storage.Tests";
        IConfigurationRoot Configuration { get; }
        internal string ConnectionString { get; }
        string MasterConnectionString { get; }
        internal static Connection Instance = new Connection();

        Connection()
        {
            Configuration = GetConfiguration();
            ConnectionString = GetConnectionString();
            MasterConnectionString = GetMasterConnectionString();
            RecreateDatabase();
        }

        IConfigurationRoot GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return configurationBuilder.Build();
        }

        string GetMasterConnectionString()
        {
            return Configuration.GetConnectionString("POC.Storage.DB");
        }

        string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(GetMasterConnectionString());
            builder.InitialCatalog = ProjectId;
            return builder.ConnectionString;
        }

        bool DatabaseExists()
        {
            var cmdText = $@"SELECT COUNT(1) FROM sys.databases WHERE name = N'{ProjectId}'";
            return (int)ExecuteScalar(cmdText, GetMasterConnection()) == 1;
        }

        internal bool DropDatabase()
        {
            var cmdText = $@"IF EXISTS
                                (  SELECT [name]
                                    FROM sys.databases
                                    WHERE [name] = N'{ProjectId}'
                                )
                                BEGIN
                                    ALTER DATABASE [{ProjectId}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                                    DROP DATABASE [{ProjectId}]
                                END";
            ExecuteNonQuery(cmdText, GetMasterConnection());
            return !DatabaseExists();
        }

        void EnsureDatabaseCreated()
        {
            if (!DatabaseExists())
            {
                var cmdText = $@"CREATE DATABASE [{ProjectId}]";
                ExecuteNonQuery(cmdText, GetMasterConnection());
            }
            Assert.True(DatabaseExists());
        }

        internal void RecreateDatabase()
        {
            DropDatabase();
            EnsureDatabaseCreated();
        }

        internal void TruncateDocumentsTable()
        {
            var cmdText = $@"IF OBJECT_ID('dbo.Document', 'U') IS NOT NULL TRUNCATE TABLE dbo.Document;";
            ExecuteNonQuery(cmdText, GetConnection());
        }

        SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        SqlConnection GetMasterConnection()
        {
            return new SqlConnection(MasterConnectionString);
        }

        internal SqlCommand CreateCommand(string cmdText, SqlConnection connection)
        {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            return new SqlCommand(cmdText, connection);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
        }

        internal int ExecuteNonQuery(string cmdText, SqlConnection? sqlConnection = null)
        {
            using (var connection = sqlConnection ?? GetConnection())
            using (var command = CreateCommand(cmdText, connection))
            {
                connection.Open();
                var result = command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
        }

        internal object ExecuteScalar(string cmdText, SqlConnection? sqlConnection = null)
        {
            using (var connection = sqlConnection ?? GetConnection())
            using (var command = CreateCommand(cmdText, connection))
            {
                connection.Open();
                var result = command.ExecuteScalar();
                connection.Close();
                return result;
            }
        }

        internal bool TableExists(string tableName)
        {
            var cmdText = $@"SELECT COUNT(1)
                               FROM sys.tables
                               WHERE [name] = N'{tableName}'";
            return (int)ExecuteScalar(cmdText) == 1;
        }

        internal bool DropTable(string tableName)
        {
            var cmdText = $@"IF EXISTS
                                (  SELECT [name]
                                   FROM sys.tables
                                   WHERE [name] = N'{tableName}'
                                )
                                    DROP TABLE [{tableName}]";
            ExecuteNonQuery(cmdText);
            return !TableExists(tableName);
        }
    }
}
