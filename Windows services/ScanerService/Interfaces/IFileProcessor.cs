namespace ScanerService.Interfaces
{
    public interface IFileProcessor
    {
        void Process(string[] files, string destinitionFolder);
    }
}
