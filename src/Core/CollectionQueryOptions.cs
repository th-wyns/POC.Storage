namespace POC.Storage
{
    /// <summary>
    /// Provides abstraction for representing query options for pageable collections.
    /// </summary>
    public abstract class CollectionQueryOptions
    {
        /// <summary>
        /// Gets or sets the number of rows to skip, before starting to return rows from the query expression.
        /// </summary>
        /// <value>
        /// The number of rows to skip.
        /// </value>
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets the number of rows to return, after processing the offset clause.
        /// </summary>
        /// <value>
        /// The number of rows to return.
        /// </value>
        public int? Limit { get; set; }
    }
}
