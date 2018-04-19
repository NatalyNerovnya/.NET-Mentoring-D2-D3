﻿using System.Collections.Generic;
using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Topshelf;
using ZXing;
using Configuration = ScanerService.Helpers.Configuration;

namespace ScanerService
{
    public class ScanProcessService : ServiceControl
    {
        private readonly Configuration _configuration;
        private FileSystemWatcher _watcher;
        private readonly IDirectoryService _directoryService;
        private readonly IFileProcessor _fileProcessor;
        private int _currentFileNumber;
        private Timer _timer;

        public ScanProcessService(Configuration config)
        {
            _configuration = config;
            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor();
            _currentFileNumber = 0;
        }

        public bool Start(HostControl hostControl)
        {
            ProcessWaitingFiles();

            var path = _configuration.Folder;
            InitializeWatcher(path);
            InitializeTimer(path);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _watcher.EnableRaisingEvents = false;

            _timer.Stop();

            return true;
        }


        private void InitializeWatcher(string path)
        {
            _watcher = _directoryService.GetFileSystemWatcher(path);

            _watcher.Changed += HandleFile;
            _watcher.Created += HandleFile;

            _watcher.EnableRaisingEvents = true;
        }

        private void InitializeTimer(string path)
        {
            var timerTime = _configuration.TimerValue;
            _timer = new Timer()
            {
                Interval = timerTime,
                AutoReset = true
            };

            _timer.Elapsed += (sender, args) =>
            {
                _timer.Stop();
                var files = Directory.GetFiles(path);
                ProcessFiles(files);
                _timer.Start();
            };

            _timer.Start();
        }

        private void HandleFile(object sender, FileSystemEventArgs args)
        {
            _timer.Stop();

            var fileName = args.Name;
            var filePath = args.FullPath;
            var isBarcode = CheckCode(filePath);

            if (!CheckImageName(fileName) && !isBarcode)
            {
                HandleError(filePath);
            }           

            if (isBarcode || (_currentFileNumber > 0 && GetImageNumber(fileName) != _currentFileNumber + 1))
            {
                var files = Directory.GetFiles(Path.GetDirectoryName(filePath) ?? "");
                var filesToProccess = files.ToList().Where(x => x != filePath).ToArray();

                ProcessFiles(filesToProccess);

                if (isBarcode)
                {
                    _directoryService.RemoveFile(filePath);
                }
            }

            if (!isBarcode)
            {
                _currentFileNumber = GetImageNumber(fileName);
            }

            _timer.Start();
        }

        private void HandleError(string path)
        {
            var errorFolder = _configuration.ErrorFolder;

            _directoryService.MoveFile(path, errorFolder);
        }

        private void ProcessFiles(string[] files)
        {           
            var successFolder = _configuration.SuccessFolder;

            _directoryService.CreateDirectory(successFolder);

            _fileProcessor.Process(files.Where(CheckImageName).ToArray(), successFolder);

            foreach (var file in files)
            {
                _directoryService.MoveFile(file, successFolder);
            }
        }

        private void ProcessWaitingFiles()
        {
            var watchedFolder = _configuration.Folder;

            if (!Directory.Exists(watchedFolder)) return;

            var files = Directory.EnumerateFiles(watchedFolder)
                .Where(x => CheckImageName(Path.GetFileName(x)) || CheckCode(x)).ToList();
            var fileNumber = 0;
            var filesInFolder = new List<string>();

            files.ForEach(x => filesInFolder.Add(x));

            foreach (var file in files)
            {
                var isBarcode = CheckCode(file);
                if (isBarcode || (fileNumber > 0 && GetImageNumber(file) != fileNumber + 1))
                {
                    var filesToFile = filesInFolder.TakeWhile(x => x != file).Where(CheckImageName).ToArray();

                    ProcessFiles(filesToFile);

                    filesToFile.ToList().ForEach(f => filesInFolder.Remove(f));

                    if (isBarcode)
                    {
                        _directoryService.RemoveFile(file);
                    }
                }

                if (!isBarcode)
                {
                    fileNumber = GetImageNumber(file);
                }
            }
        }

        private bool CheckCode(string file)
        {
            var reader = new BarcodeReader();

            using (var barcodeBitmap = (Bitmap)Image.FromFile(file))
            {
                var result = reader.Decode(barcodeBitmap);

                if (result != null)
                    return result.Text == _configuration.BarcodeString;
                return false;
            }
            
        }

        private bool CheckImageName(string imageName)
        {
            return Regex.IsMatch(imageName, _configuration.FileNamePattern);
        }

        private int GetImageNumber(string imageName)
        {
            return int.Parse(Regex.Match(imageName, @"\d+").Value);
        }        
    }
}
