using System.Runtime.InteropServices;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("1fe8d7f2-fbd7-436a-988d-f3b416eb93ed")]
    public struct SYSTEM_POWER_INFORMATION
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;
    }
}
