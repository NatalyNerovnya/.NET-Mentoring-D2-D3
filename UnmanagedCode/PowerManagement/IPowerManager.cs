using System.Runtime.InteropServices;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("a04a2ba2-1c42-4775-b860-eb52ce2bc622")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]

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
