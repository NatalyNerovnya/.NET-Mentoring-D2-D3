using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Topshelf;

namespace ScanerService
{
    public class ScanProcessService : ServiceControl
    {
        private List<FileSystemWatcher> watchers;
        private IDirectoryService _directoryService;
        private IFileProcessor _fileProcessor;

        public ScanProcessService()
        {
            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor();
            watchers = new List<FileSystemWatcher>();
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
                watcher.Error += HandleError;

                watcher.EnableRaisingEvents = true;
            }
        }

        private void HandleFile(object sender, FileSystemEventArgs args)
        {
            var files = Directory.GetFiles(Path.GetDirectoryName(args.FullPath));

            if (files.Length == 2)
            {
                _fileProcessor.Process(files);

                var successFolder = ConfigurationManager.AppSettings["SuccessFolder"];
                foreach (var file in files)
                {
                    _directoryService.MoveFile(file, successFolder);
                }
            }

            
        }

        private void HandleError(object sender, ErrorEventArgs args)
        {
            //TODO: log error
        }
    }
}
