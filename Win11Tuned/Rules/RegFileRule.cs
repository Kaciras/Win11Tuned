using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using RegistryEx;

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

	public RegFileRule(string name, string description, string content)
	{
		document = new RegDocument();
		document.Load(content);
		
		Name = name;
		Description = description;
	}

	public bool NeedOptimize()
	{
		return !document.IsSuitable;
	}

	public void Optimize() => document.Execute();
}
