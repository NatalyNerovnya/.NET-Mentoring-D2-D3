using System.Drawing;
using System.IO;
using ScanerService.Interafces;
using ScanerService.Interfaces;
using ZXing;

namespace ScanerService.Rules
{
    public class BarcodeRule: IInteruptRule
    {
        private readonly string _secretValue;
        private readonly IDirectoryService _directoryService;

        public BarcodeRule(string secretValue)
        {
            _secretValue = secretValue;
            _directoryService = new DirectoryService();
        }

        public bool IsMatch(string file)
        {
            var reader = new BarcodeReader();

            using (var barcodeBitmap = (Bitmap)Image.FromFile(file))
            {
                var result = reader.Decode(barcodeBitmap);

                if (result != null)
                {
                    barcodeBitmap.Dispose();
                    _directoryService.RemoveFile(file);
                    return result.Text == _secretValue;
                }

                return false;
            }
        }
    }
}
