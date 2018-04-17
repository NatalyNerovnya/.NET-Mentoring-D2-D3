using System.Collections.Generic;
using System.IO;

namespace ScanerService.Interafces
{
    public interface IDirectoryService
    {
        FileSystemWatcher GetFileSystemWatcher(string path);

        void MoveFile(string filePath, string destenitionPath);

        void RemoveFile(string path);
    }
}
