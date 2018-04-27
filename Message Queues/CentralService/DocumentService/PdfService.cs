using System.IO;

namespace DocumentService
{
    public class PdfService
    {
        private readonly string successFolder = @"C:\Temp\Success";
        private int _counter;

        public PdfService()
        {
            if (!Directory.Exists(successFolder))
            {
                Directory.CreateDirectory(successFolder);
            }

            _counter = 1;
        }

        public void SaveDocument(Stream messageBody)
        {
            var outFile = Path.Combine(successFolder, $"scan_{_counter++}.pdf");

            using (var output = new FileStream(outFile, FileMode.Create))
            {
                messageBody.CopyTo(output);
            }    
        }
    }
}
