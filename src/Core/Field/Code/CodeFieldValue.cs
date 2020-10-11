using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Code field value object.
    /// </summary>
    public class CodeFieldValue
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<CodeValue> Code { get; set; } = new List<CodeValue>();
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
