using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace POC.Storage
{
    /// <summary>
    /// Provides methods for generating unique identifiers.
    /// </summary>
    [DebuggerStepThrough]
    internal static class IdUtilities
    {
        /// <summary>
        /// Convert a string id to the proper TKey using Convert.ChangeType.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FromString(string id)
            => id == null ? default : Convert.ToInt32(id, CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts the provided <paramref name="id" /> to a strongly typed key object.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The id to convert.</param>
        /// <returns>
        /// An instance of <typeparamref name="TKey" /> representing the provided <paramref name="id" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TKey FromString<TKey>(string id) where TKey : struct, IEquatable<TKey>
            => id == null ? default : (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);

        /// <summary>
        /// Converts the provided <paramref name="id" /> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>
        /// An <see cref="string" /> representation of the provided <paramref name="id" />.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">A valid identifier is required.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(int id)
            => ToNullableString(id) ?? throw new InvalidOperationException();

        /// <summary>
        /// Converts the provided <paramref name="id" /> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>
        /// An <see cref="string" /> representation of the provided <paramref name="id" />.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">A valid identifier is required.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString<TKey>(TKey id) where TKey : IEquatable<TKey>
            => ToNullableString(id) ?? throw new InvalidOperationException();

        /// <summary>
        /// Converts the provided <paramref name="id" /> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>
        /// An <see cref="string" /> representation of the provided <paramref name="id" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToNullableString(int id)
            => id == default ? default : Convert.ToString(id, CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts the provided <paramref name="id" /> to its string representation.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The id to convert.</param>
        /// <returns>
        /// An <see cref="string" /> representation of the provided <paramref name="id" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToNullableString<TKey>(TKey id) where TKey : IEquatable<TKey>
            => Equals(id, default(TKey)!) ? default : id.ToString();
    }
}
