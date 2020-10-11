using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace POC.Storage
{
    /// <summary>
    /// Provides methods for generating unique identifiers.
    /// </summary>
    [DebuggerStepThrough]
    internal static class GuidUtilities
    {
        private static readonly long s_baseDateTicks = new DateTime(1900, 1, 1).Ticks;

        /// <summary>
        /// Generate a new <see cref="Guid" /> using the comb algorithm.
        /// The <c>comb</c> algorithm is designed to make the use of GUIDs as Primary Keys, Foreign Keys,
        /// and Indexes nearly as efficient as ints. This code was contributed by Donald Mull.
        /// </summary>
        /// <returns>
        /// The new <see cref="Guid" />.
        /// </returns>
        /// <remarks>
        /// https://github.com/nhibernate/nhibernate-core/blob/master/src/NHibernate/Id/GuidCombGenerator.cs
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NewGuidComb()
        {
            var guidArray = Guid.NewGuid().ToByteArray();

            var now = DateTime.UtcNow;

            // Get the days and milliseconds which will be used to build the byte string.
            var days = new TimeSpan(now.Ticks - s_baseDateTicks);
            var msecs = now.TimeOfDay;

            // Convert to a byte array.
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333.
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering.
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid.
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        /// <summary>
        /// Creates a new <see cref="Guid" />.
        /// </summary>
        /// <returns>
        /// The <see cref="Guid" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NewGuid() => Guid.NewGuid();
    }
}
