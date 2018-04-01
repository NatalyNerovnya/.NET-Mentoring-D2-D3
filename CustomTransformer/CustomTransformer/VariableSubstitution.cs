using System.Collections.Generic;
using System.Linq.Expressions;

namespace CustomTransformer
{
    public class VariableSubstitution<T>: ExpressionVisitor
    {
        public Dictionary<string, T> ValuesDictionary { get; set; }

        public VariableSubstitution(Dictionary<string, T> valuesDictionary)
        {
            ValuesDictionary = valuesDictionary;
        }

        protected override Expression VisitLambda<TU>(Expression<TU> expression)
        {
            if (expression.NodeType == ExpressionType.Parameter)
            {
                VisitParameter((ParameterExpression)expression.Body);
            }
            
            return Expression.Lambda(Visit(expression.Body), expression.Parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ValuesDictionary.ContainsKey(node.Name) ? Expression.Constant(ValuesDictionary[node.Name]) : base.VisitParameter(node);
        }


    }
}
