using Microsoft.Win32;

namespace Win11Tuned.Rules;

/// <summary>
/// 禁用 Microsoft Defender 的脚本，需要在安全模式下运行。
/// <see href="https://github.com/TairikuOokami/Windows/blob/main/Microsoft%20Defender%20Enable.bat"/>
/// </summary>
internal class NoDefenderRule : Rule
{
	public string Name => "禁用 Microsoft Defender";

	public string Description => "";

	public bool NeedOptimize()
	{
		return 2.Equals(Registry.GetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\MDCoreSvc", "Start", 0));
	}

	public void Optimize()
	{
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\SgrmBroker", "Start", 4);
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\SecurityHealthService", "Start", 4);
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\MDCoreSvc", "Start", 4);
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\WdFilter", "Start", 4);
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\WdNisDrv", "Start", 4);
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\WdNisSvc", "Start", 4);
		Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\WinDefend", "Start", 4);

		TaskSchedulerManager.Root.GetTask(@"Microsoft\Windows\ExploitGuard\ExploitGuard MDM policy Refresh").Enabled = false;
		TaskSchedulerManager.Root.GetTask(@"Microsoft\Windows\Windows Defender\Windows Defender Cache Maintenance").Enabled = false;
		TaskSchedulerManager.Root.GetTask(@"Microsoft\Windows\Windows Defender\Windows Defender Cleanup").Enabled = false;
		TaskSchedulerManager.Root.GetTask(@"Microsoft\Windows\Windows Defender\Windows Defender Scheduled Scan").Enabled = false;
		TaskSchedulerManager.Root.GetTask(@"Microsoft\Windows\Windows Defender\Windows Defender Verification").Enabled = false;
	}
}
