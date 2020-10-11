using System;
using System.Collections.Generic;

namespace POC.Storage
{
    /// <summary>
    /// Represents an optional value that can be used to express your intention when <c>null</c> has its own meaning in the current context.
    /// A filter object's properties can even use <c>null</c> to create a query that just return results where the filtered property's value is <c>null</c>
    /// so you have to make a difference between either <c>null</c> and "unset" values.
    /// </summary>
    /// <typeparam name="T">The underlying value type of the <see cref="OptionalValue{T}" /> generic type.</typeparam>
    public readonly struct OptionalValue<T> : IEquatable<OptionalValue<T>>
    {
        /// <summary>
        /// Gets a value indicating whether the current object has a value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value of the current object.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalValue{T}" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public OptionalValue(T value)
        {
            HasValue = true;
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <typeparamref name="T" /> to <see cref="OptionalValue{T}" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator OptionalValue<T>(T value)
            => new OptionalValue<T>(value);

        /// <summary>
        /// Returns <c>true</c> if its operands are equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// The result of the comparision.
        /// </returns>
        public static bool operator ==(OptionalValue<T> value1, OptionalValue<T> value2)
            => value1.Equals(value2);

        /// <summary>
        /// Returns <c>true</c> if its operands are not equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// The result of the comparision.
        /// </returns>
        public static bool operator !=(OptionalValue<T> value1, OptionalValue<T> value2)
            => !(value1 == value2);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
            => HashUtilities.Combine(HasValue.GetHashCode(), Value?.GetHashCode() ?? 0);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
            => obj is OptionalValue<T> optional ? Equals(optional) : false;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(OptionalValue<T> other)
            => HasValue == other.HasValue
            && EqualityComparer<T>.Default.Equals(Value, other.Value);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
            => $"{Value}";

        public OptionalValue<T> ToOptionalValue(T value)
        {
            return new OptionalValue<T>(value);
        }
    }
}
