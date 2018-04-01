using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using CustomTransformer;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Incrementing");
            //Expression<Func<int, int>> incrementExpression = (x) => x + 1;
            //Console.WriteLine(incrementExpression);

            //CustomExpressionTransformer transformer = new CustomExpressionTransformer();
            //var transformatedExpression = transformer.VisitAndConvert(incrementExpression, "");
            //Console.WriteLine(transformatedExpression);

            //Console.WriteLine($"Increment with value = 2 : {transformatedExpression?.Compile().Invoke(2)}");



            //Console.WriteLine("\nDecrementing");
            //Expression<Func<int, int>> decrementExpression = (u) => u - 1;
            //Console.WriteLine(decrementExpression);

            //transformatedExpression = transformer.VisitAndConvert(decrementExpression, "");
            //Console.WriteLine(transformatedExpression);

            //Console.WriteLine($"Decrement with value = 2 : {transformatedExpression?.Compile().Invoke(2)}");

            //Console.ReadKey();
            //Console.Clear();

            Expression<Func<int, int, int, int>> someExpression = (x, y, z) => x + y*z;
            var valueDictionary = new Dictionary<string, int>() { { "x", 1 }, { "y", 2 }, { "z", 3 } };
            
            

            var substituter = new VariableSubstitution<int>(valueDictionary);
            var subsititedExpression = substituter.VisitAndConvert(someExpression, "");

            Console.WriteLine(someExpression);
            Console.WriteLine(subsititedExpression);
            Console.WriteLine(subsititedExpression?.Compile().Invoke(0,0,0));

            Console.ReadKey();
        }
    }
}
