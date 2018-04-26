namespace ScanerService.Interfaces
{
    public interface IFileService
    {
        void AddPage(string filePath);
        void SaveDocument();
    }
}
