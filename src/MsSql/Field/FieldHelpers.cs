using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;

namespace POC.Storage.MsSql
{
    internal static class FieldHelpers
    {
        static Type s_fieldTypeType = typeof(FieldType);

        internal static string GetSqlColumnType(this FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.LongText:
                    return "NVARCHAR(MAX)";
                case FieldType.FixedLengthText:
                    return $"NVARCHAR({Field.FixedLengthTextMaxLength})";
                case FieldType.Date:
                    return "DATETIME2";
                case FieldType.Number:
                    return "DECIMAL(38,6)"; // WHAT RANGE WITH WHAT PRECISION?
                case FieldType.Code:
                    return "NVARCHAR(MAX)"; // RESTRICTION?
                //case FieldType.BlobReference:
                //    return "NVARCHAR(MAX)";
                //case FieldType.TextIndex:
                //    return null;
                default:
                    throw new ArgumentException(fieldType.ToString(), nameof(fieldType));
            }
        }

        internal static SqlDbType? GetSqlDbType(this FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.LongText:
                    return SqlDbType.NVarChar;
                case FieldType.FixedLengthText:
                    return SqlDbType.NVarChar;
                case FieldType.Date:
                    return SqlDbType.DateTime2;
                case FieldType.Number:
                    return SqlDbType.Decimal;
                case FieldType.Code:
                    return SqlDbType.NVarChar;
                //case FieldType.BlobReference:
                //    return SqlDbType.NVarChar;
                //case FieldType.TextIndex:
                //    return null;
                default:
                    throw new ArgumentException(fieldType.ToString(), nameof(fieldType));
            }
        }

        internal static IList<Field> ReadFields(SqlDataReader reader)
        {
            var schema = new List<Field>();
            while (reader.Read())
            {
                var id = reader.GetString(0);
                var createdDate = reader.GetDateTimeOffset(10);
                var modifiedDate = reader.GetDateTimeOffset(11);

                schema.Add(new Field(id, createdDate, modifiedDate)
                {
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Type = (FieldType)Enum.Parse(s_fieldTypeType, reader.GetString(3)),
                    IsBuiltIn = reader.GetBoolean(4),
                    IsRelational = reader.GetBoolean(5),
                    IsIncludeInTextSearch = reader.GetBoolean(6),
                    IsRequiredOnCodeSets = reader.GetBoolean(7),
                    IsComputed = reader.GetBoolean(8),
                    CodeConfiguration = JsonConvert.DeserializeObject<CodeConfiguration>(reader.GetString(9)),
                });
            }
            return schema;
        }
    }
}
