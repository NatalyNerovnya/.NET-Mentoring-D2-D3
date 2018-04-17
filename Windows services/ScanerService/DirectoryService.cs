using System.IO;
using ScanerService.Interafces;
using System.Threading;

namespace ScanerService
{
    public class DirectoryService : IDirectoryService
    {
        public FileSystemWatcher GetFileSystemWatcher(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var watcher = new FileSystemWatcher()
            {
                Filter = "*.jpg",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Path = path
            };

            return watcher;
        }

        public void MoveFile(string filePath, string destenitionPath)
        {
            if (!File.Exists(filePath)) return;

            if (!Directory.Exists(destenitionPath))
            {
                Directory.CreateDirectory(destenitionPath);
            }

            var destinitionFile = Path.Combine(destenitionPath, Path.GetFileName(filePath));

            if(TryOpen(filePath, 3))
            {
                File.Move(filePath, destinitionFile);
            }           

        }

        public void RemoveFile(string path)
        {
            if (File.Exists(path) && TryOpen(path, 3))
            {
                File.Delete(path);
            }
        }

        private bool TryOpen(string fileName, int tryCount)
        {
            for (int i = 0; i < tryCount; i++)
            {
                try
                {
                    var file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    file.Close();

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(5000);
                }
            }

            return false;
        }

    }
}
