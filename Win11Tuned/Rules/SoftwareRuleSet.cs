using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Win11Tuned.Rules;

// Can't find GUID for MSI installer of this type of softwares, so we use it's uninstall string.
public sealed class SoftwareRuleSet : OptimizableSet
{
	const string UNINSTALL = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\";

	static readonly Regex EXE = new(@"\.exe(?: |$)");

	public string Name => $"卸载应用（用户）";

	readonly List<string> keys = new();
	readonly RegistryKey @namespace;

	public SoftwareRuleSet(RegistryKey @namespace)
	{
		this.@namespace = @namespace;
	}

	public void Add(string key) => keys.Add(key);

	public IEnumerable<Optimizable> Scan()
	{
		foreach (var keyName in keys)
		{
			using var key = @namespace.OpenSubKey(UNINSTALL + keyName);
			if (key == null)
			{
				continue;
			}
			var name = (string)key.GetValue("DisplayName", keyName);
			yield return new OptimizeAction(name, "", () => Optimize(keyName));
		}
	}

	public void Optimize(string keyName)
	{
		using var key = @namespace.OpenSubKey(UNINSTALL + keyName);
		var cmd = key.GetValue("QuietUninstallString") ?? key.GetValue("UninstallString");
		var splited = EXE.Split((string)cmd, 2);
		Process.Start(splited[0] + ".exe", splited[1]).WaitForExit();
	}
}
