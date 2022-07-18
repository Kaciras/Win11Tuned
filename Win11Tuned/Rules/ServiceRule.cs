using System.Security;
using Microsoft.Win32;
using RegistryEx;

namespace Win11Tuned.Rules;

public class ServiceRule : Rule
{
	const string SERVICE_DIR = @"SYSTEM\CurrentControlSet\Services\";

	/// <summary>
	/// 服务在注册表里的键，不同于显示名。
	/// </summary>
	public string Key { get; }

	/// <summary>
	/// 对此服务的简单介绍，以及需要优化的原因。
	/// </summary>
	public string Description { get; }

	/// <summary>
	/// 服务的显示名。
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// 此服务应当被优化为什么状态。
	/// </summary>
	public ServiceState TargetState { get; }

	readonly string subPath;

	public ServiceRule(string key, string description, ServiceState state)
	{
		Key = key;
		Description = description;
		TargetState = state;

		subPath = SERVICE_DIR + Key;
	}

	public bool NeedOptimize()
	{
		using var config = Registry.LocalMachine.OpenSubKey(subPath);

		if (config == null)
		{
			return false; // 服务不存在
		}
		if (Name == null)
		{
			Name = (string)config.GetValue("DisplayName", Key);
			if (Name.StartsWith("@"))
			{
				Name = Utils.ExtractStringResource(Name);
			}
		}

		var state = (ServiceState)config.GetValue("Start");
		var delayed = (int)config.GetValue("DelayedAutostart", -1) == 1;

		if (state == ServiceState.Automatic && delayed)
		{
			state = ServiceState.LazyStart;
		}

		return state != TargetState;
	}

	public void Optimize()
	{
		if (TargetState == ServiceState.Deleted)
		{
			Registry.LocalMachine.DeleteSubKeyTree(subPath);
			return;
		}
		try
		{
			ChaangeServiceStartupRegistry();
		}
		catch (SecurityException)
		{
			using var _ = RegistryHelper.Elevate(Registry.LocalMachine, subPath);
			ChaangeServiceStartupRegistry();
		}
	}

	void ChaangeServiceStartupRegistry()
	{
		using var key = Registry.LocalMachine.OpenSubKey(SERVICE_DIR + Key, true);
		var startValue = TargetState;

		if (TargetState == ServiceState.LazyStart)
		{
			key.SetValue("DelayedAutostart", 1, RegistryValueKind.DWord);
			startValue = ServiceState.Automatic;
		}
		else
		{
			// 默认不存在会报错，需要添加第二个参数
			key.DeleteValue("DelayedAutostart", false);
		}

		key.SetValue("Start", (int)startValue, RegistryValueKind.DWord);
	}
}
