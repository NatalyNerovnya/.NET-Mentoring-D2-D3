using System;
using System.Linq.Expressions;
using CustomTransformer;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int>> incrementExpression = (x) => x + 1;
            Console.WriteLine(incrementExpression);

            CustomExpressionTransformer transformer = new CustomExpressionTransformer();
            Console.WriteLine(transformer.Visit(incrementExpression));
        }
    }
}
