using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Representing query options for Document object.
    /// </summary>
    /// <seealso cref="POC.Storage.CollectionQueryOptions" />
    public class DocumentQueryOptions : CollectionQueryOptions
    {
        /// <summary>
        /// Option to provide document ids to filter for.
        /// </summary>
        /// <value>
        /// The document ids to filter for.
        /// </value>
        public HashSet<long> Ids { get; } = new HashSet<long>();

        /// <summary>
        /// Option to provide parent ids to filter for.
        /// </summary>
        /// <value>
        /// The parent ids to filter for.
        /// </value>
        public HashSet<string?> ParentIds { get; } = new HashSet<string?>();

        /// <summary>
        /// Option to customize the response view to include only certain fields in the result.
        /// </summary>
        /// <value>
        /// The field names to return in the result view.
        /// </value>
        public HashSet<string> FieldNames { get; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets the field name to order by.
        /// </summary>
        /// <value>
        /// The field name to order by.
        /// </value>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Gets or sets the order type.
        /// </summary>
        /// <value>
        /// The order type.
        /// </value>
        public bool OrderAsc { get; set; }
    }
}
