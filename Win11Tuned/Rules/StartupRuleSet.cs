using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace Win11Tuned.Rules;

public sealed class StartupRuleSet : OptimizableSet
{
	const string RUN32 = @"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
	const string RUN = @"Software\Microsoft\Windows\CurrentVersion\Run";

	public string Name { get; }

	readonly List<string> patterns = [];
	readonly RegistryKey basekey;

	public StartupRuleSet(bool isSystem)
	{
		if (isSystem)
		{
			Name = "删除启动项（系统）";
			basekey = Registry.LocalMachine;
		}
		else
		{
			Name = "删除启动项（用户）";
			basekey = Registry.CurrentUser;
		}
	}

	public void Add(string regex)
	{
		patterns.Add(regex);
	}

	public IEnumerable<Optimizable> Scan()
	{
		var regex = new Regex("(?:" + string.Join("|", patterns) + ")");
		return Scan(regex, RUN).Concat(Scan(regex, RUN32));
	}

	public IEnumerable<Optimizable> Scan(Regex regex, string @namespace)
	{
		using var run = basekey.OpenSubKey(@namespace);
		if (run == null)
		{
			yield break;
		}
		foreach (var name in run.GetValueNames())
		{
			if (regex.IsMatch(name))
			{
				var command = (string)run.GetValue(name);
				var descr = GetDisplayName(command);

				yield return new OptimizeAction(descr, "不用的启动项删了吧", () =>
				{
					using var run = basekey.OpenSubKey(@namespace, true);
					run.DeleteValue(name);
				});
			}
		}
	}

	string GetDisplayName(string command)
	{
		using var parser = new TextFieldParser(new StringReader(command))
		{
			Delimiters = [" "],
			HasFieldsEnclosedInQuotes = true
		};

		var first = parser.ReadFields()[0];
		try
		{
			return FileVersionInfo.GetVersionInfo(first).FileDescription;
		}
		catch (FileNotFoundException)
		{
			return first; // Not a file, just display the first part.
		}
	}
}
