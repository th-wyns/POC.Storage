using System;

namespace POC.Storage
{
    /// <summary>
    /// Query Orchestrator Interface
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IQueryOrchestrator : IDisposable
    {
        /// <summary>
        /// Sets the metadata provider.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        void SetMetadataProvider(IMetadataProvider metadataProvider);

        /// <summary>
        /// Executes this instance.
        /// </summary>
        void Execute();
    }
}
