using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{

    /// <summary>
    /// Field Manager Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IFieldManager" />
    public class FieldManager : IFieldManager
    {
        /// <summary>
        /// Gets the field store.
        /// </summary>
        /// <value>
        /// The field store.
        /// </value>
        public IFieldStore FieldStore { get; }

        /// <summary>
        /// Gets the index store.
        /// </summary>
        /// <value>
        /// The index store.
        /// </value>
        public IIndexStore IndexStore { get; }

        /// <summary>
        /// Gets the audit report provider.
        /// </summary>
        /// <value>
        /// The audit report provider.
        /// </value>
        public IAuditReportProvider AuditReportProvider { get; }


        //TransactionOrchestrator transactionOrchestrator = new TransactionOrchestrator();


        /// <summary>
        /// Initializes a new instance of the <see cref="FieldManager"/> class.
        /// </summary>
        /// <param name="fieldStore">The field store.</param>
        /// <param name="indexStore">The index store.</param>
        /// <param name="auditReportProvider">The audit report provider.</param>
        public FieldManager(IFieldStore fieldStore, IIndexStore indexStore, IAuditReportProvider auditReportProvider)
        {
            FieldStore = fieldStore;
            IndexStore = indexStore;
            AuditReportProvider = auditReportProvider;
        }

        /// <summary>
        /// Adds the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task CreateAsync(Field field, CancellationToken cancellationToken)
        {
            // in transaction
            await FieldStore.CreateAsync(field, cancellationToken);
            //await FieldStore.CreateAsync(field, transactionOrchestrator, cancellationToken);
            await IndexStore.CreateAsync(field/*TransactionOrchestrator*/);
            AuditReportProvider.Log(/*TransactionOrchestrator*/);
        }

        /// <summary>
        /// Updates the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task UpdateAsync(Field field, CancellationToken cancellationToken)
        {
            // in transaction
            await FieldStore.UpdateAsync(field, cancellationToken);
            //await FieldStore.UpdateAsync(field, transactionOrchestrator, cancellationToken);
            IndexStore.Update("", "");
            AuditReportProvider.Log(/*TransactionOrchestrator*/);
        }

        /// <summary>
        /// Deletes the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        public async Task DeleteAsync(Field field, CancellationToken cancellationToken)
        {
            // in transaction
            await FieldStore.DeleteAsync(field, cancellationToken);
            //await DeleteAsync(field, transactionOrchestrator, cancellationToken);
            IndexStore.Delete("");
            AuditReportProvider.Log(/*TransactionOrchestrator*/);
        }

        /// <summary>
        /// Gets the field for the specified identifier.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        public async Task<Field> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await FieldStore.FindByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Gets the field for the specified identifier.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        public async Task<Field> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await FieldStore.FindByNameAsync(name, cancellationToken);
        }

        /// <summary>
        /// Gets all fields.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All fields.</returns>
        public async Task<IList<Field>> FindAllAsync(CancellationToken cancellationToken)
        {
            return await FieldStore.FindAllAsync(cancellationToken);
        }


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
