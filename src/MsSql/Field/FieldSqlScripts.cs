using System.Globalization;

namespace POC.Storage.MsSql
{
    class FieldSqlScripts
    {
        internal const string TableName = "Field";

        internal static readonly string InsertIntoFieldTableSqlCommand = string.Format(CultureInfo.InvariantCulture, @"INSERT INTO [{0}]
            ([Id], [Name], [Description], [Type], [IsBuiltIn], [IsRelational], [IsIncludeInTextSearch], [IsRequiredOnCodeSets], [IsComputed], [CodeConfiguration], [CreatedDate], [ModifiedDate]) VALUES
            (@Id, @Name, @Description, @Type, @IsBuiltIn, @IsRelational, @IsIncludeInTextSearch, @IsRequiredOnCodeSets, @IsComputed, @CodeConfiguration, @CreatedDate, @ModifiedDate);", TableName);

        internal static readonly string UpdateFieldTableSqlCommand = string.Format(CultureInfo.InvariantCulture, @"
            UPDATE {0}
            SET [Name] = @Name,
                [Description] = @Description,
                [IsRelational] = @IsRelational,
                [IsIncludeInTextSearch] =  @IsIncludeInTextSearch,
                [IsRequiredOnCodeSets] =  @IsRequiredOnCodeSets,
                [CodeConfiguration] =  @CodeConfiguration
            WHERE Id = @Id;", TableName);

        internal static readonly string DeleteFieldTableSqlCommand = string.Format(CultureInfo.InvariantCulture, @"
            DELETE FROM {0}
            WHERE Id = @Id;", TableName);

        internal const string CreateDocumentIndexSqlCommand = "CREATE INDEX [IX_{0}] ON Document([{1}]);";

        internal const string AlterDocumentTableSqlCommand = "ALTER TABLE Document ADD [{0}] {1}; {2}";

        internal static readonly string FindFieldByIdSqlCommand = string.Format(CultureInfo.InvariantCulture, @"SELECT
            [Id], [Name], [Description], [Type], [IsBuiltIn], [IsRelational], [IsIncludeInTextSearch], [IsRequiredOnCodeSets], [IsComputed], [CodeConfiguration], [CreatedDate], [ModifiedDate]
            FROM [{0}]
            WHERE [Id] = @Id", TableName);

        internal static readonly string FindFieldByNameSqlCommand = string.Format(CultureInfo.InvariantCulture, @"SELECT
            [Id], [Name], [Description], [Type], [IsBuiltIn], [IsRelational], [IsIncludeInTextSearch], [IsRequiredOnCodeSets], [IsComputed], [CodeConfiguration], [CreatedDate], [ModifiedDate]
            FROM [{0}]
            WHERE [Name] = @Name", TableName);

        internal static readonly string FindAllFieldSqlCommand = string.Format(CultureInfo.InvariantCulture, @"SELECT
            [Id], [Name], [Description], [Type], [IsBuiltIn], [IsRelational], [IsIncludeInTextSearch], [IsRequiredOnCodeSets], [IsComputed], [CodeConfiguration], [CreatedDate], [ModifiedDate]
            FROM [{0}];", TableName);

        internal static readonly string CreateFieldTableSqlCommand = string.Format(CultureInfo.InvariantCulture, @"IF NOT EXISTS
            (  SELECT [name] FROM sys.tables WHERE [name] = '{0}' )
            CREATE TABLE [{0}] (
                [Id] NVARCHAR(100) NOT NULL,
                [Name] NVARCHAR(100) NOT NULL,
                [Description] NVARCHAR(250),
                [Type] NVARCHAR(50),
                [IsBuiltIn] BIT,
                [IsRelational] BIT,
                [IsIncludeInTextSearch] BIT,
                [IsRequiredOnCodeSets] BIT,
                [IsComputed] BIT,
                [CodeConfiguration] NVARCHAR(MAX),
                [CreatedDate] DATETIMEOFFSET(7),
                [ModifiedDate] DATETIMEOFFSET(7),
                CONSTRAINT [PK_{0}] PRIMARY KEY ([Id]))", TableName);

        internal const string AlterTableRemoveIndex = "DROP INDEX [IX_{0}] ON Document;";
        internal const string AlterTableRemoveColumn = "{1}; ALTER TABLE Document DROP COLUMN [{0}];";
    }
}
