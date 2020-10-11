using System.Globalization;

namespace POC.Storage.MsSql
{
    class DocumentSqlScripts
    {
        private const string TableName = "Document";

        internal static readonly string CreateDocumentTableSqlCommand = string.Format(CultureInfo.InvariantCulture, @"IF NOT EXISTS
        (  SELECT [name]
            FROM sys.tables
            WHERE [name] = '{0}'
        )
            CREATE TABLE [{0}] (
                [Id] BIGINT IDENTITY(1,1),
                [CreatedDate] DATETIMEOFFSET(7),
                [ModifiedDate] DATETIMEOFFSET(7),
                [ParentId] NVARCHAR(512) NOT NULL DEFAULT ''
                CONSTRAINT [PK_{0}] PRIMARY KEY ([Id])
        );", TableName);

        internal const string DeleteFromTableById = "DELETE FROM Document WHERE Id={0};";
    }
}
