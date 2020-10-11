using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace POC.Storage.MsSql
{
    internal static class FileHelpers
    {
        static readonly Type s_fileTypeType = typeof(FileType);

        internal static IList<File> ReadFiles(SqlDataReader reader)
        {
            var schema = new List<File>();
            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                var providerType = reader.GetString(6);
                var reference = reader.GetString(7);
                var isFinalized = reader.GetBoolean(8);
                var createdDate = reader.GetDateTimeOffset(9);
                var modifiedDate = reader.GetDateTimeOffset(10);
                var documentIdentifier = reader.GetString(12);
                schema.Add(new File(id, documentIdentifier, providerType, reference, isFinalized, createdDate, modifiedDate)
                {
                    DocumentId = reader.GetInt64(1),
                    Type = (FileType)Enum.Parse(s_fileTypeType, reader.GetString(2)),
                    FileName = reader.GetString(3),
                    Index = reader.GetInt32(4),
                    Size = reader.GetInt64(5),
                    PageId = reader.GetString(11)
                });
            }
            return schema;
        }
    }
}
