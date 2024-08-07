using System;
using System.Collections.Generic;
using System.Linq;
using Win11Tuned.Properties;

namespace Win11Tuned.Rules;

public class HostsRuleSet : OptimizableSet
{
	public const string PATH = @"%SystemRoot%\System32\drivers\etc\hosts";

	public string Name => Resources.HostsFile;

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
		using var reader = Utils.OpenEmbedded(asset);

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
		var hosts = new HostsFile(file);

		hosts.AddEmptyLine();
		foreach (var (host, ip) in document.Entries())
		{
			hosts.RemoveAll(host);
			hosts.Add(host, ip);
		}
		using (new TempWriteableSession(file))
		{
			hosts.WriteTo(file);
		}
	}
}
