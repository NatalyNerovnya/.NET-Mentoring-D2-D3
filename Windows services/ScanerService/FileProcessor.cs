using System;
using System.Collections.Generic;
using ScanerService.Interfaces;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ScanerService.Interafces;

namespace ScanerService
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly string _successFolder;
        private readonly string _errorFolder;
        private readonly string _processingFolder;
        private readonly string _fileNamePattern;

        public FileProcessor(string successFolder, string errorFolder, string processingFolder, string fileNamePattern)
        {
            _directoryService = new DirectoryService();
            _fileService = new PdfFileService(successFolder);
            _successFolder = successFolder;
            _errorFolder = errorFolder;
            _processingFolder = processingFolder;
            _fileNamePattern = fileNamePattern;
        }

        public void ProcessFiles(string filePath, List<IInteruptRule> rules)
        {
            //As files should come from scanner their creation time should be around now (for TimerRule)
            File.SetCreationTime(filePath, DateTime.Now);
            
            CheckAndProcess(filePath, rules);
        }

        public void ProcessWaitingFiles(string watchFolder, List<IInteruptRule> rules)
        {
            var files = Directory.EnumerateFiles(watchFolder).ToList();

            if (!files.Any()) return;
            
            foreach (var file in files)
            {
                CheckAndProcess(file, rules);
            }
        }

        private void CheckAndProcess(string filePath, IEnumerable<IInteruptRule> rules)
        {
            foreach (var rule in rules)
            {
                if (CheckRules(rule, filePath)) break;
            }

            if (CheckImageName(filePath))
            {
                _fileService.AddPage(filePath);
                _directoryService.MoveFile(filePath, _processingFolder);
            }
            else
            {
                if (File.Exists(filePath))
                {
                    _directoryService.MoveFile(filePath, _errorFolder);
                }
            }
        }

        private bool CheckRules(IInteruptRule rule, string filePath)
        {
            if (rule.IsMatch(filePath))
            {
                _fileService.SaveDocument();

                foreach (var file in Directory.EnumerateFiles(_processingFolder))
                {
                    _directoryService.MoveFile(file, _successFolder);
                }

                return true;
            }

            return false;
        }

        private bool CheckImageName(string imageName)
        {
            return Regex.IsMatch(imageName, _fileNamePattern);
        }
    }
}
