using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Win11Tuned.Properties;

namespace Win11Tuned.Rules;

// Can't find GUID for MSI installer of this type of softwares, so we use it's uninstall string.
public sealed class SoftwareRuleSet : OptimizableSet
{
	const string UNINSTALL = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\";

	static readonly Regex EXE = new(@"\.exe(?: |$)");

	public string Name { get; }

	readonly List<string> keys = [];
	readonly RegistryKey @namespace;

	public SoftwareRuleSet(bool system)
	{
		@namespace = system ? Registry.LocalMachine : Registry.CurrentUser;
		Name = Resources.UninstallApps + (system ? Resources.SystemScope : Resources.UserScope);
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

	// "C:\Users\Kaciras\AppData\Local\Microsoft\OneDrive\25.174.0907.0003\OneDriveSetup.exe"  /uninstall 
	public void Optimize(string keyName)
	{
		using var key = @namespace.OpenSubKey(UNINSTALL + keyName);
		var command = (string)(key.GetValue("QuietUninstallString") ?? key.GetValue("UninstallString"));

        // Process.Start 需要分出文件和参数，另一种方法是用 CMD.exe /C
        var filename = command;
		var args = "";
		if (command[0] == '"')
		{
			var i = command.IndexOf('"', 1);
			filename = command.Substring(1, i - 1);
			args = command.Substring(i + 1);
		}
		else
		{
			var s = command.Split([' '], 2);
			filename = s[0];
			args = s.Length == 2 ? s[1] : "";
		}
		Process.Start(filename, "/quiet " + args).WaitForExit();
	}
}
