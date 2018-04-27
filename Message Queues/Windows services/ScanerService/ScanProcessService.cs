using System.Collections.Generic;
using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.IO;
using System.Linq;
using ScanerService.Rules;
using Topshelf;
using Configuration = ScanerService.Helpers.Configuration;
using ScanerService.Status;
using System.Timers;
using QueueClient;

namespace ScanerService
{
    public class ScanProcessService : ServiceControl
    {
        private readonly Configuration _configuration;
        private FileSystemWatcher _watcher;
        private readonly IDirectoryService _directoryService;
        private readonly IFileProcessor _fileProcessor;
        private readonly List<IInteruptRule> _rules;
        private readonly StatusService statusService;
        private readonly Timer statusTimer;

        public ScanProcessService(Configuration config)
        {
            var queueClient = new AzureQueueClient();
            statusService = new StatusService(config.BarcodeString, config.TimerValue, CurerntState.WatingFiles, queueClient);

            _configuration = config;

            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor(_configuration.SuccessFolder, _configuration.ErrorFolder, _configuration.ProcessingFolder, _configuration.FileNamePattern, queueClient);
            _rules = new List<IInteruptRule>
            {
                new TimerRule(_configuration.TimerValue),
                new BarcodeRule(_configuration.BarcodeString),
                new NameRule(_configuration.FileNamePattern)
            };

            statusTimer = new Timer(config.StatusTimerTime);
            statusTimer.Elapsed += StatusTimer_Elapsed;
        }

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            statusService.SendStatus();
        }

        public bool Start(HostControl hostControl)
        {
            var path = _configuration.Folder;

            _directoryService.CreateDirectory(_configuration.SuccessFolder);
            _directoryService.CreateDirectory(_configuration.ErrorFolder);
            _directoryService.CreateDirectory(_configuration.ProcessingFolder);
            _directoryService.CreateDirectory(path);

            statusTimer.Start();

            _fileProcessor.ProcessWaitingFiles(path, _rules.Where(r => r.GetType() != typeof(TimerRule)).ToList());
            
            InitializeWatcher(path);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _watcher.EnableRaisingEvents = false;
            statusTimer.Stop();

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
            statusService.ServiceStatus.Status = CurerntState.ProcessFiles;

            var filePath = args.FullPath;
            if (_directoryService.TryOpen(filePath, 3))
            {
                _fileProcessor.ProcessFiles(filePath, _rules);
            }

            statusService.ServiceStatus.Status = CurerntState.WatingFiles;
        }
    }
}
