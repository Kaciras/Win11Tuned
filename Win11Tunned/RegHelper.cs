using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace Win11Tunned;

public static class RegHelper
{
	/// <summary>
	/// 从 .NET 标准库里抄的快捷方法，增加了根键的缩写支持，为什么微软不直接提供？
	/// <br/>
	/// <see href="https://referencesource.microsoft.com/#mscorlib/microsoft/win32/registry.cs,94"/>
	/// </summary>
	/// <returns>注册表键，如果不存在则为 null</returns>
	public static RegistryKey OpenKey(string path, bool wirte = false)
	{
		var basekeyName = path;
		var i = path.IndexOf('\\');
		if (i != -1)
		{
			basekeyName = path.Substring(0, i);
		}
		var basekey = GetBaseKey(basekeyName);

		if (i == -1 || i == path.Length)
		{
			return basekey;
		}
		else
		{
			var pathRemain = path.Substring(i + 1, path.Length - i - 1);
			return basekey?.OpenSubKey(pathRemain, wirte);
		}
	}

	static RegistryKey GetBaseKey(string name) => name.ToUpper() switch
	{
		"HKEY_CURRENT_USER" or "HKCU" => Registry.CurrentUser,
		"HKEY_LOCAL_MACHINE" or "HKLM" => Registry.LocalMachine,
		"HKEY_CLASSES_ROOT" or "HKCR" => Registry.ClassesRoot,
		"HKEY_USERS" or "HKU" => Registry.Users,
		"HKEY_CURRENT_CONFIG" or "HKCC" => Registry.CurrentConfig,
		"HKEY_PERFORMANCE_DATA" => Registry.PerformanceData,
		"HKEY_DYN_DATA" => RegistryKey.OpenBaseKey(RegistryHive.DynData, RegistryView.Default),
		_ => null, // 微软的 API 在不存在时返回 null，这里也保持一致而不是用异常。
	};

	/// <summary>
	/// 便捷的函数用于检查一个键是否存在于注册表中。
	/// </summary>
	public static bool KeyExists(string path)
	{
		var i = path.IndexOf('\\') + 1;
		if (i == 0)
		{
			return GetBaseKey(path) != null;
		}
		var basekey = GetBaseKey(path.Substring(0, i - 1));
		if (basekey == null)
		{
			return false;
		}
		path = path.Substring(i, path.Length - i);
		return basekey.ContainsSubKey(path);
	}

	/// <summary>
	/// 导出注册表键，相当于注册表编辑器里右键 -> 导出。
	/// </summary>
	/// <param name="file">保存的文件</param>
	/// <param name="path">注册表键</param>
	public static void Export(string file, string path) => Utils.Execute("regedit", $"/e {file} {path}");

	// 必须用 regedit.exe，如果用 regedt32 可能出错，上面的一样
	public static void Import(string file) => Utils.Execute("regedit", $"/s {file}");

	/// <summary>
	/// 从注册表中读取指定 CLSID 项的默认值。
	/// </summary>
	/// <param name="clsid">CLSID值，格式{8-4-4-4-12}</param>
	/// <exception cref="DirectoryNotFoundException">如果CLSID记录不存在</exception>
	public static string GetCLSIDValue(string clsid)
	{
		using var key = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + clsid);
		return (string)key?.GetValue(string.Empty)
			?? throw new DirectoryNotFoundException("CLSID 记录不存在");
	}

	/// <summary>
	/// 在指定的目录中搜索含有某个路径的项，只搜一层。
	/// <br/>
	/// 因为 rootKey 会销毁，必须在离开作用域前遍历完，所以返回IList。
	/// </summary>
	/// <param name="root">在此目录中搜索</param>
	/// <param name="key">要搜索的键路径</param>
	/// <returns>子项名字列表</returns>
	public static List<string> Search(string root, string key)
	{
		using var rootKey = OpenKey(root);
		return rootKey.GetSubKeyNames()
			.Where(name => rootKey.ContainsSubKey(Path.Combine(name, key)))
			.ToList();
	}

	public static bool ContainsSubKey(this RegistryKey key, string name)
	{
		using var subKey = key.OpenSubKey(name);
		return subKey != null;
	}

	/// <summary>
	/// 为当前用户设置完全控制键的权限，用户至少要有修改权限的权限。
	/// <br/>
	/// 尽管程序以管理员身份运行，仍有些注册表键没有修改权限，故需要添加一下。
	/// <code>
	/// // 使用 using 语法来自动还原：
	/// using var _ = RegistryHelper.ElevatePermission(key);
	/// </code>
	/// </summary>
	/// <param name="key">键</param>
	/// <returns>一个可销毁对象，在销毁时还原键的权限</returns>
	/// <see cref="https://stackoverflow.com/a/6491052"/>
	public static TemporaryElevateSession Elevate(RegistryKey baseKey, string name)
	{
		// 这几个权限枚举得设对，否则要么打不开要么无法改权限。
		var key = baseKey.OpenSubKey(
			name,
			RegistryKeyPermissionCheck.ReadWriteSubTree,
			RegistryRights.ChangePermissions);

		if (key == null)
		{
			throw new DirectoryNotFoundException();
		}

		var user = WindowsIdentity.GetCurrent().User;
		var rule = new RegistryAccessRule(user, RegistryRights.FullControl, AccessControlType.Allow);

		return new TemporaryElevateSession(key, rule);
	}

	public readonly struct TemporaryElevateSession : IDisposable
	{
		readonly RegistryKey key;

		readonly RegistryAccessRule rule;
		readonly RegistryAccessRule old;

		internal TemporaryElevateSession(RegistryKey key, RegistryAccessRule rule)
		{
			this.key = key;
			this.rule = rule;

			var security = key.GetAccessControl();

			var identity = rule.IdentityReference;
			old = security.GetAccessRules(true, false, identity.GetType())
				.Cast<RegistryAccessRule>()
				.FirstOrDefault(r => r.IdentityReference.Equals(identity));

			security.SetAccessRule(rule);
			key.SetAccessControl(security);
		}

		public void Dispose()
		{
			// key 可能被删除了，所以重新获取一遍。
			using var current = OpenKey(key.Name, true);
			key.Dispose();

			if (current != null)
			{
				var accessControl = current.GetAccessControl();
				if (old != null)
				{
					accessControl.SetAccessRule(old);
				}
				else
				{
					accessControl.RemoveAccessRule(rule);
				}
				current.SetAccessControl(accessControl);
			}
		}
	}
}
