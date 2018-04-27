using System;

namespace ScanerService.Status
{
    [Serializable]
    public class ServiceStatus
    {
        public int PageTimeout { get; set; }
        public string BarcodeString { get; set; }
        public CurerntState Status { get; set; }

    }

    [Serializable]
    public enum CurerntState
    {
        WatingFiles = 1,
        ProcessFiles,
    }
}
