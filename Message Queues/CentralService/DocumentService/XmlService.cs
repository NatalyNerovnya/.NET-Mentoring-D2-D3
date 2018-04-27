using System;
using System.IO;

namespace DocumentService
{
    public class XmlService
    {
        private readonly string folder = @"C:\Temp\Status";
        private int _counter;

        public XmlService()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            _counter = 1;
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
