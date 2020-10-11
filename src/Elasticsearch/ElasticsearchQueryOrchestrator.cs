using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Elasticsearch
{

    /// <summary>
    /// Elasticsearch Query Orchestrator Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.QueryOrchestratorBase" />
    public class ElasticSearchQueryOrchestrator : QueryOrchestratorBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearchQueryOrchestrator"/> class.
        /// </summary>
        public ElasticSearchQueryOrchestrator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearchQueryOrchestrator"/> class.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public ElasticSearchQueryOrchestrator(IMetadataProvider metadataProvider) : base(metadataProvider)
        {
        }


        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {

        }
    }
}
