using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeValue
    {
        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        /// <value>
        /// The code value.
        /// </value>
        public string Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subcode.
        /// </summary>
        /// <value>
        /// The subcode.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only
        public SubcodeValue Subcode { get; set; } = new SubcodeValue();
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
