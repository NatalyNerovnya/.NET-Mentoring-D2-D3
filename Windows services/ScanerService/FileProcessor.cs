using System;
using System.Collections.Generic;
using ScanerService.Interfaces;
using System.IO;
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
        private readonly string _fileNamePattern;

        public FileProcessor(string successFolder, string errorFolder, string fileNamePattern)
        {
            _directoryService = new DirectoryService();
            _fileService = new PdfFileService(successFolder);
            _successFolder = successFolder;
            _errorFolder = errorFolder;
            _fileNamePattern = fileNamePattern;
        }

        public void ProcessFiles(string filePath, List<IInteruptRule> rules)
        {
            //As files should come from scanner their creation time should be around now (for TimerRule)
            File.SetCreationTime(filePath, DateTime.Now);

            foreach (var rule in rules)
            {
                if (CheckAndProcess(rule, filePath))break;
            }

            if (CheckImageName(filePath))
            {
                _fileService.AddPage(filePath);
                _directoryService.MoveFile(filePath, _successFolder);
            }
            else
            {
                if (File.Exists(filePath))
                {
                    _directoryService.MoveFile(filePath, _errorFolder);
                }
            }
        }

        public void ProcessWaitingFiles(List<IInteruptRule> rules)
        {
            //var files = Directory.EnumerateFiles(_watchedFolder).ToList();

            //if (!files.Any()) return;

            //var filesInFolder = new List<string>();

            //files.ForEach(x => filesInFolder.Add(x));

            //foreach (var file in files)
            //{
            //    foreach (var rule in rules)
            //    {
            //        var filesToFile = filesInFolder.TakeWhile(x => x != file).Where(CheckImageName).ToList();

            //        filesToFile.ForEach((f) =>
            //        {
            //            if (CheckAndProcess(rule, f))
            //            {
            //                filesInFolder.Remove(f);
            //            }
            //        });
            //    }
            //}
        }

        private bool CheckAndProcess(IInteruptRule rule, string filePath)
        {
            if (rule.IsMatch(filePath))
            {
                _fileService.SaveDocument();
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
