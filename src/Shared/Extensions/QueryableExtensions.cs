using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Queryable Extensions
    /// </summary>
    internal static class QueryableExtensions
    {
        /// <summary>
        /// Applies the specified collection query <paramref name="options" />. It bypasses and returns a specified number
        /// of contiguous elements from the start of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="query">The sequence to return elements from.</param>
        /// <param name="options">A set of key/value pairs that configures query options for pageable collections.</param>
        /// <returns>
        /// The sequence.
        /// </returns>
        internal static IQueryable<TSource> CollectionQuery<TSource>(this IQueryable<TSource> query, CollectionQueryOptions options)
        {
            if (options.Offset.HasValue)
            {
                query = query.Skip(options.Offset.Value);
            }

            if (options.Limit.HasValue)
            {
                query = query.Take(options.Limit.Value);
            }

            return query;
        }

        /// <summary>
        /// Wheres the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TVal">The type of the value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        internal static IQueryable<TSource> Where<TSource, TVal>(this IQueryable<TSource> source, Expression<Func<TSource, TVal>> predicate, IEnumerable<TVal> values)
        {
            return source.Where(GetWhereExpression(predicate, values));
        }

        /// <summary>
        /// Gets the where expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        internal static Expression<Func<T, bool>> GetWhereExpression<T, TValue>(this Expression<Func<T, TValue>> selector, IEnumerable<TValue> values)
        {
            Expression? result = null;

            foreach (var value in values)
            {
                var match = Expression.Equal(
                    selector.Body,
                    Expression.Constant(value, typeof(TValue)));

                result = result is null ? match : Expression.OrElse(result, match);
            }

            if (result is null)
            {
                return x => true; 
            }

            return Expression.Lambda<Func<T, bool>>(result, selector.Parameters);
        }
    }
}
