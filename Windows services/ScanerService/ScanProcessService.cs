using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Topshelf;

namespace ScanerService
{
    public class ScanProcessService : ServiceControl
    {
        private List<FileSystemWatcher> watchers;
        private IDirectoryService _directoryService;
        private IFileProcessor _fileProcessor;
        private string _imageNamePattern;
        private int currentFileNumber;

        public ScanProcessService()
        {
            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor();
            watchers = new List<FileSystemWatcher>();
            _imageNamePattern = ConfigurationManager.AppSettings["FileNamePattern"];
            currentFileNumber = 0;
        }

        public bool Start(HostControl hostControl)
        {
            //TODO: process already existed files 
            InitializeWatchers();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            foreach (var watcher in watchers)
            {
                watcher.EnableRaisingEvents = false;
            }

            return true;
        }


        private void InitializeWatchers()
        {
            var paths = ConfigurationManager.AppSettings["Folders"].Split(';');

            watchers.AddRange(_directoryService.GetFileSystemWatchers(paths));
            foreach (var watcher in watchers)
            {
                watcher.Changed += HandleFile;
                watcher.Created += HandleFile;

                watcher.EnableRaisingEvents = true;
            }
        }

        private void HandleFile(object sender, FileSystemEventArgs args)
        {
            var fileName = args.Name;

            if (!CheckImageName(fileName))
            {
                HandleError(args.FullPath);
            }        
            
            if (currentFileNumber > 0 && GetImageNumber(fileName) != currentFileNumber + 1)
            {
                var files = Directory.GetFiles(Path.GetDirectoryName(args.FullPath));
                var successFolder = ConfigurationManager.AppSettings["SuccessFolder"];

                var filesToProccess = files.ToList().Where(x => x != args.FullPath).ToArray();
                _fileProcessor.Process(filesToProccess);
                
                foreach (var file in filesToProccess)
                {
                    _directoryService.MoveFile(file, successFolder);
                }
            }

            currentFileNumber = GetImageNumber(fileName);            
        }

        private void HandleError(string path)
        {
            var errorFolder = ConfigurationManager.AppSettings["ErrorFolder"];

            _directoryService.MoveFile(path, errorFolder);
        }

        private bool CheckImageName(string imageName)
        {
            return Regex.IsMatch(imageName, _imageNamePattern);
        }

        private int GetImageNumber(string imageName)
        {
            return int.Parse(Regex.Match(imageName, @"\d+").Value);
        }


    }
}
