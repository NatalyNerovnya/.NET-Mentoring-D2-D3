using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ScanerService.Interfaces;

namespace ScanerService
{
    public class PdfFileService: IFileService
    {
        private readonly string _successFolder;
        private PdfDocument _document;
        private int _counter;
        private bool _isFileCreated;

        public PdfFileService(string successFolder)
        {
            _isFileCreated = false;
            _successFolder = successFolder;
            _counter = 1;
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

            _document.Save(Path.Combine(_successFolder, $"scan_{_counter++}.pdf"));
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
