namespace POC.Storage
{

    /// <summary>
    /// Type of the field.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Long text with out length limitation.
        /// </summary>
        LongText,
        /// <summary>
        /// Text with length limitation.
        /// </summary>
        FixedLengthText,
        /// <summary>
        /// Date time type.
        /// </summary>
        Date,
        /// <summary>
        /// Floating point number.
        /// </summary>
        Number,
        /// <summary>
        /// Code / Sub-code values.
        /// </summary>
        Code
    }
}
