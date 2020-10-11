
namespace POC.Storage
{
    /// <summary>
    /// The storage module.
    /// </summary>
    /// <seealso cref="POC.Storage.IStorage" />
    public class Storage : IStorage
    {
        IStorageConfiguration Configuration { get; }
        /// <summary>
        /// Gets the metadata accessor.
        /// </summary>
        /// <value>
        /// The metadata accessor.
        /// </value>
        public IMetadataProvider Metadata { get; }
        /// <summary>
        /// Gets the search accessor.
        /// </summary>
        /// <value>
        /// The search accessor.
        /// </value>
        public ISearchProvider Search { get; }
        /// <summary>
        /// Gets the audit report accessor.
        /// </summary>
        /// <value>
        /// The audit report accessor.
        /// </value>
        public IAuditReportProvider AuditReport { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Storage"/> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        public Storage(string projectId) : this(new AppSettingsBasedConfiguration(projectId, true).Init())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Storage"/> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="configuration">The configuration.</param>
        public Storage(string projectId, Microsoft.Extensions.Configuration.IConfiguration configuration) : this(new AppSettingsBasedConfiguration(projectId, configuration).Init())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Storage"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Storage(IStorageConfiguration configuration)
        {
            Configuration = configuration;
            Metadata = Configuration.MetadataProvider;
            Search = Configuration.SearchProvider;
            AuditReport = Configuration.AuditReportProvider;
        }
    }
}
