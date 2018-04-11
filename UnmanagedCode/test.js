var powerManagementService = new ActiveXObject("PowerManagement.PowerManagementService");

var lastSleepTime = powerManagementService.GetLastSleepTime();

WScript.Echo("Last sleep time: ");
WScript.Echo(lastSleepTime);
