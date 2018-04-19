using System.Drawing;
using System.IO;
using ScanerService.Interfaces;
using ZXing;

namespace ScanerService.Rules
{
    public class BarcodeRule: IInteruptRule
    {
        private readonly string _secretValue;

        public BarcodeRule(string secretValue)
        {
            _secretValue = secretValue;
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
                    RemoveFile(file);
                    return result.Text == _secretValue;
                }

                return false;
            }
        }

        private void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
