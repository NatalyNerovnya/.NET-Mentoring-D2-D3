using System;
using System.Linq.Expressions;
using CustomTransformer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomExpressionTransformerTests
{
    [TestClass]
    public class CustomTransformerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Expression<Func<int, int, int, double>> someExpr = (x, y, z) => (x + y + z) / 3.0;
            var myVisitor = new MyExpressionVisitor();

            // visit the expression's Body instead
            myVisitor.Visit(someExpr.Body);
        }
    }
}
