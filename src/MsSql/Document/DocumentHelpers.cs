using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace POC.Storage.MsSql
{
    internal static class DocumentHelpers
    {
        internal static List<Document> ReadDocuments(SqlDataReader reader, IEnumerable<Field> dynFields)
        {
            var documents = new List<Document>();
            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                var createdDate = reader.GetDateTimeOffset(1);
                var modifiedDate = reader.GetDateTimeOffset(2);

                var document = new Document(id, createdDate, modifiedDate)
                {
                    ParentId = reader.GetString(3)
                };

                int position = 4;
                foreach (var dynField in dynFields)
                {
                    var dynFieldName = dynField.Id;
                    var value = reader.GetValue(position++);
                    if (value.GetType().Name == "DBNull")
                    {
                        //TODO: handle null value
                    }
                    else if (dynField.Type == FieldType.Code)
                    {
                        value = JsonConvert.DeserializeObject<CodeFieldValue>((string)value);
                    }
                    document.Fields.Add(dynFieldName, value);
                }
                documents.Add(document);
            }
            return documents;
        }

        internal static Dictionary<string, int> ReadCountsByParents(SqlDataReader reader)
        {
            var countByFolders = new Dictionary<string, int>();
            while (reader.Read())
            {
                var parentId = reader.GetString(0);
                var count = reader.GetInt32(1);
                countByFolders.Add(parentId, count);
            }
            return countByFolders;
        }

        internal static Dictionary<OptionalValue<object?>, int> ReadFieldUsageStats(SqlDataReader reader, Field field)
        {
            var fieldUsageStats = new Dictionary<OptionalValue<object?>, int>();
            while (reader.Read())
            {
                var value = reader.GetValue(0);
                var count = reader.GetInt32(1);
                if (field.Type == FieldType.Code)
                {
                    value = JsonConvert.DeserializeObject<CodeFieldValue>((string)value);
                }
                fieldUsageStats.Add(value, count);
            }
            return fieldUsageStats;
        }

        internal static List<SqlParameter> CreateCommandParameters(Document document, Dictionary<string, Field> schema)
        {
            var parameters = new List<SqlParameter>();
            foreach (var dynProp in document.Fields)
            {
                var field = schema[dynProp.Key];
                var dbType = field.Type.GetSqlDbType();
                if (!dbType.HasValue)
                {
                    continue;
                }
                var value = dynProp.Value;
                switch (field.Type)
                {
                    case FieldType.Code:
                        value = JsonConvert.SerializeObject(value);
                        break;
                }
                parameters.Add(Connection.CreateCommandParameter($"@{dynProp.Key}", dbType.Value, value));
            }
            return parameters;
        }
    }
}
