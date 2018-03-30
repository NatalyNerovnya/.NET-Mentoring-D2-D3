using System;
using System.Linq.Expressions;

namespace CustomTransformer
{
    public class CustomExpressionTransformer : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression expression)
        {
            var rightParameter = expression.Right;
            if (rightParameter.NodeType != ExpressionType.Constant)
            {
                return expression;
            }
            var parameter = expression.Left;

            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    return Expression.Increment(parameter);
                case ExpressionType.Subtract:
                    return Expression.Decrement(parameter);
            }

            return expression;
        }


    }
}
