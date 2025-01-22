using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Win32;
using RegistryEx;
using Win11Tuned.Helper;

namespace Win11Tuned.Rules;

/// <summary>
/// 注册表规则，能够判断是否需要导入内置的注册表文件（.reg），并在优化时将其导入。
/// <br/>
/// 注册表文件需要放在 RegFiles 目录下，并设置为 Embedded Resource.
/// <br/>
/// 导入注册表文件一条命令即可，但没有现成的方法可以比较其跟注册表是否一致，故自己实现了这功能。
/// </summary>
public class RegFileRule : Rule
{
	public string Name { get; }

	public string Description { get; }

	readonly RegDocument document;
	readonly List<string> elevates;

	public RegFileRule(
		string name,
		string description,
		string content,
		List<string> elevates
	) {
		document = new RegDocument();
		document.Load(content);

		Name = name;
		Description = description;
		this.elevates = elevates;
	}

	public RegFileRule(string name, string description, string content) 
		: this(name, description, content, []) {}

	public bool NeedOptimize()
	{
		return !document.IsSuitable;
	}

	public void Optimize()
	{
		using var _ = new MultiDisposable<string>(elevates, key =>
		{
			var (baseKey, path) = SplitForKey(key);
			return RegistryHelper.Elevate(baseKey, path);
		});

		document.Execute();
	}

	// TODO: Expose it in RegistryHelper
	private static (RegistryKey, string) SplitForKey(string path)
	{
		string item = "";
		string name = path;
		int num = path.IndexOf('\\');
		if (num != -1)
		{
			item = path.Substring(num + 1);
			name = path.Substring(0, num);
		}

		return (RegistryHelper.GetBaseKey(name), item);
	}
}
