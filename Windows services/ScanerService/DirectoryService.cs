using System;
using System.Collections.Generic;
using System.IO;
using ScanerService.Interafces;
using System.Threading;

namespace ScanerService
{
    public class DirectoryService : IDirectoryService
    {
        public List<FileSystemWatcher> GetFileSystemWatchers(string[] paths)
        {
            var watchers = new List<FileSystemWatcher>();

            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                watchers.Add(new FileSystemWatcher()
                {
                    Filter = "*.jpg",
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Path = path
                });
            }

            return watchers;
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
