using System;
using System.Runtime.CompilerServices;

namespace POC.Storage
{
    /// <summary>
    /// Exposes conversion helpers.
    /// </summary>
    /// <remarks>
    /// As a thumb of rules, we should just allow extend custom/user-defined types
    /// but not built-in or external CLR types to avoid naming conflicts with creator of APIs.
    /// Instead, create a utility class and use that.
    /// </remarks>
    internal static class ConvertUtilities
    {
        /// <summary>
        /// Converts the specified <paramref name="value" /> to <see cref="bool" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean(object value)
            => value is int valueAsInt ? (valueAsInt > 0) : ((bool?)value ?? false);

        /// <summary>
        /// Converts the specified <paramref name="value" /> to <see cref="int" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(object value)
            => (int?)value ?? 0;

        /// <summary>
        /// Converts the specified <paramref name="value" /> to <see cref="decimal" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal ToDecimal(object value)
            => (decimal?)value ?? 0;

        /// <summary>
        /// Converts the specified <paramref name="value" /> to <see cref="DateTime" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime(object value)
            => (DateTime?)value ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        /// <summary>
        /// Converts the specified <paramref name="value" /> to <see cref="string" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(object value)
            => (string)value ?? string.Empty;
    }
}
