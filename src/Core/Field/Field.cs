using System;
using System.Text.RegularExpressions;

namespace POC.Storage
{
    /// <summary>
    /// Field object model.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// The fixed length text maximum length
        /// </summary>
        public const int FixedLengthTextMaxLength = 450;

        /// <summary>
        /// The document identifier field name
        /// </summary>
        public const string DocumentIdFieldName = "Document ID";

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="name">The field name.</param>
        public Field(string name)
        {
            Id = GenerateId(name);
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="createdDate">The creation date.</param>
        /// <param name="modifiedDate">The modification date.</param>
        public Field(string id, DateTimeOffset createdDate, DateTimeOffset modifiedDate)
        {
            Id = id;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; } = default!;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the field type.
        /// </summary>
        /// <value>
        /// The field type.
        /// </value>
        public FieldType Type { get; set; }

        /// <summary>
        /// Indicates whether or not this field is built in or user created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the field is built in; otherwise, <c>false</c>.
        /// </value>
        public bool IsBuiltIn { get; set; }

        /// <summary>
        /// Allows a user to select whether or not this field is intended to be used to group documents together.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the field is intended to be used to group documents together; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelational { get; set; }

        /// <summary>
        /// Gets or sets indicating whether this field should be included in text search.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this field should be included in text search; otherwise, <c>false</c>.
        /// </value>
        public bool IsIncludeInTextSearch { get; set; }

        /// <summary>
        /// Gets or sets indicating whether this field is required on code sets.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this field is required on code sets; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequiredOnCodeSets { get; set; }

        /// <summary>
        /// Gets or sets indicating whether this field is computed.
        /// If this field is computed the the value cannot be updated from outside.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this field is computed; otherwise, <c>false</c>.
        /// </value>
        public bool IsComputed { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTimeOffset CreatedDate { get; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>
        /// <value>
        /// The modification date.
        /// </value>
        public DateTimeOffset ModifiedDate { get; }

        /// <summary>
        /// Gets or sets the code configuration.
        /// </summary>
        /// <value>
        /// The code configuration.
        /// </value>
        public CodeConfiguration? CodeConfiguration { get; set; }

        // TODO: be an option: NotIndexed, Indexed, Analyzed, IndexedAndAnalyzed (Analyze -> TextExtract provider should add to the index)
        //public bool IsIndexed { get; set; }

        string GenerateId(string name)
        {
            var regex = new Regex(@"[^\p{L}\p{N}]+");
            var replacedName = regex.Replace(name, "").ToLowerInvariant();
            return $"{replacedName.Substring(0, Math.Min(50, replacedName.Length))}_{DateTime.UtcNow.Ticks}";
        }
    }
}
