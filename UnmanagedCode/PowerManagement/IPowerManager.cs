using System.Runtime.InteropServices;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("ECA83D70-C317-4EAD-B505-F800981CDA6D")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IPowerManager
    {
        long GetLastSleepTime();

        long GetLastWakeTime();

        SYSTEM_BATTERY_STATE GetSystemBatteryState();

        SYSTEM_POWER_INFORMATION GetSystemPowerInformation();

        void ReserveRemoveHibernatioFile(bool flag);

        void Sleep();
    }
}
