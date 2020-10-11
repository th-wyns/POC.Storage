using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage.Importer
{
    public class DocumentTests
    {
        static Connection Connection { get; } = Connection.Instance;
        public Storage Storage { get; } = new Storage(Connection.ProjectId);


        public async Task ImportAync(int duplicateLimit = 0)
        {
            // load loadfile, parse columns, gather documents
            LoadFileHelper.Load(out var fieldSchema, out var documentsFields);
            var documentsFields2 = new List<Dictionary<string, object>>(documentsFields);
            if (duplicateLimit > 0)
            {
                while (documentsFields2.Count < duplicateLimit)
                {
                    documentsFields2.AddRange(documentsFields);
                }
                documentsFields2 = documentsFields2.Take(duplicateLimit).ToList();
            }

            // create fields
            var existingFields = (await Storage.Metadata.Field.FindAllAsync(CancellationToken.None)).ToDictionary(field => field.Name, field => field);
            foreach (var fieldType in fieldSchema)
            {
                if (!existingFields.ContainsKey(fieldType.Key))
                {
                    var field = new Field(fieldType.Key) { Description = fieldType.Value, Type = fieldType.Key == LoadFileHelper.TextExtractCol ? FieldType.LongText : FieldType.FixedLengthText };
                    await Storage.Metadata.CreateFieldAsync(field);
                }
            }
            existingFields = (await Storage.Metadata.Field.FindAllAsync(CancellationToken.None)).ToDictionary(field => field.Name, field => field);

            // import documents
            var count = await Storage.Metadata.Document.CountAsync(new List<string?>(), CancellationToken.None);

            var documents = new List<Document>();
            foreach (var documentFields in documentsFields2)
            {
                var importDocumentFields = documentFields.ToDictionary(k => existingFields[k.Key].Id, v => v.Value);
                var document = new Document(++count, default!, default!)
                {
                    ParentId = string.Empty
                }.SetFields(importDocumentFields);
                documents.Add(document);
            }

            Console.WriteLine($"CREATING: {documentsFields2.Count} documents");
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            await Storage.Metadata.Document.CreateAsync(documents, CancellationToken.None);
            sw.Stop();
            Console.WriteLine($"CREATED IN: {sw.Elapsed.TotalSeconds} seconds ({Math.Floor(documentsFields2.Count / sw.Elapsed.TotalSeconds)} documents/sec)");

            // import natives

            // parse preview file
            // import previews
        }



        public async Task UpdateAsync()
        {
            // fields
            var fields = await Storage.Metadata.Field.FindAllAsync(CancellationToken.None);
            var u1field = fields.Where(field => field.Name == LoadFileHelper.Update1Col).Single();
            var u2field = fields.Where(field => field.Name == LoadFileHelper.Update2Col).Single();

            // documents
            var opt = new DocumentQueryOptions();
            for (int i = 0; i < 10000; i++)
            {
                opt.Ids.Add(i);
            }
            var documents = await Storage.Metadata.Document.FindAllAsync(opt, CancellationToken.None);

            // set field value parameters
            var fieldValues = new Dictionary<string, object?>
            {
                { u1field.Id, FieldUtilities.GetRandomValue(u1field.Type) },
                { u2field.Id, FieldUtilities.GetRandomValue(u2field.Type) }
            };

            // set fields on documents
            Console.WriteLine($"UPDATING: {documents.Count} documents");
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            await Storage.Metadata.Document.SetFieldsAsync(documents.Select(document => document.Id), fieldValues, CancellationToken.None);
            sw.Stop();
            Console.WriteLine($"UPDATED IN: {sw.Elapsed.TotalSeconds} seconds ({Math.Floor(documents.Count / sw.Elapsed.TotalSeconds)} documents/sec)");
        }
    }
}
