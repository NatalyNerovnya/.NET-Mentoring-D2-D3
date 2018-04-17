using System.Collections.Generic;
using System.IO;

namespace ScanerService.Interafces
{
    public interface IDirectoryService
    {
        List<FileSystemWatcher> GetFileSystemWatchers(string[] paths);

        void MoveFile(string filePath, string destenitionPath);
    }
}
