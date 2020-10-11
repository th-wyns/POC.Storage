using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Storage
{

    /// <summary>
    /// Audit Report Provider Interface
    /// </summary>
    /// <seealso cref="POC.Storage.IProvider" />
    public interface IAuditReportProvider : IProvider
    {
        // State enum: initiated, succeeded, failed (e.g: Document | Create | Succeeded)

        /// <summary>
        /// Logs this instance.
        /// </summary>
        void Log();
    }
}
