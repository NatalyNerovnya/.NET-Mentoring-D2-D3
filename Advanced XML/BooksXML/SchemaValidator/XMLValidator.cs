using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace SchemaValidator
{
    public class XMLValidator
    {
        private XmlReaderSettings settings;
        private StringBuilder errorMessage;
        private string xsdSchemaPath;
        private string xsdSchemaNamespace;

        public XMLValidator(string schemaPath, string schemaNamespace)
        {
            xsdSchemaPath = schemaPath;
            xsdSchemaNamespace = schemaNamespace;
        }
        public string Validate(string xmlPath)
        {
            if (string.IsNullOrEmpty(xmlPath))
            {
                throw new ArgumentNullException($"Aggument {nameof(xmlPath)} should contain correct path");
            }

            errorMessage = new StringBuilder();

            settings = new XmlReaderSettings();
            settings.Schemas.Add(xsdSchemaNamespace, xsdSchemaPath);
            settings.ValidationEventHandler += HandleError;
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationType = ValidationType.Schema;

            CheckAllDocument(xmlPath);

            if (errorMessage.Length == 0)
            {
                errorMessage.Append("XML is valid");
            }

            return errorMessage.ToString();
        }

        private string CreateErrorMessage(string tagName, ValidationEventArgs e)
        {
            string message = string.Empty;

            switch (tagName)
            {
                case Tags.Genre:
                    message = "Incorrect genre. ";
                    break;

                case Tags.Isbn:
                    message = "Invalid isbn. ";
                    break;

                case Tags.Registration_date:
                    message = "Invalid registration date format. ";
                    break;

                case Tags.Publish_date:
                    message = "Invalid publish date format. ";
                    break;

                case Tags.Book:

                    message = "Id isn't unique. ";
                    break;
            }

            return $"{message}[{e.Exception.LineNumber}:{e.Exception.LinePosition}] {e.Message}";
        }

        private void CheckAllDocument(string xmlPath)
        {
            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                while (reader.Read()) {}
            }
        }

        private void HandleError(object sender, ValidationEventArgs e)
        {
            if (sender is XmlReader element)
            {
                errorMessage.AppendLine(CreateErrorMessage(element.Name, e));
            }
        }
    }
}
