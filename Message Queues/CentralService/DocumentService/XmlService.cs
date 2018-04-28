using System;
using System.IO;

namespace DocumentService
{
    public class XmlService
    {
        private readonly string folder = @"C:\Temp\Status";

        public XmlService()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public void SaveDocument(Stream messageBody)
        {
            var outFile = Path.Combine(folder, $"status_{DateTime.Now.ToFileTime()}.xml");

            using (var output = new FileStream(outFile, FileMode.Create))
            {
                messageBody.CopyTo(output);
            }
        }
    }
}
