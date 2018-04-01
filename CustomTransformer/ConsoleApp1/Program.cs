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
            var transformatedExpression = transformer.VisitAndConvert(incrementExpression, "");
            Console.WriteLine(transformatedExpression);

            Console.WriteLine($"Increment with value = 2 : {transformatedExpression?.Compile().Invoke(2)}");



            Console.WriteLine("Decrementing");
            Expression<Func<int, int>> decrementExpression = (u) => u - 1;
            Console.WriteLine(decrementExpression);

            transformatedExpression = transformer.VisitAndConvert(decrementExpression, "");
            Console.WriteLine(transformatedExpression);

            Console.WriteLine($"Decrement with value = 2 : {transformatedExpression?.Compile().Invoke(2)}");

            Console.ReadKey();
        }
    }
}
