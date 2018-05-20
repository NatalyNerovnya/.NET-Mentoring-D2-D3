using System.Collections.Generic;

namespace ScanerService.Interfaces
{
    public interface IFileProcessor
    {
        void ProcessFiles(string filePath, List<IInteruptRule> rules);

        void ProcessWaitingFiles(string watchedFolder, List<IInteruptRule> rules);
    }
}
