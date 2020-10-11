using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POC.Storage
{
    /// <summary>
    /// Interface for Field object store implementations.
    /// </summary>
    public interface IFieldStore : IDisposable
    {
        /// <summary>
        /// Adds the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task CreateAsync(Field field, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task UpdateAsync(Field field, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        Task DeleteAsync(Field field, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the field for the specified identifier.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        Task<Field> FindByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the field for the specified name.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The field.</returns>
        Task<Field> FindByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all fields.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All fields.</returns>
        Task<IList<Field>> FindAllAsync(CancellationToken cancellationToken);
    }
}
