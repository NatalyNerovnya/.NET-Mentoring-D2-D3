using SchemaValidator;
using System;
using System.Configuration;

namespace BookValidationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var schemaPath = ConfigurationManager.AppSettings["xsdSchemaPath"];
            var schemaNamespace = ConfigurationManager.AppSettings["xsdSchemaNamespace"];
            var validator = new XMLValidator(schemaPath, schemaNamespace);
            var validFilePath = ConfigurationManager.AppSettings["validFile"];
            var invalidFilePath = ConfigurationManager.AppSettings["invalidFile"];

            Console.WriteLine("Valid file output\n");
            Console.WriteLine(validator.Validate(validFilePath));

            Console.WriteLine("\n \n-------------------------------------------------------------------------------------\n \n");

            Console.WriteLine("Invalid file output\n");
            Console.WriteLine(validator.Validate(invalidFilePath));

            Console.ReadKey();
        }
    }
}
