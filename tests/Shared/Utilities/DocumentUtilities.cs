using System;
using System.Collections.Generic;
using System.Linq;

namespace POC.Storage
{
    static class DocumentUtilities
    {
        static Random rnd = new Random();

        internal static Document GetRandomDocument(IEnumerable<Field> fields = default!)
        {
            var fieldValues = new Dictionary<string, object>();
            foreach (var field in fields ?? Enumerable.Empty<Field>())
            {
                fieldValues.Add(field.Id, FieldUtilities.GetRandomValue(field.Type));
            }

            return new Document
            {
                ParentId = $"Parent_{rnd.Next()}"
            }.SetFields(fieldValues);
        }
    }
}
