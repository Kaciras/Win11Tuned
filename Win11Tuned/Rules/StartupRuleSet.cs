using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace Win11Tuned.Rules;

public sealed class StartupRuleSet : OptimizableSet
{
	const string RUN = @"Software\Microsoft\Windows\CurrentVersion\Run";

	public string Name => "启动项删除";

	readonly List<string> user = new();

	public void Add(string name)
	{
		user.Add(name);
	}

	public IEnumerable<Optimizable> Scan()
	{
		using var run = Registry.CurrentUser.OpenSubKey(RUN);
		foreach (var name in user)
		{
			var command = run.GetValue(name);
			if (command == null)
			{
				continue;
			}

			var reader = new StringReader((string)command);
			var parser = new TextFieldParser(reader)
			{
				Delimiters = new string[] { " " },
				HasFieldsEnclosedInQuotes = true
			};
			var descr = parser.ReadFields()[0];

			try
			{
				descr = FileVersionInfo.GetVersionInfo(descr).FileDescription;
			}
			catch (FileNotFoundException)
			{
				// Ignore, just use filename from the command.
			}

			var Optimize = () =>
			{
				using var run = Registry.CurrentUser.OpenSubKey(RUN, true);
				run.DeleteValue(name);
			};
			yield return new OptimizeAction(descr, "", Optimize);
		}
	}
}
