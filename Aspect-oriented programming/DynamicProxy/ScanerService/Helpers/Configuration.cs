
namespace ScanerService.Helpers
{
    public class Configuration
    {
        public Configuration(string folder, string successFolder, string errorFolder, string processingFolder, string fileNamePattern, int timerValue, string barcodeString)
        {
            Folder = folder;
            SuccessFolder = successFolder;
            ErrorFolder = errorFolder;
            ProcessingFolder = processingFolder;
            FileNamePattern = fileNamePattern;
            TimerValue = timerValue;
            BarcodeString = barcodeString;
        }

        public string Folder { get; set; }
        public string SuccessFolder { get; set; }
        public string ErrorFolder { get; set; }
        public string ProcessingFolder { get; set; }
        public string FileNamePattern { get; set; }
        public int TimerValue { get; set; }
        public string BarcodeString { get; set; }
    }
}
