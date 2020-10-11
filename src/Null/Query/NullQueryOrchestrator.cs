using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null Query Orchestrator Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.QueryOrchestratorBase" />
    public class NullQueryOrchestrator : QueryOrchestratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullQueryOrchestrator"/> class.
        /// </summary>
        public NullQueryOrchestrator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullQueryOrchestrator"/> class.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public NullQueryOrchestrator(IMetadataProvider metadataProvider) : base(metadataProvider)
        {
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            Trace.WriteLine("Null.Execute");
        }
    }
}
