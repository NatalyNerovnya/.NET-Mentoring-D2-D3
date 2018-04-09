using System;
using System.Runtime.InteropServices;

namespace PowerManagement
{
    internal class PowerManagementInterop
    {
        [DllImport("PowrProf.dll", SetLastError = true)]
        public static extern uint CallNtPowerInformation(
            POWER_INFORMATION_LEVEL InformationLevel,
            IntPtr lpInputBuffer,
            [MarshalAs(UnmanagedType.U4)]
            uint nInputBufferSize,
            IntPtr lpOutputBuffer,
            [MarshalAs(UnmanagedType.U4)]
            uint nOutputBufferSize
        );

        [DllImport("Powrprof.dll", SetLastError = true)]
        public static extern bool SetSuspendState(
            bool hibernate,
            bool forceCritical,
            bool disableWakeEvent
        );
    }

}
