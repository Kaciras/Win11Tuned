using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Management.Deployment;

namespace Win11Tunned.Rules;

public sealed class AppxRuleSet : OptimizableSet
{
	public string Name => "卸载预装 UWP 应用";

	private readonly HashSet<string> uninstall = new();

	public void Add(string name)
	{
		uninstall.Add(name);
	}

	public IEnumerable<Optimizable> Scan()
	{
		var packageManager = new PackageManager();
		return packageManager
			.FindPackagesForUser("")
			.Where(package => uninstall.Contains(package.Id.Name))
			.Select(package => new UninstallAppx(package));
	}
}

public sealed class UninstallAppx : Optimizable
{
	public string Name { get; private set; }

	public string Description { get; }

	private readonly Package package;

	public UninstallAppx(Package package, string descroption = "如果你不用它就卸了吧")
	{
		this.package = package;
		Name = "卸载 - " + package.DisplayName;
		Description = descroption;
	}

	public void Optimize()
	{
		var packageManager = new PackageManager();
		var task = packageManager.RemovePackageAsync(package.Id.FullName);

		var @event = new ManualResetEvent(false);
		task.Completed = (_, _) => @event.Set();
		@event.WaitOne();

		if (task.Status == AsyncStatus.Error)
		{
			var reason = task.GetResults().ErrorText;
			throw new Exception($"卸载 ${package.DisplayName} 失败：{reason}");
		}
	}
}
