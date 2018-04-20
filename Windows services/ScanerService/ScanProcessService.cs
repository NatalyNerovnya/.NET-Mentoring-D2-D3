using System.Collections.Generic;
using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.IO;
using System.Linq;
using ScanerService.Rules;
using Topshelf;
using Configuration = ScanerService.Helpers.Configuration;

namespace ScanerService
{
    public class ScanProcessService : ServiceControl
    {
        private readonly Configuration _configuration;
        private FileSystemWatcher _watcher;
        private readonly IDirectoryService _directoryService;
        private readonly IFileProcessor _fileProcessor;
        private readonly List<IInteruptRule> _rules;

        public ScanProcessService(Configuration config)
        {
            _configuration = config;

            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor(_configuration.SuccessFolder, _configuration.ErrorFolder, _configuration.ProcessingFolder, _configuration.FileNamePattern);
            _rules = new List<IInteruptRule>
            {
                new TimerRule(_configuration.TimerValue),
                new BarcodeRule(_configuration.BarcodeString),
                new NameRule(_configuration.FileNamePattern)
            };
        }

        public bool Start(HostControl hostControl)
        {
            var path = _configuration.Folder;

            _directoryService.CreateDirectory(_configuration.SuccessFolder);
            _directoryService.CreateDirectory(_configuration.ErrorFolder);
            _directoryService.CreateDirectory(_configuration.ProcessingFolder);
            _directoryService.CreateDirectory(path);

            _fileProcessor.ProcessWaitingFiles(path, _rules.Where(r => r.GetType() != typeof(TimerRule)).ToList());
            
            InitializeWatcher(path);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _watcher.EnableRaisingEvents = false;
            
            return true;
        }


        private void InitializeWatcher(string path)
        {
            _watcher = _directoryService.GetFileSystemWatcher(path);

            _watcher.Changed += HandleFile;
            _watcher.Created += HandleFile;

            _watcher.EnableRaisingEvents = true;
        }

        private void HandleFile(object sender, FileSystemEventArgs args)
        {
            var filePath = args.FullPath;

            _fileProcessor.ProcessFiles(filePath, _rules);
        }
    }
}
