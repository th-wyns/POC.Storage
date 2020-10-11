using System;
using System.Linq;
using System.Linq.Expressions;

namespace POC.Storage
{
    /// <summary>
    /// Provides methods for rewriting expression trees.
    /// </summary>
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Merges the member initialization list of two lambda expressions into one.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TBaseDest">Resulting type of the base mapping expression. TBaseDest is typically a super class of TExtendedDest</typeparam>
        /// <typeparam name="TExtendedDest">Resulting type of the extended mapping expression.</typeparam>
        /// <param name="baseExpression">The base mapping expression, containing a member initialization expression.</param>
        /// <param name="mergeExpression">The extended mapping expression to be merged into the base member initialization expression.</param>
        /// <returns>
        /// Resulting expression, after the merged select expression has been applied.
        /// </returns>
        internal static Expression<Func<TSource, TExtendedDest>> Merge<TSource, TBaseDest, TExtendedDest>(
            this Expression<Func<TSource, TBaseDest>> baseExpression,
            Expression<Func<TSource, TExtendedDest>> mergeExpression)
        {
            //
            //  Expression<Func<Project, ProjectDto>> selector = project => new ProjectDto { Name = project.Name };
            //
            //  if (fields.HasFlag(ProjectField.Owner))
            //  {
            //      selector = selector.Merge(project => new ProjectDto { Owner = project.Owner });
            //  }
            //

            return (Expression<Func<TSource, TExtendedDest>>)
                new MergingVisitor<TSource, TBaseDest, TExtendedDest>(baseExpression).Visit(mergeExpression);
        }

        /// <summary>
        /// The merging visitor doing the actual merging work.
        /// </summary>
        /// <typeparam name="TSource">Source data type.</typeparam>
        /// <typeparam name="TBaseDest">Resulting type of the base query.</typeparam>
        /// <typeparam name="TExtendedDest">Resulting type of the merged expression.</typeparam>
        /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
        private sealed class MergingVisitor<TSource, TBaseDest, TExtendedDest> : ExpressionVisitor
        {
            /// <summary>
            /// Internal helper, that rebinds the lambda of the base init expression. The
            /// reason for this is that the member initialization list of the base expression
            /// is bound to the range variable in the base expression. To be able to merge those
            /// into the extended expression, all those references have to be rebound to the
            /// range variable of the extended expression. That rebinding is done by this helper.
            /// </summary>
            private sealed class LambdaRebindingVisitor : ExpressionVisitor
            {
                private readonly ParameterExpression _newParameter;
                private readonly ParameterExpression _oldParameter;

                /// <summary>
                /// Initializes a new instance of the <see cref="LambdaRebindingVisitor" /> class.
                /// </summary>
                /// <param name="newParameter">The range vaiable of the extended expression.</param>
                /// <param name="oldParameter">The range variable of the base expression.</param>
                public LambdaRebindingVisitor(ParameterExpression newParameter, ParameterExpression oldParameter)
                {
                    _newParameter = newParameter;
                    _oldParameter = oldParameter;
                }

                /// <summary>
                /// Visits the children of the <see cref="T:System.Linq.Expressions.MemberExpression" />.
                /// </summary>
                /// <param name="node">The expression to visit.</param>
                /// <returns>
                /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
                /// </returns>
                protected override Expression VisitMember(MemberExpression node)
                {
                    if (node.Expression == _oldParameter)
                    {
                        return Expression.MakeMemberAccess(_newParameter, node.Member);
                    }

                    return base.VisitMember(node);
                }
            }

            private readonly MemberInitExpression _baseInit;
            private readonly ParameterExpression _baseParameter;
            private ParameterExpression? _newParameter;

            /// <summary>
            /// Initializes a new instance of the <see cref="MergingVisitor{TSource, TBaseDest, TExtendedDest}" /> class.
            /// </summary>
            /// <param name="baseExpression">The base expression to merge into the member init list of the extended expression.</param>
            public MergingVisitor(Expression<Func<TSource, TBaseDest>> baseExpression)
            {
                LambdaExpression lambda = baseExpression;
                _baseInit = (MemberInitExpression)lambda.Body;
                _baseParameter = lambda.Parameters[0];
            }

            /// <summary>
            /// Visits the children of the <see cref="T:System.Linq.Expressions.Expression`1" />.
            /// </summary>
            /// <typeparam name="T">The type of the delegate.</typeparam>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                if (_newParameter == null)
                {
                    _newParameter = node.Parameters[0];
                }

                return base.VisitLambda<T>(node);
            }

            /// <summary>
            /// Visits the children of the <see cref="T:System.Linq.Expressions.MemberInitExpression" />.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                var rebindingVisitor = new LambdaRebindingVisitor(_newParameter!, _baseParameter);
                var baseInitExpression = (MemberInitExpression)rebindingVisitor.Visit(_baseInit);
                var mergedInitListBindings = node.Bindings.Concat(baseInitExpression.Bindings);
                return Expression.MemberInit(Expression.New(typeof(TExtendedDest)), mergedInitListBindings);
            }
        }
    }
}
