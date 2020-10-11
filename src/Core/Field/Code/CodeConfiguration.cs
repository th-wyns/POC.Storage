using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Configuration description for code field.
    /// </summary>
    public class CodeConfiguration
    {
        /// <summary>
        /// Gets or sets the code field type.
        /// </summary>
        /// <value>
        /// The code field type.
        /// </value>
        public CodeType Type { get; set; }

        /// <summary>
        /// Gets the code options.
        /// </summary>
        /// <value>
        /// The code options.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<CodeOption> Code { get; set; } = new List<CodeOption>();
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
