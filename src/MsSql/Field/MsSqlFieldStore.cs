using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace POC.Storage.MsSql
{
    /// <summary>
    /// MsSql implementation for Field object store.
    /// </summary>
    /// <seealso cref="POC.Storage.FieldStoreBase" />
    class MsSqlFieldStore : FieldStoreBase
    {
        Connection Connection { get; set; }

        internal MsSqlFieldStore(Connection connection)
        {
            Connection = connection;
            InitAsync(CancellationToken.None).Wait();
        }

        // TODO: create dictionary with column names, sql types
        internal Task<int> InitAsync(CancellationToken cancellationToken)
        {
            return Connection.ExecuteNonQueryAsync(FieldSqlScripts.CreateFieldTableSqlCommand, cancellationToken);
        }

        public override async Task CreateAsync(Field field, CancellationToken cancellationToken)
        {
            var id = field.Id; //GenerateId(field.Id);

            // Add entry to Field table

            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.NVarChar, id),
                Connection.CreateCommandParameter("@Name", SqlDbType.NVarChar, field.Name ?? string.Empty),
                Connection.CreateCommandParameter("@Description", SqlDbType.NVarChar, field.Description ?? string.Empty),
                Connection.CreateCommandParameter("@Type", SqlDbType.NVarChar, field.Type.ToString()),
                Connection.CreateCommandParameter("@IsBuiltIn", SqlDbType.Bit, field.IsBuiltIn),
                Connection.CreateCommandParameter("@IsRelational", SqlDbType.Bit, field.IsRelational),
                Connection.CreateCommandParameter("@IsIncludeInTextSearch", SqlDbType.Bit, field.IsIncludeInTextSearch),
                Connection.CreateCommandParameter("@IsRequiredOnCodeSets", SqlDbType.Bit, field.IsRequiredOnCodeSets),
                Connection.CreateCommandParameter("@IsComputed", SqlDbType.Bit, field.IsComputed),
                Connection.CreateCommandParameter("@CodeConfiguration", SqlDbType.NVarChar, JsonConvert.SerializeObject(field.CodeConfiguration)),
                Connection.CreateCommandParameter("@CreatedDate", SqlDbType.DateTimeOffset, field.CreatedDate),
                Connection.CreateCommandParameter("@ModifiedDate", SqlDbType.DateTimeOffset, field.ModifiedDate),
            };
            _ = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FieldSqlScripts.InsertIntoFieldTableSqlCommand, parameters), cancellationToken);

            // Add column to Document table
            var columnType = field.Type.GetSqlColumnType();
            if (columnType != null)
            {
                // TODO: add these?
                //var defaultValue = field.DefaultValue == null ? string.Empty : $"DEFAULT({field.DefaultValue}) WITH VALUES";
                //var index = field.IsIndexed ? $"CREATE INDEX IX_{id} ON Document({id});" : string.Empty;

                //var cmdText2 = $@"ALTER TABLE Document ADD {id} {columnType} {defaultValue}; {index};";
                //
                var cmdIdx = field.Type == FieldType.LongText || field.Type == FieldType.Code ? string.Empty : string.Format(CultureInfo.InvariantCulture, FieldSqlScripts.CreateDocumentIndexSqlCommand, id, id);
                var cmdText2 = string.Format(CultureInfo.InvariantCulture, FieldSqlScripts.AlterDocumentTableSqlCommand, id, columnType, cmdIdx);
                _ = await Connection.ExecuteNonQueryAsync(cmdText2, cancellationToken);
            }
        }

        public override async Task UpdateAsync(Field field, CancellationToken cancellationToken)
        {
            var id = field.Id; //GenerateId(field.Id);

            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.NVarChar, id),
                Connection.CreateCommandParameter("@Name", SqlDbType.NVarChar, field.Name ?? string.Empty),
                Connection.CreateCommandParameter("@Description", SqlDbType.NVarChar, field.Description ?? string.Empty),
                Connection.CreateCommandParameter("@IsRelational", SqlDbType.Bit, field.IsRelational),
                Connection.CreateCommandParameter("@IsIncludeInTextSearch", SqlDbType.Bit, field.IsIncludeInTextSearch),
                Connection.CreateCommandParameter("@CodeConfiguration", SqlDbType.NVarChar, JsonConvert.SerializeObject(field.CodeConfiguration)),
                Connection.CreateCommandParameter("@IsRequiredOnCodeSets", SqlDbType.Bit, field.IsRequiredOnCodeSets),
            };
            _ = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FieldSqlScripts.UpdateFieldTableSqlCommand, parameters), cancellationToken);
        }

        public override async Task DeleteAsync(Field field, CancellationToken cancellationToken)
        {
            var cmdIdx = field.Type == FieldType.LongText || field.Type == FieldType.Code ? string.Empty : string.Format(CultureInfo.InvariantCulture, FieldSqlScripts.AlterTableRemoveIndex, field.Id);
            var cmdText = string.Format(CultureInfo.InvariantCulture, FieldSqlScripts.AlterTableRemoveColumn, field.Id, cmdIdx);

            _ = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(cmdText), cancellationToken);

            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.NVarChar, field.Id)
            };
            _ = await Connection!.ExecuteNonQueryAsync(Connection.CreateCommand(FieldSqlScripts.DeleteFieldTableSqlCommand, parameters), cancellationToken);
        }


        public override async Task<Field> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Id", SqlDbType.NVarChar, id),
            };
            return (await Connection!.ExecuteQueryAsync(Connection.CreateCommand(FieldSqlScripts.FindFieldByIdSqlCommand, parameters), FieldHelpers.ReadFields, cancellationToken)).SingleOrDefault();
        }

        public override async Task<Field> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                Connection.CreateCommandParameter("@Name", SqlDbType.NVarChar, name),
            };
            return (await Connection!.ExecuteQueryAsync(Connection.CreateCommand(FieldSqlScripts.FindFieldByNameSqlCommand, parameters), FieldHelpers.ReadFields, cancellationToken)).SingleOrDefault();
        }

        public override Task<IList<Field>> FindAllAsync(CancellationToken cancellationToken)
        {
            return Connection!.ExecuteQueryAsync(Connection.CreateCommand(FieldSqlScripts.FindAllFieldSqlCommand), FieldHelpers.ReadFields, cancellationToken);
        }
    }
}
