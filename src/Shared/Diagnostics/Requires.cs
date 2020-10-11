using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace POC.Storage
{
    /// <summary>
    /// Common runtime checks that throw <see cref="ArgumentException" /> upon failure.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Requires
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if a condition does not evaluate to true.
        /// </summary>
        public static void Argument([DoesNotReturnIf(false)] bool condition, string parameterName, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c></exception>
        public static void NotNull<T>([ValidatedNotNull] T value, string parameterName)
            where T : class // Ensures value-types aren't passed to a null checking method.
        {
            if (value == null)
            {
                FailArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null.  It passes through the specified value back as a return value.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>
        /// The value of the parameter.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c></exception>
        [return: NotNull]
        public static T NotNullPassthrough<T>([ValidatedNotNull] T value, string parameterName)
            where T : class // Ensures value-types aren't passed to a null checking method.
        {
            NotNull(value, parameterName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c></exception>
        /// <remarks>
        /// This method exists for callers who themselves only know the type as a generic parameter which
        /// may or may not be a class, but certainly cannot be null.
        /// </remarks>
        public static void NotNullAllowStructs<T>([ValidatedNotNull] T value, string parameterName)
        {
            if (null == value)
            {
                FailArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was null.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [DoesNotReturn]
        private static void FailArgumentNullException(string parameterName)
        {
            // Separating out this throwing operation helps with inlining of the caller.
            throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException" /> if a condition does not evaluate to true.
        /// </summary>
        public static void Range([DoesNotReturnIf(false)] bool condition, string parameterName)
        {
            if (!condition)
            {
                FailRange(parameterName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if a condition does not evaluate to true.
        /// </summary>
        public static void Range([DoesNotReturnIf(false)] bool condition, string parameterName, string message)
        {
            if (!condition)
            {
                FailRange(parameterName, message);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [DoesNotReturn]
        public static void FailRange(string parameterName)
        {
            throw new ArgumentOutOfRangeException(parameterName);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [DoesNotReturn]
        public static void FailRange(string parameterName, string message)
        {
            throw new ArgumentOutOfRangeException(parameterName, message);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException" /> if a condition does not evaluate to true.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Valid([DoesNotReturnIf(false)] bool condition, string message)
        {
            if (!condition)
            {
                FailInvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException" /> if the specified value is null.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ValidRef<T>([ValidatedNotNull] T value, string message)
        {
            if (value == null)
            {
                FailInvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException" /> if the specified value is null.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [return: NotNull]
        public static T ValidRefPassthrough<T>([ValidatedNotNull] T value, string message)
        {
            if (value == null)
            {
                FailInvalidOperationException(message);
            }
            return value;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        [DoesNotReturn]
        public static void FailInvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException" /> for a disposed object.
        /// </summary>
        /// <typeparam name="TDisposed">Specifies the type of the disposed object.</typeparam>
        /// <param name="disposed">The disposed object.</param>
        /// <exception cref="System.ObjectDisposedException"></exception>
        [DoesNotReturn]
#if NET45 || NET451
        [MethodImpl(MethodImplOptions.NoInlining)] // Inlining this on .NET < 4.5.2 on x64 causes InvalidProgramException.
#endif
        public static void FailObjectDisposed<TDisposed>(TDisposed disposed) where TDisposed : class
        {
            // Separating out this throwing helps with inlining of the caller, especially
            // due to the retrieval of the type's name.
            throw new ObjectDisposedException(disposed.GetType().FullName);
        }
    }
}
