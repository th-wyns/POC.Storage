using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{

    /// <summary>
    /// Audit Report Provider Base  Iplementation
    /// </summary>
    /// <seealso cref="POC.Storage.IAuditReportProvider" />
    public abstract class AuditReportProviderBase : IAuditReportProvider
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditReportProviderBase"/> class.
        /// </summary>
        public AuditReportProviderBase()
        {
        }

        /// <summary>
        /// Logs this instance.
        /// </summary>
        public abstract void Log();

        #region Dispose
        /// <summary>
        /// Dispose flag.
        /// </summary>
        private bool _disposed;
        
        /// <summary>
        /// Releases all resources used by the user manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
            }
        }
        #endregion
    }
}
