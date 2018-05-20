using System;
using System.IO;
using ScanerService.Interfaces;

namespace ScanerService.Rules
{
    public class TimerRule: IInteruptRule
    {
        private DateTime _lastUploadFileTime;
        private readonly TimeSpan _timerSpan;

        public TimerRule(int timerTime)
        {
            _timerSpan = new TimeSpan(0, 0, 0, timerTime);
        }

        public bool IsMatch(string file)
        {
            var creationTime = DateTime.UtcNow;

            if (_lastUploadFileTime == DateTime.MinValue)
            {
                _lastUploadFileTime = creationTime;

                return false;
            }

            var result = _lastUploadFileTime + _timerSpan <= creationTime;
            _lastUploadFileTime = creationTime;

            return result;
        }
    }
}
