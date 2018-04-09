using System.Runtime.InteropServices;

namespace PowerManagement
{
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
