using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace POC.Storage.MsSql
{

    /// <summary>
    /// MsSql Audit Report Provider Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.AuditReportProviderBase" />
    public class MsSqlAuditReportProvider : AuditReportProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlAuditReportProvider"/> class.
        /// </summary>
        public MsSqlAuditReportProvider()
        {
        }

        /// <summary>
        /// Logs this instance.
        /// </summary>
        public override void Log()
        {
            // TODO: implement
            return;
        }
    }
}
