using ScanerService.Interfaces;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace ScanerService
{
    public class FileProcessor : IFileProcessor
    {
        private static int _counter = 1;
        public void Process(string[] files, string destinitionFolder)
        {
            if (files.Length == 0) return;
            
            using (var document = new PdfDocument())
            {
                for (var i = 0; i < files.Length; i++)
                {
                    document.Pages.Add(new PdfPage());

                    var page = document.Pages[i];

                    using (var image = XImage.FromFile(files[i]))
                    {
                        using (var gfx = XGraphics.FromPdfPage(page))
                        {
                            gfx.DrawImage(image, 0, 0);
                        }
                    }
                }

                document.Save(Path.Combine(destinitionFolder, $"scan_{_counter++}.pdf"));
            }
        }
    }
}
