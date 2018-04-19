using System.IO;
using System.Text.RegularExpressions;
using ScanerService.Interfaces;

namespace ScanerService.Rules
{
    public class NameRule : IInteruptRule
    {
        private readonly string _fileNamePattern;
        private int _curentFileNumber;
        public NameRule(string fileNamePattern)
        {
            _fileNamePattern = fileNamePattern;
            _curentFileNumber = 0;
        }

        public bool IsMatch(string file)
        {
            var fileName = Path.GetFileName(file);

            if (!CheckImageName(fileName)) return false;

            var fileNumber = GetImageNumber(Path.GetFileName(file));
            var result = _curentFileNumber > 0 && fileNumber != _curentFileNumber + 1;
            _curentFileNumber = fileNumber;

            return result;
        }

        private bool CheckImageName(string imageName)
        {
            return Regex.IsMatch(imageName, _fileNamePattern);
        }

        private int GetImageNumber(string imageName)
        {
            return int.Parse(Regex.Match(imageName, @"\d+").Value);
        }
    }
}
