namespace ScanerService.Interfaces
{
    public interface IInteruptRule
    {
        bool IsMatch(string file);
    }
}
