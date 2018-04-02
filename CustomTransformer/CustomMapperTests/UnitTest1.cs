using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CustomMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace CustomMapperTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var class1Instance = new Class1()
            {
                Prop1 = 5.3,
                Prop2 = "Nataly",
                Prop3 = new List<int>() { 13, 1261, 33 }
            };

            var mapper = new ObjectsMapper();
            var class2Instance = mapper.Map<Class2>(class1Instance);

            Console.WriteLine(class1Instance);
            Console.WriteLine(class2Instance);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var class3Instance = new Class3()
            {
                Prop1 = 500,
                Prop2 = ".NET",
                Prop3 = new List<double>() { 1.2, 3.4, 5.6, 7.8 },
                Prop4 = 12.13
            };

            var mapper = new ObjectsMapper();
            var class2Instance = mapper.Map<Class2>(class3Instance);

            Console.WriteLine(class3Instance);
            Console.WriteLine(class2Instance);
        }

    }
}
