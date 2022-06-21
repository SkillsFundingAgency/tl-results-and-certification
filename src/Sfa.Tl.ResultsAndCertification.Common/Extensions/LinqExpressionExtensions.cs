using System;
using System.Linq.Expressions;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public class LinqExpressionExtensions
    {
        public static Expression<Func<T, bool>> OrCombine<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null && right == null) throw new ArgumentException("At least one argument must not be null");
            if (left == null) return right;
            if (right == null) return left;

            var parameter = Expression.Parameter(typeof(T), "p");
            var combined = new ParameterReplacer(parameter).Visit(Expression.OrElse(left.Body, right.Body));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }

    class ParameterReplacer : ExpressionVisitor
    {
        readonly ParameterExpression parameter;

        internal ParameterReplacer(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return parameter;
        }
    }
}
