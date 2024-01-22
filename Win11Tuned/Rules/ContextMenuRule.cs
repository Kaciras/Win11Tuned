using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using RegistryEx;

namespace Win11Tuned.Rules;

public sealed class ContextMenuRule : Rule
{
	readonly string item;
	readonly IEnumerable<string> folders;

	public string Name { get; }

	public string Description { get; }

	public ContextMenuRule(string item, IEnumerable<string> folders, string name, string description)
	{
		this.item = item;
		this.folders = folders;
		Name = name;
		Description = description;
	}

	public bool NeedOptimize()
	{
		return folders
			.Select(folder => Path.Combine(folder, item))
			.Any(Registry.ClassesRoot.ContainsSubKey);
	}

	public void Optimize()
	{
		foreach (var folder in folders)
		{
			var key = Path.Combine("HKCR", folder, item);
			RegistryHelper.DeleteKeyTree(key, false);
		}
	}
}
