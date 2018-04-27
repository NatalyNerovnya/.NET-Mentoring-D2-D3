using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ScanerService.Interfaces;
using QueueClient;

namespace ScanerService
{
    public class PdfFileService: IFileService
    {
        private readonly string _successFolder;
        private PdfDocument _document;
        private int _counter;
        private bool _isFileCreated;
        private readonly AzureQueueClient _queueClient;

        public PdfFileService(string successFolder)
        {
            _isFileCreated = false;
            _successFolder = successFolder;
            _counter = 1;

            _queueClient = new AzureQueueClient();
        }
        
        public void AddPage(string filePath)
        {
            if (!_isFileCreated) CreateDocument();

            var page = new PdfPage();
            _document.Pages.Add(page);

            using (var image = XImage.FromFile(filePath))
            {
                using (var gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawImage(image, 0, 0);
                }
            }
        }

        public void SaveDocument()
        {
            if (!_isFileCreated) return;

            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                _document.Save(stream, true);
                fileContents = stream.ToArray();
            }
            
            _queueClient.SendBytes(fileContents);

            _document.Close();

            _isFileCreated = false;
        }

        private void CreateDocument()
        {
            if (_isFileCreated) return;

            _document = new PdfDocument();

            _isFileCreated = true;
        }
    }
}
