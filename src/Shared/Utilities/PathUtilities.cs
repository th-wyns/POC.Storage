using System;
using System.IO;
using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Provides methods for file system operations.
    /// </summary>
    internal static class PathUtilities
    {
        private readonly static char[] s_trimChars = new[] { ' ', '/', '\\' };

        /// <summary>
        /// Returns the directory information for the specified path string without trailing separator and space characters.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>
        /// Directory information for path, or null if path denotes a root directory or is null.
        /// Returns <see cref="String.Empty" /> if path does not contain directory information.
        /// </returns>
        internal static string GetDirectoryName(string path)
        {
            return TrimDirectoryName(Path.GetDirectoryName(path));
        }

        /// <summary>
        /// Removes all the trailing occurrences of directory separator and space characters.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>
        /// The directory name.
        /// </returns>
        internal static string TrimDirectoryName(string path)
        {
            return path.TrimEnd(s_trimChars);
        }
    }
}
