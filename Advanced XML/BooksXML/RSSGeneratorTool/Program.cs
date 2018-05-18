using System;
using System.IO;
using RSSGenerator;
using System.Configuration;

namespace RSSGeneratorTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator();
            var validFilePath = ConfigurationManager.AppSettings["validFile"];
            
            generator.GenerateRss(validFilePath);
            generator.GenerateHtml(validFilePath);

            Console.WriteLine("Created result.xml and result.html are located in C:/temp");
            Console.ReadLine();
        }
    }
}
