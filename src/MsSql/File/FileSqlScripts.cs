namespace POC.Storage.MsSql
{
    class FileSqlScripts
    {
        private const string TableName = "File";

        internal static readonly string CreateTable =
            $@"IF NOT EXISTS
               (  SELECT [name]
                  FROM sys.tables
                  WHERE [name] = '{TableName}'
               )
                   CREATE TABLE [{TableName}] (
                       [Id] BIGINT IDENTITY(1,1),
                       [DocumentId] BIGINT,
                       [Type] NVARCHAR(50),
                       [FileName] NVARCHAR(500),
                       [Index] INT,
                       [Size] BIGINT,
                       [ProviderType] NVARCHAR(250),
                       [Reference] NVARCHAR(MAX),
                       [IsFinalized] BIT DEFAULT 0,
                       [CreatedDate] DATETIMEOFFSET(7),
                       [ModifiedDate] DATETIMEOFFSET(7),
                       [PageId] NVARCHAR(250),
                       [DocumentIdentifier] NVARCHAR(250),
                       CONSTRAINT [PK_{TableName}] PRIMARY KEY ([Id])
               )";

        internal static readonly string Create =
            $@"INSERT INTO [{TableName}] (
                  [DocumentId],
                  [Type],
                  [FileName],
                  [Index],
                  [Size],
                  [ProviderType],
                  [Reference],
                  [CreatedDate],
                  [PageId],
                  [DocumentIdentifier]
               ) VALUES (
                  @DocumentId,
                  @Type,
                  @FileName,
                  @Index,
                  @Size,
                  @ProviderType,
                  @Reference,
                  @CreatedDate,
                  @PageId,
                  @DocumentIdentifier
               );
               SELECT SCOPE_IDENTITY();";

        internal static readonly string Update =
            $@"UPDATE [{TableName}]
               SET
                  [DocumentId] = @DocumentId,
                  [DocumentIdentifier] = @DocumentIdentifier,
                  [Type] = @Type,
                  [FileName] = @FileName,
                  [Index] = @Index,
                  [Size] = @Size,
                  [ModifiedDate] = @ModifiedDate,
                  [PageId] = @PageId
               WHERE Id = @Id";

        internal static readonly string Init =
            $@"UPDATE [{TableName}]
               SET 
                  [ProviderType] = @ProviderType,
                  [Reference] = @Reference,
                  [ModifiedDate] = @ModifiedDate
               WHERE Id = @Id";

        internal static readonly string Finalize =
            $@"UPDATE [{TableName}]
               SET 
                  [IsFinalized] = @IsFinalized,
                  [ModifiedDate] = @ModifiedDate
               WHERE Id = @Id";

        internal static readonly string FindById =
            $@"SELECT
                  [Id],
                  [DocumentId],
                  [Type],
                  [FileName],
                  [Index],
                  [Size],
                  [ProviderType],
                  [Reference],
                  [IsFinalized],
                  [CreatedDate],
                  [ModifiedDate],
                  [PageId],
                  [DocumentIdentifier]
               FROM [{TableName}]
               WHERE [Id] = @Id";

        internal static readonly string Find =
            $@"SELECT
                  [Id],
                  [DocumentId],
                  [Type],
                  [FileName],
                  [Index],
                  [Size],
                  [ProviderType],
                  [Reference],
                  [IsFinalized],
                  [CreatedDate],
                  [ModifiedDate],
                  [PageId],
                  [DocumentIdentifier]
               FROM [{TableName}]";

        internal static string DeleteFromTableById = $"DELETE FROM [{TableName}] WHERE [Id] = @Id;";
    }
}
