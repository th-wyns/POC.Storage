using System;
using System.Collections.Generic;
using System.Reflection;

namespace POC.Storage
{
    /// <summary>
    /// Base class for configuration.
    /// </summary>
    /// <seealso cref="POC.Storage.IStorageConfiguration" />
    public abstract class ConfigurationBase : IStorageConfiguration
    {
        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public string ProjectId { get; }
        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        public abstract string DbConnectionString { get; }
        /// <summary>
        /// Gets the index connection string.
        /// </summary>
        /// <value>
        /// The index connection string.
        /// </value>
        public abstract string IndexConnectionString { get; }
        /// <summary>
        /// Gets the binary connection string.
        /// </summary>
        /// <value>
        /// The binary connection string.
        /// </value>
        public abstract string BinaryConnectionString { get; }
        /// <summary>
        /// Gets the name of the metadata provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the metadata provider assembly qualified.
        /// </value>
        public abstract string MetadataProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the search provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the search provider assembly qualified.
        /// </value>
        public abstract string SearchProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the audit report provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the audit report provider assembly qualified.
        /// </value>
        public abstract string AuditReportProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets the name of the binary provider assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the binary provider assembly qualified.
        /// </value>
        public abstract string BinaryProviderAssemblyQualifiedName { get; }
        /// <summary>
        /// Gets or sets the metadata provider.
        /// </summary>
        /// <value>
        /// The metadata provider.
        /// </value>
        public abstract IMetadataProvider MetadataProvider { get; protected set; }
        /// <summary>
        /// Gets or sets the search provider.
        /// </summary>
        /// <value>
        /// The search provider.
        /// </value>
        public abstract ISearchProvider SearchProvider { get; protected set; }
        /// <summary>
        /// Gets or sets the audit report provider.
        /// </summary>
        /// <value>
        /// The audit report provider.
        /// </value>
        public abstract IAuditReportProvider AuditReportProvider { get; protected set; }
        /// <summary>
        /// Gets or sets the binary provider.
        /// </summary>
        /// <value>
        /// The binary provider.
        /// </value>
        public abstract IBinaryProvider BinaryProvider { get; protected set; }
        /// <summary>
        /// Gets the supported binary providers.
        /// </summary>
        /// <value>
        /// The supported binary providers.
        /// </value>
        public Dictionary<string, IBinaryProvider> SupportedBinaryProviders { get; } = new Dictionary<string, IBinaryProvider>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBase"/> class.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        public ConfigurationBase(string projectId)
        {
            ProjectId = projectId;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public IStorageConfiguration Init()
        {
            FillSupportedBinaryProviders();
            AuditReportProvider = CreateAuditReportProvider();
            BinaryProvider = CreateBinaryProvider();
            SearchProvider = CreateSearchProvider() ?? throw new NullReferenceException("SearchProvider is null");
            MetadataProvider = CreateMetadataProvider(SearchProvider.IndexStore, AuditReportProvider);
            SearchProvider.SetMetadataProvider(MetadataProvider);
            return this;
        }

        static readonly Type[] s_metadataProviderCtorSignature = new[] { typeof(string), typeof(string), typeof(IBinaryProvider), typeof(Dictionary<string, IBinaryProvider>), typeof(IIndexStore), typeof(IAuditReportProvider) };
        static readonly Type[] s_searchProviderCtorSignature = new[] { typeof(string), typeof(string) };
        static readonly Type[] s_auditReportProviderCtorSignature = Array.Empty<Type>();
        static readonly Type[] s_binaryProviderCtorSignature = new[] { typeof(string), typeof(string) };

        /// <summary>
        /// Creates the metadata provider.
        /// </summary>
        /// <param name="indexStore">The index store.</param>
        /// <param name="auditReportProvider">The audit report provider.</param>
        /// <returns></returns>
        protected IMetadataProvider CreateMetadataProvider(IIndexStore indexStore, IAuditReportProvider auditReportProvider)
        {
            return (IMetadataProvider)Activator.GetActivator(MetadataProviderAssemblyQualifiedName, s_metadataProviderCtorSignature)(ProjectId, DbConnectionString, BinaryProvider, SupportedBinaryProviders, indexStore, auditReportProvider);
        }

        /// <summary>
        /// Creates the search provider.
        /// </summary>
        /// <returns></returns>
        protected ISearchProvider CreateSearchProvider()
        {
            return (ISearchProvider)Activator.GetActivator(SearchProviderAssemblyQualifiedName, s_searchProviderCtorSignature)(ProjectId, IndexConnectionString);
        }

        /// <summary>
        /// Creates the audit report provider.
        /// </summary>
        /// <returns></returns>
        protected IAuditReportProvider CreateAuditReportProvider()
        {
            return (IAuditReportProvider)Activator.GetActivator(AuditReportProviderAssemblyQualifiedName, s_auditReportProviderCtorSignature)();
        }

        /// <summary>
        /// Creates the binary provider.
        /// </summary>
        /// <returns></returns>
        protected IBinaryProvider CreateBinaryProvider()
        {
            // get from supported
            return SupportedBinaryProviders[BinaryProviderAssemblyQualifiedName];
            //return (IBinaryProvider)Activator.GetActivator(BinaryProviderAssemblyQualifiedName, s_binaryProviderCtorSignature)(BinaryConnectionString, ProjectId);
        }

        /// <summary>
        /// Fills the supported binary providers.
        /// </summary>
        protected void FillSupportedBinaryProviders()
        {
            // TODO:
            // get config SupportedBinaryProviders
            // foreach
            //  get connectionstring
            //  create
            var nsb = (IBinaryProvider)Activator.GetActivator(BinaryProviderAssemblyQualifiedName, s_binaryProviderCtorSignature)(BinaryConnectionString, ProjectId);
            SupportedBinaryProviders[BinaryProviderAssemblyQualifiedName] = nsb;
        }

        static readonly Dictionary<string, MethodInfo> s_createIndexStoreMethodCache = new Dictionary<string, MethodInfo>();
        /// <summary>
        /// Creates the index store.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Missing static 'CreateIndexStore' method from {targetClass}.</exception>
        public IIndexStore CreateIndexStore()
        {
            var targetClass = SearchProviderAssemblyQualifiedName;
            if (!s_createIndexStoreMethodCache.ContainsKey(targetClass))
            {
                var createIndexMethod = Type.GetType(targetClass).GetMethod("CreateIndexStore", BindingFlags.Public | BindingFlags.Static);
                if (createIndexMethod == null)
                {
#pragma warning disable IDE0016 // Use 'throw' expression
                    throw new NotImplementedException($"Missing static 'CreateIndexStore' method from {targetClass}.");
#pragma warning restore IDE0016 // Use 'throw' expression
                }
                s_createIndexStoreMethodCache[targetClass] = createIndexMethod;
            }
            var indexStore = (IIndexStore)s_createIndexStoreMethodCache[targetClass].Invoke(null, null);
            return indexStore;
        }
    }
}
