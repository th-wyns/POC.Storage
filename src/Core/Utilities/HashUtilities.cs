using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace POC.Storage
{
    /// <summary>
    /// Provides methods for computing hash values.
    /// </summary>
    internal static class HashUtilities
    {
        /// <summary>
        /// Computes a new hash value combining the specified values.
        /// </summary>
        /// <param name="hash1">The hash1 to add.</param>
        /// <param name="hash2">The hash2 to add.</param>
        /// <returns>
        /// The computed hash value.
        /// </returns>
        /// <remarks>
        /// http://referencesource.microsoft.com/#System.Web/Util/HashCodeCombiner.cs
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Combine(int hash1, int hash2) => ((hash1 << 5) + hash1) ^ hash2;

        /// <summary>
        /// Computes a new hash value combining the specified values.
        /// </summary>
        /// <param name="hash1">The hash1 to add.</param>
        /// <param name="hash2">The hash2 to add.</param>
        /// <param name="hash3">The hash3 to add.</param>
        /// <returns>
        /// The computed hash value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Combine(int hash1, int hash2, int hash3) => Combine(hash1, Combine(hash2, hash3));

        /// <summary>
        /// Computes a new hash value combining the specified values.
        /// </summary>
        /// <param name="hash1">The hash1 to add.</param>
        /// <param name="hash2">The hash2 to add.</param>
        /// <param name="hash3">The hash3 to add.</param>
        /// <param name="hash4">The hash4 to add.</param>
        /// <returns>
        /// The computed hash value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Combine(int hash1, int hash2, int hash3, int hash4) => Combine(hash1, Combine(hash2, Combine(hash3, hash4)));

        /// <summary>
        /// Computes a new hash value combining an array of hash values of the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="values">The values to include.</param>
        /// <returns>
        /// The computed hash value.
        /// </returns>
        internal static int CombineAll<T>(T[] values)
        {
            if (values.Length == 0)
            {
                return 0;
            }

            var hashCode = 0;
            for (var i = 0; i < values.Length; ++i)
            {
                var value = values[i];
                if (value != null)
                {
                    hashCode = Combine(value.GetHashCode(), hashCode);
                }
            }
            return hashCode;
        }

        /// <summary>
        /// Computes a new hash value combining a sequence of hash values of the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="values">The values to combine.</param>
        /// <returns>
        /// The computed hash value.
        /// </returns>
        internal static int CombineAll<T>(IEnumerable<T> values)
        {
            if (values == null)
            {
                return 0;
            }

            var hashCode = 0;
            foreach (var value in values)
            {
                if (value != null)
                {
                    hashCode = Combine(value.GetHashCode(), hashCode);
                }
            }
            return hashCode;
        }

        /// <summary>
        /// Computes a new hash value combining a sequence of hash values of the specified strings.
        /// </summary>
        /// <param name="values">The values to combine.</param>
        /// <param name="stringComparer">A string comparison operation that uses specific case and culture-based or ordinal comparison rules.</param>
        /// <returns>
        /// The computed hash value.
        /// </returns>
        internal static int CombineAll(IEnumerable<string> values, StringComparer stringComparer)
        {
            if (values == null)
            {
                return 0;
            }

            var hashCode = 0;
            foreach (var value in values)
            {
                if (value != null)
                {
                    hashCode = Combine(stringComparer.GetHashCode(value), hashCode);
                }
            }
            return hashCode;
        }
    }
}
