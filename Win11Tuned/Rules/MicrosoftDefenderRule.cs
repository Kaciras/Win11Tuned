using Microsoft.Win32;
using RegistryEx;
using System.Collections.Generic;
using System.Linq;
using TaskScheduler;

namespace Win11Tuned.Rules;

/// <summary>
/// 禁用 Microsoft Defender 的脚本，需要在安全模式下运行。
/// <see href="https://github.com/TairikuOokami/Windows/blob/main/Microsoft%20Defender%20Enable.bat"/>
/// </summary>
class DisableDefenderStep2 : Rule
{
	public string Name => "Disable Defender Step 2";

	public string Description => "The final step of disable Microsoft Defender";

	public bool NeedOptimize()
	{
		return 2.Equals(Registry.GetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\MDCoreSvc", "Start", 0));
	}

	public void Optimize()
	{
		DisableService("SgrmBroker");
		DisableService("SecurityHealthService");
		DisableService("MDCoreSvc");
		DisableService("WdFilter");
		DisableService("WdNisDrv");
		DisableService("WdNisSvc");
		DisableService("WinDefend");
	}

	void DisableService(string name)
	{
		var keyPath = @"System\CurrentControlSet\Services\" + name;
        using var _ = RegistryHelper.Elevate(Registry.LocalMachine, keyPath);
        Registry.LocalMachine.SetValue(keyPath, "Start", 4);
    }
}

// 安全模式下用不了任务计划程序，会报找不到路径，所以必须分两次运行。
class DisableDefenderStep1 : Rule
{
	public string Name => "Disable Defender Step 1";

	public string Description => "After optimization, you should boot in safe mode to run the next step";

	static readonly string[] TASKS = [
		@"Microsoft\Windows\ExploitGuard\ExploitGuard MDM policy Refresh",
		@"Microsoft\Windows\Windows Defender\Windows Defender Cache Maintenance",
		@"Microsoft\Windows\Windows Defender\Windows Defender Cleanup",
		@"Microsoft\Windows\Windows Defender\Windows Defender Scheduled Scan",
		@"Microsoft\Windows\Windows Defender\Windows Defender Verification",
	];

	List<IRegisteredTask> toDisable;

	public bool NeedOptimize()
	{
		return (toDisable = [.. TASKS.Select(TaskSchedulerManager.Find).Where(x => x != null)]).Count != 0;
	}

	public void Optimize()
	{
		foreach (var task in toDisable) task.Enabled = false;
    }
}
