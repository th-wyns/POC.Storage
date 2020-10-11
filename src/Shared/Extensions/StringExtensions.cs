using System;
using System.Runtime.CompilerServices;

namespace POC.Storage
{
    /// <summary>
    /// Provides string extensions.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Converts this string to the specified <typeparamref name="T" /> enum type.
        /// </summary>
        /// <typeparam name="T">The enum type to convert to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="fallbackValue">The fallback value to be used if conversion fails.</param>
        /// <returns>
        /// The enum value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToEnum<T>(this string value, T fallbackValue = default) where T : struct, Enum
            => Enum.TryParse<T>(value, out var result) ? result : fallbackValue;

        /// <summary>
        /// Replace the specified <paramref name="value" /> to <see cref="bool" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReplaceInvariant(this string value, string oldValue, string newValue, bool ignoreCase = false)
            => value.Replace(oldValue, newValue, ignoreCase, System.Globalization.CultureInfo.InvariantCulture);

        /// <summary>
        /// Calls the toString with Invariantculture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringInvariant(this string value)
            => value.ToString(System.Globalization.CultureInfo.InvariantCulture);

        /// <summary>
        /// Endses the with.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="strValue">The string value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWithInvariant(this string value, string strValue, bool ignoreCase = false)
            => value.EndsWith(strValue, ignoreCase, System.Globalization.CultureInfo.InvariantCulture);
    }
}
