using System;
using System.Diagnostics;

namespace POC.Storage
{
    /// <summary>
    /// Common compile-time assertions.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Asserts
    {
        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        [Conditional("DEBUG")]
        public static void NotNull<T>([ValidatedNotNull] T value, string parameterName)
            where T : class // Ensures value-types aren't passed to a null checking method.
        {
            if (value == null)
            {
                FailArgumentAssertion(parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <remarks>
        /// This method exists for callers who themselves only know the type as a generic parameter which
        /// may or may not be a class, but certainly cannot be null.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void NotNullAllowStructs<T>([ValidatedNotNull] T value, string parameterName)
        {
            if (null == value)
            {
                FailArgumentAssertion(parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was null.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [Conditional("DEBUG")]
        private static void FailArgumentAssertion(string parameterName)
        {
            // Separating out this throwing operation helps with inlining of the caller.
            Debug.Fail($"The parameter '{parameterName}' should not be null.");
        }
    }
}
