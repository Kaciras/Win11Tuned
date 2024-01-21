using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Win11Tuned.Rules;

public class HostsRuleSet : OptimizableSet
{
	public const string PATH = @"%SystemRoot%\System32\drivers\etc\hosts";

	public string Name => "Hosts 文件";

	readonly List<HostsRule> rules = [];

	public void LoadList(string name)
	{
		rules.Add(new HostsRule(name));
	}

	public IEnumerable<Optimizable> Scan()
	{
		var target = new HostsFile(Environment.ExpandEnvironmentVariables(PATH));
		return rules.Where(rule => rule.Check(target));
	}
}

sealed class HostsRule : Optimizable
{
	public string Name { get; }

	public string Description { get; }

	readonly HostsFile document = new();

	public HostsRule(string asset)
	{
		var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(asset);
		using var reader = new StreamReader(stream);

		Name = reader.ReadLine().Substring(2);
		Description = reader.ReadLine().Substring(2);
		document.Load(reader);
	}

	public bool Check(HostsFile target)
	{
		return document.Entries().Any(e => !target.ContainsExactly(e.Item1, e.Item2));
	}

	// 会多次打开文件，但这是本项目的设计缺陷，我也懒得改了。
	public void Optimize()
	{
		var file = Environment.ExpandEnvironmentVariables(HostsRuleSet.PATH);
		var target = new HostsFile(file);

		target.AddEmptyLine();
		foreach (var (host, ip) in document.Entries())
		{
			target.RemoveAll(host);
			target.Add(host, ip);
		}
		target.Save(file);
	}
}
