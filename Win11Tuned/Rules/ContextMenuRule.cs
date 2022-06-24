using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace Win11Tuned.Rules;

public sealed class ContextMenuRule : Rule
{
	/// <summary>
	/// 当没有子目录时它们也可以被删除，这些目录内不应该有值。
	/// <br/>
	/// 如果要删除多级，请将每一级都写上，并且下级要放在上级的前面。
	/// </summary>
	static readonly string[] EmptyRemovable = {
		"shell", @"shellex\ContextMenuHandlers", "shellex",
	};

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
			using var folderKey = Registry.ClassesRoot.OpenSubKey(folder, true);
			try
			{
				folderKey.DeleteSubKeyTree(item);
			}
			catch (ArgumentException)
			{
				continue; // 菜单项不存在，跳过空目录删除过程
			}

			// 如果一些目录（shell, shellex, etc...）为空则删除
			foreach (var path in EmptyRemovable)
			{
				try
				{
					folderKey.DeleteSubKey(path);
				}
				catch (ArgumentException)
				{
					// 被外部删空或是本来就不存在
				}
				catch (InvalidOperationException)
				{
					break; // 目录非空，删除终止
				}
			}
		}
	}
}

