﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PowerManagement
{
    [ComVisible(true)]
    [Guid("9D64C6A7-D452-42E3-9FF8-02246BB2470B")]
    [ClassInterface(ClassInterfaceType.None)]

    public class PowerManagementService : IPowerManager
    {
        private const int StatusSuccess = 0;

        public long GetLastSleepTime()
        {
            var longType = typeof(long);
            var result = Marshal.AllocCoTaskMem(Marshal.SizeOf(longType));

            var status = PowerManagementInterop.CallNtPowerInformation(
                POWER_INFORMATION_LEVEL.LastSleepTime,
                (IntPtr)null,
                0,
                result,
                (uint)Marshal.SizeOf(longType));

            if (status == StatusSuccess)
            {
                var lastSleepTime = (long)Marshal.PtrToStructure(result, longType);
                Marshal.FreeCoTaskMem(result);
                return lastSleepTime;
            }

            Marshal.FreeCoTaskMem(result);
            throw new Win32Exception();
        }

        public long GetLastWakeTime()
        {
            var longType = typeof(long);
            var result = Marshal.AllocCoTaskMem(Marshal.SizeOf(longType));

            var status = PowerManagementInterop.CallNtPowerInformation(
                POWER_INFORMATION_LEVEL.LastWakeTime,
                (IntPtr)null,
                0,
                result,
                (uint)Marshal.SizeOf(longType));

            if (status == StatusSuccess)
            {
                var lastWakeTime = (long)Marshal.PtrToStructure(result, longType);
                Marshal.FreeCoTaskMem(result);
                return lastWakeTime;
            }

            Marshal.FreeCoTaskMem(result);
            throw new Win32Exception();
        }

        public SYSTEM_BATTERY_STATE GetSystemBatteryState()
        {
            var sbsType = typeof(SYSTEM_BATTERY_STATE);
            var result = Marshal.AllocCoTaskMem(Marshal.SizeOf(sbsType));

            var status = PowerManagementInterop.CallNtPowerInformation(
                POWER_INFORMATION_LEVEL.SystemBatteryState,
                (IntPtr)null,
                0,
                result,
                (uint)Marshal.SizeOf(sbsType));

            if (status == StatusSuccess)
            {
                var sbs = (SYSTEM_BATTERY_STATE)Marshal.PtrToStructure(result, sbsType);
                Marshal.FreeCoTaskMem(result);
                return sbs;
            }

            Marshal.FreeCoTaskMem(result);
            throw new Win32Exception();
        }

        public SYSTEM_POWER_INFORMATION GetSystemPowerInformation()
        {
            var spiType = typeof(SYSTEM_POWER_INFORMATION);
            var result = Marshal.AllocCoTaskMem(Marshal.SizeOf(spiType));

            var status = PowerManagementInterop.CallNtPowerInformation(
                POWER_INFORMATION_LEVEL.SystemPowerInformation,
                (IntPtr)null,
                0,
                result,
                (uint)Marshal.SizeOf(spiType));

            if (status == StatusSuccess)
            {
                var spi = (SYSTEM_POWER_INFORMATION)Marshal.PtrToStructure(result, spiType);
                Marshal.FreeCoTaskMem(result);
                return spi;
            }

            Marshal.FreeCoTaskMem(result);
            throw new Win32Exception();
        }

        public void ReserveRemoveHibernatioFile(bool flag)
        {
            var lpInputBufer = Marshal.AllocCoTaskMem(sizeof(bool));
            Marshal.WriteByte(lpInputBufer, Convert.ToByte(flag));

            var status = PowerManagementInterop.CallNtPowerInformation(
                POWER_INFORMATION_LEVEL.SystemReserveHiberFile,
                lpInputBufer,
                sizeof(bool),
                (IntPtr)null,
                0);

            if (status != StatusSuccess)
            {
                Marshal.FreeCoTaskMem(lpInputBufer);
                throw new Win32Exception();
            }

            Marshal.FreeCoTaskMem(lpInputBufer);
        }

        public void Sleep()
        {
            PowerManagementInterop.SetSuspendState(false, false, false);
        }
    }

}
