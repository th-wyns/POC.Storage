using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace POC.Storage
{
    /// <summary>
    /// Document object model.
    /// </summary>
    public class Document : BaseStorageContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        public Document()
        {
            //Id = GuidUtilities.NewGuidComb().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="createdDate">The creation date.</param>
        /// <param name="modifiedDate">The modification date.</param>
        public Document(long id, DateTimeOffset createdDate = default!, DateTimeOffset modifiedDate = default!)
        {
            Id = id;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTimeOffset CreatedDate { get; } = default!;

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        /// <value>
        /// The modification date.
        /// </value>
        public DateTimeOffset ModifiedDate { get; } = default!;

        /// <summary>
        /// Gets or sets the parent object ID.
        /// </summary>
        /// <value>
        /// The parent object ID.
        /// </value>
        public string? ParentId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the dynamic fields.
        /// </summary>
        /// <value>
        /// The dynamic fields.
        /// </value>
        public IDictionary<string, object?> Fields { get; } = new Dictionary<string, object?>(StringComparer.Ordinal);

        /// <summary>
        /// Sets the dynamic fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public Document SetFields(IDictionary<string, object> fields)
        {
            if (fields == null)
            {
                return this;
            }

            foreach (var field in fields)
            {
                Fields[field.Key] = field.Value;
            }
            return this;
        }
    }
}
