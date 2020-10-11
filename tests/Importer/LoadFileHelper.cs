using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IOFile = System.IO.File;

namespace POC.Storage.Importer
{
    class LoadFileHelper
    {
        const string loadFolder = ".\\LoadFileSample";
        const string loadFile = "LoadFile.dat";
        const char lineSeparator = 'þ';
        const string colSeparator = "þþ";
        internal const string TextExtractCol = "Text_Extract";
        internal const string Update1Col = "U1";
        internal const string Update2Col = "U2";

        internal static void Load(out Dictionary<string, string> fieldSchema, out List<Dictionary<string, object>> documentsFields)
        {
            documentsFields = new List<Dictionary<string, object>>();
            var text = IOFile.ReadAllText(Path.Combine(loadFolder, loadFile));
            var lines = Regex.Split(text, "\r\n|\r|\n");
            fieldSchema = Regex.Split(lines[0].Trim(lineSeparator), colSeparator)
                .Where(fieldId => !string.IsNullOrEmpty(fieldId))
                .ToDictionary(fieldId => fieldId.Replace(' ', '_'), fieldId => fieldId);
            fieldSchema.Add(TextExtractCol, TextExtractCol);
            fieldSchema.Add(Update1Col, Update1Col);
            fieldSchema.Add(Update2Col, Update2Col);

            var fieldIds = fieldSchema.Keys.ToList();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var values = Regex.Split(lines[i].Substring(0, lines[i].Length - 1), colSeparator);
                var fields = new Dictionary<string, object>();
                for (int j = 0; j < values.Length && j < fieldSchema.Count; j++)
                {
                    fields.Add(fieldIds[j], values[j]);
                }

                // load text extract
                var textFieldId = "Text_Precedence";
                var textPath = fields.ContainsKey(textFieldId) ? fields[textFieldId].ToString() : null;
                if (!string.IsNullOrEmpty(textPath))
                {
                    var textExtract = IOFile.ReadAllText(Path.Combine(loadFolder, textPath));
                    fields.Add(TextExtractCol, textExtract);
                }

                documentsFields.Add(fields);
            }
        }

        public static string TrimEnd(string source, string value)
        {
            if (!source.EndsWith(value, StringComparison.Ordinal))
                return source;

            return source.Remove(source.LastIndexOf(value, StringComparison.Ordinal));
        }
    }
}
