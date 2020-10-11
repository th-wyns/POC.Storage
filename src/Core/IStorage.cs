namespace POC.Storage
{
    public interface IStorage
    {
        public IMetadataProvider Metadata { get; }
        public ISearchProvider Search { get; }
        public IAuditReportProvider AuditReport { get; }
    }
}
