using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace POC.Storage.Null
{
    /// <summary>
    /// Null Audit Report Provider Implementation 
    /// </summary>
    /// <seealso cref="POC.Storage.AuditReportProviderBase" />
    public class NullAuditReportProvider : AuditReportProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="NullAuditReportProvider"/> class.
        /// </summary>
        public NullAuditReportProvider()
        {
        }

        /// <summary>
        /// Logs this instance.
        /// </summary>
        public override void Log()
        {
            Trace.WriteLine("NullAuditReportProvider.Log");
        }
    }
}
