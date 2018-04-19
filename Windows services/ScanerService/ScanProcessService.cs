using System;
using System.Collections.Generic;
using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

            _rules = new List<IInteruptRule>
            {
                new TimerRule(_configuration.TimerValue),
                new BarcodeRule(_configuration.BarcodeString),
                new NameRule(_configuration.FileNamePattern)
            };
            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor();
        }

        public bool Start(HostControl hostControl)
        {
            ProcessWaitingFiles();

            var path = _configuration.Folder;
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

            //As files should come from scanner their creation time should be around now
            File.SetCreationTime(filePath, DateTime.Now);

            foreach (var rule in _rules)
            {
                if (rule.IsMatch(filePath))
                {
                    var files = Directory.GetFiles(Path.GetDirectoryName(filePath) ?? "");
                    var filesToProccess = files.ToList().Where(x => x != filePath).ToArray();

                    ProcessFiles(filesToProccess);
                }
            }
        }

        private void ProcessFiles(string[] files)
        {
            var successFolder = _configuration.SuccessFolder;
            var errorFolder = _configuration.ErrorFolder;

            _directoryService.CreateDirectory(successFolder);
            _directoryService.CreateDirectory(errorFolder);

            _fileProcessor.Process(files.Where(CheckImageName).ToArray(), successFolder);

            foreach (var file in files)
            {
                _directoryService.MoveFile(file, CheckImageName(file) ? successFolder : errorFolder);
            }
        }

        private void ProcessWaitingFiles()
        {
            //var watchedFolder = _configuration.Folder;

            //if (!Directory.Exists(watchedFolder)) return;

            //var files = Directory.EnumerateFiles(watchedFolder)
            //    .Where(x => CheckImageName(Path.GetFileName(x))).ToList();
            //var fileNumber = 0;
            //var filesInFolder = new List<string>();

            //files.ForEach(x => filesInFolder.Add(x));

            //foreach (var file in files)
            //{
            //    if (fileNumber > 0 && GetImageNumber(file) != fileNumber + 1)
            //    {
            //        var filesToFile = filesInFolder.TakeWhile(x => x != file).Where(CheckImageName).ToArray();

            //        ProcessFiles(filesToFile);

            //        filesToFile.ToList().ForEach(f => filesInFolder.Remove(f));
            //    }

            //    fileNumber = GetImageNumber(file);
            //}
        }

        private bool CheckImageName(string imageName)
        {
            return Regex.IsMatch(imageName, _configuration.FileNamePattern);
        }
    }
}
