using SchemaValidator;
using System;
using System.Configuration;
using System.IO;

namespace BookValidationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var schemaPath = ConfigurationManager.AppSettings["xsdSchemaPath"];
            var schemaNamespace = ConfigurationManager.AppSettings["xsdSchemaNamespace"];
            var validator = new XMLValidator(schemaPath, schemaNamespace);

            string filePath;
            string consoleAnswer;

            do
            {
                do
                {
                    Console.WriteLine("Please, enter correct full path to xml file:");
                    filePath = Console.ReadLine();
                } while (!File.Exists(filePath));

                Console.WriteLine(validator.Validate(filePath));

                Console.WriteLine("Do you want to repeat?[y,n]");
                consoleAnswer = Console.ReadLine();

            } while (consoleAnswer == "y");
        }
    }
}
