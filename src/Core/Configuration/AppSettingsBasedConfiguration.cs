using Microsoft.Extensions.Configuration;

namespace POC.Storage
{
    /// <summary>
    /// Appsettings.json based configuration.
    /// </summary>
    /// <seealso cref="POC.Storage.ConfigurationBase" />
    public class AppSettingsBasedConfiguration : ConfigurationBase
    {
        private const string ConfigurationFileName = "appsettings.json";
        private const string ConfigSectionKey = "POC.Storage";
        private const string DbConnectionStringKey = "POC.Storage.DB";
        private const string IndexConnectionStringKey = "POC.Storage.Index";
        private const string BinaryConnectionStringKey = "POC.Storage.Binary";
        private const string MetadataProviderAssemblyQualifiedNameKey = "MetadataProviderAssemblyQualifiedName";
        private const string SearchProviderAssemblyQualifiedNameKey = "SearchProviderAssemblyQualifiedName";
        private const string AuditReportProviderAssemblyQualifiedNameKey = "AuditReportProviderAssemblyQualifiedName";
        private const string BinaryProviderAssemblyQualifiedNameKey = "BinaryProviderAssemblyQualifiedName";

        Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        public override string DbConnectionString { get; }
        /// <summary>
        /// Gets the index connection string.
        /// </summary>
        /// <value>
        /// The index connection string.
        /// </value>
        public override string IndexConnectionString { get; }
        /// <summary>
        /// Gets the binary connection string.
        /// </summary>
        /// <value>
        /// The binary connection string.
        /// </value>
        public override string BinaryConnectionString { get; }
        /// <summary>
        /// Gets the name of the metadata provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the metadata provider assembly qualified.
        /// </value>
        public override string MetadataProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the search provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the search provider assembly qualified.
        /// </value>
        public override string SearchProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the audit report provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the audit report provider assembly qualified.
        /// </value>
        public override string AuditReportProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the binary provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the binary provider assembly qualified.
        /// </value>
        public override string BinaryProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets or sets the metadata provider.
        /// </summary>
        /// <value>
        /// The metadata provider.
        /// </value>
        public override IMetadataProvider MetadataProvider { get; protected set; } = default!;
        /// <summary>
        /// Gets or sets the search provider.
        /// </summary>
        /// <value>
        /// The search provider.
        /// </value>
        public override ISearchProvider SearchProvider { get; protected set; } = default!;
        /// <summary>
        /// Gets or sets the audit report provider.
        /// </summary>
        /// <value>
        /// The audit report provider.
        /// </value>
        public override IAuditReportProvider AuditReportProvider { get; protected set; } = default!;
        /// <summary>
        /// Gets or sets the binary provider.
        /// </summary>
        /// <value>
        /// The binary provider.
        /// </value>
        public override IBinaryProvider BinaryProvider { get; protected set; } = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsBasedConfiguration"/> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="loadKeyVault">if set to <c>true</c> load credentials from key vault.</param>
        public AppSettingsBasedConfiguration(string projectId, bool loadKeyVault) : this(projectId, ConfigurationFileName, loadKeyVault)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsBasedConfiguration"/> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="configurationFileName">Name of the configuration file.</param>
        /// <param name="loadKeyVault">if set to <c>true</c> load credentials from key vault.</param>
        public AppSettingsBasedConfiguration(string projectId, string configurationFileName, bool loadKeyVault) : this(projectId, GetConfiguration(configurationFileName, loadKeyVault))
        {
        }

        // TODO: USE THIS TO AVOID PARSING SETTINGS ALL THE TIME
        // ALTERNATIVELY: CACHE SETTINGS IN STATIC DICT TO AVOID REPARSING IN GetConfiguration
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsBasedConfiguration" /> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="configuration">The configuration.</param>
        public AppSettingsBasedConfiguration(string projectId, Microsoft.Extensions.Configuration.IConfiguration configuration) : base(projectId)
        {
            Configuration = configuration;
            DbConnectionString = GetDbConnectionString();
            IndexConnectionString = GetIndexConnectionString();
            BinaryConnectionString = GetBinaryConnectionString();
            MetadataProviderAssemblyQualifiedName = GetMetadataProviderAssemblyQualifiedName();
            SearchProviderAssemblyQualifiedName = GetSearchProviderAssemblyQualifiedName();
            AuditReportProviderAssemblyQualifiedName = GetAuditReportProviderAssemblyQualifiedName();
            BinaryProviderAssemblyQualifiedName = GetBinaryProviderAssemblyQualifiedName();
        }

        // TODO: CACHE BUILT SETTINGS IN STATIC VARIABLE TO SPEED UP
        static IConfigurationRoot GetConfiguration(string configurationFileName, bool loadKeyVault)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile(configurationFileName, optional: false, reloadOnChange: true);

            return configBuilder.Build();
        }

        string GetDbConnectionString()
        {
            return Configuration.GetConnectionString(DbConnectionStringKey);
        }

        string GetIndexConnectionString()
        {
            return Configuration.GetConnectionString(IndexConnectionStringKey);
        }
        string GetBinaryConnectionString()
        {
            return Configuration.GetConnectionString(BinaryConnectionStringKey);
        }

        string GetMetadataProviderAssemblyQualifiedName()
        {
            return Configuration.GetSection(ConfigSectionKey)[MetadataProviderAssemblyQualifiedNameKey];
        }

        string GetSearchProviderAssemblyQualifiedName()
        {
            return Configuration.GetSection(ConfigSectionKey)[SearchProviderAssemblyQualifiedNameKey];
        }

        string GetAuditReportProviderAssemblyQualifiedName()
        {
            return Configuration.GetSection(ConfigSectionKey)[AuditReportProviderAssemblyQualifiedNameKey];
        }

        string GetBinaryProviderAssemblyQualifiedName()
        {
            return Configuration.GetSection(ConfigSectionKey)[BinaryProviderAssemblyQualifiedNameKey];
        }
    }
}
