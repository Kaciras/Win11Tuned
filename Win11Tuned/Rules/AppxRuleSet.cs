using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Win11Tuned.Properties;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Management.Deployment;

namespace Win11Tuned.Rules;

public sealed class AppxRuleSet : OptimizableSet
{
	public string Name => Resources.UWPApps;

	readonly HashSet<string> uninstall = [];

	readonly List<string> install = [];

	public void Uninstall(string name)
	{
		uninstall.Add(name);
	}

	public void Install(string name)
	{
		install.Add(name);
	}

	public IEnumerable<Optimizable> Scan()
	{
		// TODO: DeprovisionPackageForAllUsersAsync 彻底删除 
		var packageManager = new PackageManager();

        return packageManager
			.FindPackagesForUser("")
			.Where(package => uninstall.Contains(package.Id.Name))
			.Select(package => new UninstallAppx(package));
	}
}

public sealed class InstallAppx : Optimizable
{
	public string Name { get; }

	public string Description { get; }

	readonly Package package;

	public InstallAppx(Package package, string descroption = "安装这个 App")
	{
		this.package = package;
		Name = Resources.Install + " - " + package.DisplayName;
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

public sealed class UninstallAppx : Optimizable
{
	public string Name { get; }

	public string Description { get; }

	readonly Package package;

	public UninstallAppx(Package package, string description = "如果你不用它就卸了吧")
	{
		this.package = package;
		Name = Resources.Uninstall + " - " + package.DisplayName;
		Description = description;
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
