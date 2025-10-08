using System;
using System.Collections.Generic;
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

        class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;
            private readonly HashSet<ParameterExpression> _parameterMap;

            internal ParameterReplacer(ParameterExpression parameter)
            {
                _parameter = parameter;
                _parameterMap = new HashSet<ParameterExpression>();
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_parameterMap.Contains(node))
                    return node;

                if (node.Type == _parameter.Type)
                {
                    return _parameter;
                }

                return node;
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                foreach (var param in node.Parameters)
                {
                    _parameterMap.Add(param);
                }

                var result = base.VisitLambda(node);

                foreach (var param in node.Parameters)
                {
                    _parameterMap.Remove(param);
                }

                return result;
            }
        }
    }
}
