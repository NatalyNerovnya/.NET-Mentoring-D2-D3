﻿using System;
using System.IO;
using RSSGenerator;

namespace RSSGeneratorTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator();

            string filePath;

            do
            {
                Console.WriteLine("Please, enter correct full path to xml file:");
                filePath = Console.ReadLine();
            } while (!File.Exists(filePath));

            generator.Generate(filePath);

            Console.WriteLine("Created result.xml is located in C:/temp");
        }
    }
}
