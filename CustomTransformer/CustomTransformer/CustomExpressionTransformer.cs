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
            if (expression.NodeType == ExpressionType.Add)
            {
                return Expression.Increment(parameter);
            }
            else //if (expression.NodeType == ExpressionType.Subtract)
            {
                return Expression.Decrement(parameter);
            }
        }
    }
}
