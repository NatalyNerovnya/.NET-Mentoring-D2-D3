using System;
using System.Linq.Expressions;

namespace CustomTransformer
{
    public class CustomExpressionTransformer : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression expression)
        {
            var rightParameter = expression.Right;
            if (rightParameter.NodeType != ExpressionType.Constant || Convert.ToInt32((rightParameter as ConstantExpression)?.Value) != 1)
            {
                Console.WriteLine("Not suitable pattern. Pattern should be <parameter> + 1 / <parameter> + 1");
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
