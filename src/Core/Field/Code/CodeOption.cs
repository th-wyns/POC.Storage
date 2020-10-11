using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Configuration description for code option.
    /// </summary>
    /// <seealso cref="POC.Storage.SubcodeOption" />
    public class CodeOption
    {
        /// <summary>
        /// Gets or sets the value of the option.
        /// </summary>
        /// <value>
        /// The value of the option.
        /// </value>
        public string Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subcode options.
        /// </summary>
        /// <value>
        /// The subcode options.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only
        public SubcodeOption Subcode { get; set; } = new SubcodeOption();
#pragma warning restore CA2227 // Collection properties should be read only

        
    }
}
