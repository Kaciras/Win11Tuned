using System;
using System.Collections.Generic;
using System.IO;
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
		var sys = Environment.ExpandEnvironmentVariables(PATH);
        var target = new HostsFile();

        try
		{
            using var reader = new StreamReader(sys);
			target.Load(reader);
		}
		catch (FileNotFoundException) 
		{
			// In rare cases, the hosts file can be missing.
		}

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
		var hosts = new HostsFile();
		var fileMissing = false;

		try
		{
			using var reader = new StreamReader(file);
			hosts.Load(reader);
		}
		catch (FileNotFoundException)
		{
			fileMissing = true;
		}

		hosts.AddEmptyLine();
		foreach (var (host, ip) in document.Entries())
		{
			hosts.RemoveAll(host);
			hosts.Add(host, ip);
		}

		if (fileMissing)
		{
			hosts.WriteTo(file);
		}
		else
		{
			using (new TempWriteableSession(file))
			{
				hosts.WriteTo(file);
			}
		}
	}
}
