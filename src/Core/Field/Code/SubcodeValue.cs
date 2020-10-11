using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public class SubcodeValue
    {
        /// <summary>
        /// Gets or sets the subcode value.
        /// </summary>
        /// <value>
        /// The subcode value.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<string> Value { get; set; } = new List<string>();
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
