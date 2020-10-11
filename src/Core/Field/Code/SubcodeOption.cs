using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Configuration for subcode option.
    /// </summary>
    public class SubcodeOption
    {
        /// <summary>
        /// Gets or sets the subcode field type.
        /// </summary>
        /// <value>
        /// The subcode field type.
        /// </value>
        public CodeType Type { get; set; }

        /// <summary>
        /// Gets or sets whether the related subcode selection is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  the related subcode selection is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        /// <value>
        /// The field value.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<string> Values { get; set; } = new List<string>();
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
