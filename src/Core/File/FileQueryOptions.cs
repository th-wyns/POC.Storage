using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Representing query options for File object.
    /// </summary>
    /// <seealso cref="POC.Storage.CollectionQueryOptions" />
    public class FileQueryOptions : CollectionQueryOptions
    {
        /// <summary>
        /// Option to provide file ids to filter for.
        /// </summary>
        /// <value>
        /// The document ids to filter for.
        /// </value>
        public ICollection<long> Ids { get; } = new HashSet<long>();

        /// <summary>
        /// Option to provide parent document ids to filter for.
        /// </summary>
        /// <value>
        /// The parent document ids to filter for.
        /// </value>
        public ICollection<long> DocumentIds { get; } = new HashSet<long>();

        /// <summary>
        /// Option to provide the required file types.
        /// </summary>
        /// <value>
        /// The file types to filter for.
        /// </value>
        public FileType? Type { get; set; } = null;

        /// <summary>
        /// Option to provide the indexes.
        /// </summary>
        /// <value>
        /// The indexes to filter for.
        /// </value>
        public ICollection<int> Indexes { get; } = new HashSet<int>();

    }
}
