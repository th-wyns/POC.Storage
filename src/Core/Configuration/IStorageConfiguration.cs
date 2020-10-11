namespace POC.Storage
{
    /// <summary>
    /// /**/
    /// </summary>
    public interface IStorageConfiguration
    {
        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        string DbConnectionString { get; }
        /// <summary>
        /// Gets the index connection string.
        /// </summary>
        /// <value>
        /// The index connection string.
        /// </value>
        string IndexConnectionString { get; }
        /// <summary>
        /// Gets the binary connection string.
        /// </summary>
        /// <value>
        /// The binary connection string.
        /// </value>
        string BinaryConnectionString { get; }
        /// <summary>
        /// Gets the name of the metadata provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the metadata provider assembly qualified.
        /// </value>
        string MetadataProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the search provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the search provider assembly qualified.
        /// </value>
        string SearchProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the audit report provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the audit report provider assembly qualified.
        /// </value>
        string AuditReportProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the metadata provider.
        /// </summary>
        /// <value>
        /// The metadata provider.
        /// </value>
        IMetadataProvider MetadataProvider { get; }
        /// <summary>
        /// Gets the search provider.
        /// </summary>
        /// <value>
        /// The search provider.
        /// </value>
        ISearchProvider SearchProvider { get; }
        /// <summary>
        /// Gets the audit report provider.
        /// </summary>
        /// <value>
        /// The audit report provider.
        /// </value>
        IAuditReportProvider AuditReportProvider { get; }
        /// <summary>
        /// Gets the binary provider.
        /// </summary>
        /// <value>
        /// The binary provider.
        /// </value>
        IBinaryProvider BinaryProvider { get; }
        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public string ProjectId { get; }
    }
}
