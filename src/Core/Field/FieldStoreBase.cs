using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace POC.Storage
{

    /// <summary>
    /// Field Store Base Implementation
    /// </summary>
    /// <seealso cref="POC.Storage.IFieldStore" />
    public abstract class FieldStoreBase : IFieldStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldStoreBase"/> class.
        /// </summary>
        public FieldStoreBase()
        {
        }

        /// <summary>
        /// Adds the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public abstract Task CreateAsync(Field field, CancellationToken cancellationToken);


        /// <summary>
        /// Deletes the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public abstract Task DeleteAsync(Field field, CancellationToken cancellationToken);


        /// <summary>
        /// Gets all fields.
        /// </summary>
        /// <returns></returns>
        public abstract Task<IList<Field>> FindAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the field for the specified identifier.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        public abstract Task<Field> FindByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the field for the specified name.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        public abstract Task<Field> FindByNameAsync(string name, CancellationToken cancellationToken);


        /// <summary>
        /// Updates the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>All fields.</returns>
        public abstract Task UpdateAsync(Field field, CancellationToken cancellationToken);

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
