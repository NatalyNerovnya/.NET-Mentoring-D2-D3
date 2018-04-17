using ScanerService.Interafces;
using ScanerService.Interfaces;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Topshelf;

namespace ScanerService
{
    public class ScanProcessService : ServiceControl
    {
        private FileSystemWatcher watcher;
        private IDirectoryService _directoryService;
        private IFileProcessor _fileProcessor;
        private string _imageNamePattern;
        private int currentFileNumber;
        private Timer timer;

        public ScanProcessService()
        {
            _directoryService = new DirectoryService();
            _fileProcessor = new FileProcessor();
            _imageNamePattern = ConfigurationManager.AppSettings["FileNamePattern"];
            currentFileNumber = 0;
        }

        public bool Start(HostControl hostControl)
        {
            //TODO: process already existed files 
            var path = ConfigurationManager.AppSettings["Folders"];
            InitializeWatcher(path);
            InitializeTimer(path);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            watcher.EnableRaisingEvents = false;

            timer.Stop();

            return true;
        }


        private void InitializeWatcher(string path)
        {
            watcher = _directoryService.GetFileSystemWatcher(path);

            watcher.Changed += HandleFile;
            watcher.Created += HandleFile;

            watcher.EnableRaisingEvents = true;
        }

        private void InitializeTimer(string path)
        {
            var timerTime = double.Parse(ConfigurationManager.AppSettings["TimerTime"]);
            timer = new Timer()
            {
                Interval = timerTime,
                AutoReset = true
            };

            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();
                var files = Directory.GetFiles(path);
                ProcessFiles(files);
                timer.Start();
            };

            timer.Start();
        }

        private void HandleFile(object sender, FileSystemEventArgs args)
        {
            var fileName = args.Name;

            if (!CheckImageName(fileName))
            {
                HandleError(args.FullPath);
            }

            timer.Stop();            

            if (currentFileNumber > 0 && GetImageNumber(fileName) != currentFileNumber + 1)
            {
                var files = Directory.GetFiles(Path.GetDirectoryName(args.FullPath));
                var filesToProccess = files.ToList().Where(x => x != args.FullPath).ToArray();

                ProcessFiles(filesToProccess);
            }

            currentFileNumber = GetImageNumber(fileName);
            timer.Start();
        }

        private void HandleError(string path)
        {
            var errorFolder = ConfigurationManager.AppSettings["ErrorFolder"];

            _directoryService.MoveFile(path, errorFolder);
        }

        private void ProcessFiles(string[] files)
        {           
            var successFolder = ConfigurationManager.AppSettings["SuccessFolder"];

            _fileProcessor.Process(files);

            foreach (var file in files)
            {
                _directoryService.MoveFile(file, successFolder);
            }
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
