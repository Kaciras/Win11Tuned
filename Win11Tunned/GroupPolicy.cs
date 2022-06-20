using System;
using Microsoft.Win32;

namespace Win11Tunned;

/// <summary>
/// 连续尝试两天修改组策略失败，Microsoft.GroupPolicy 不会用，最终还是选择了这种方式。
/// 组策略并非简单地修改注册表就行，它还需要 Group Policy Client 服务做一些额外的工作。
/// <br/>
/// 代码抄自：<see href="https://stackoverflow.com/a/22673417"/>
/// </summary>
public static class GroupPolicy
{
	[ExecuteOnSTAThread]
	public static void SetPolicySetting(string key, 
		string item, object value, RegistryValueKind kind)
	{
		var gpo = new ComputerGroupPolicyObject();
		var section = Key(key, out string subkey);

		using var root = gpo.GetRootRegistryKey(section);

		// Data can't be null so we can use this value to indicate key must be delete
		if (value == null)
		{
			using var subKey = root.OpenSubKey(subkey, true);
			if (subKey != null)
			{
				subKey.DeleteValue(item);
			}
		}
		else
		{
			using var subKey = root.CreateSubKey(subkey);
			subKey.SetValue(item, value, kind);
		}

		gpo.Save();
	}

	[ExecuteOnSTAThread]
	public static object GetPolicySetting(string key, string item)
	{
		var gpo = new ComputerGroupPolicyObject();
		var section = Key(key, out string subkey);

		using var root = gpo.GetRootRegistryKey(section);
		using var subKey = root.OpenSubKey(subkey, true);
		return subKey?.GetValue(item);
	}

	private static GroupPolicySection Key(string path, out string subkey)
	{
		var i = path.IndexOf('\\');
		var hive = path.Substring(0, i);
		subkey = path.Substring(i + 1);

		return hive.ToUpper() switch
		{
			"HKEY_LOCAL_MACHINE" or "HKLM" => GroupPolicySection.Machine,
			"HKEY_CURRENT_USER" or "HKCU" => GroupPolicySection.User,
			_ => throw new Exception($"错误的注册表 Root key: {hive}"),
		};
	}
}
