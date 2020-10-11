using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Name utilities.
    /// </summary>
    /// <remarks>
    /// As a thumb of rules, we should just allow extend custom/user-defined types
    /// but not built-in or external CLR types to avoid naming conflicts with creator of APIs.
    /// Instead, create a utility class and use that.
    /// </remarks>
    internal static class NameUtilities
    {
        [return: NotNull]
        public static string NumberedUniqueDisplayName([NotNull]string displayName, Func<string, bool> CheckNameIsAvailable)
        {
            while (!CheckNameIsAvailable(displayName))
            {
                displayName = IncrementNameSuffix(displayName, out _);
            }

            return displayName;
        }

        public async static Task<string> NumberedUniqueDisplayNameAsync(string displayName, Func<string, Task<bool>> CheckNameIsAvailable)
        {
            while (!await CheckNameIsAvailable(displayName))
            {
                displayName = IncrementNameSuffix(displayName, out _);
            }

            return displayName;
        }

        private static string IncrementNameSuffix(string name, out string nameBase, bool IsFile = false)
        {
            //name = RepositoryPath.GetFileName(name);
            var ext = string.Empty;
            var fileName = name;
            if (IsFile)
            {
                ext = System.IO.Path.GetExtension(name);
                fileName = System.IO.Path.GetFileNameWithoutExtension(name);
            }
            var index = ParseSuffix(fileName, out nameBase, out var inValidNumber);
            var newName = (inValidNumber) ?
                String.Format(CultureInfo.InvariantCulture, "{0}({1}){2}", nameBase, 1, ext) : //Guid.NewGuid().ToString(), ext) :
                String.Format(CultureInfo.InvariantCulture, "{0}({1}){2}", nameBase, ++index, ext);
            return newName;
        }

        /// <summary>
        /// Parses name from format 'name(x)'
        /// </summary>
        /// <param name="name">name to parse</param>
        /// <param name="nameBase">parsed namebase</param>
        /// <param name="inValidNumber">true if correct format is detected but (x) is not a valid number</param>
        /// <returns>the parsed number in suffix</returns>
        private static int ParseSuffix(string name, out string nameBase, out bool inValidNumber)
        {
            nameBase = name;
            inValidNumber = false;
            if (!name.EndsWith(")", false, CultureInfo.InvariantCulture))
                return 0;
            var p = name.LastIndexOf("(", StringComparison.InvariantCulture);
            if (p < 0)
                return 0;
            nameBase = name.Substring(0, p);
            var n = name.Substring(p + 1, name.Length - p - 2);
            if (Int32.TryParse(n, out var result))
                return result;
            inValidNumber = true;
            nameBase = name;
            return 0;
        }
    }
}
