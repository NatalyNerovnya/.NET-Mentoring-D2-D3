using System;
using System.Linq.Expressions;
using CustomTransformer;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Incrementing");
            Expression<Func<int, int>> incrementExpression = (x) => x + 1;
            Console.WriteLine(incrementExpression);
            
            CustomExpressionTransformer transformer = new CustomExpressionTransformer();
            var transformatedExpression = transformer.Visit(incrementExpression.Body);
            Console.WriteLine(transformatedExpression);


            Console.WriteLine("Decrementing");
            Expression<Func<int, int>> decrementExpression = (u) => u - 1;
            Console.WriteLine(decrementExpression);

            transformatedExpression = transformer.Visit(decrementExpression.Body);
            Console.WriteLine(transformatedExpression);

            Console.ReadKey();
        }
    }
}
