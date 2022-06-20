using System.ServiceProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win11Tunned.Rules;

namespace Win11Tunned.Test.Rules;

[TestClass]
public sealed class ServiceRuleTest
{
	[TestInitialize]
	public void Init()
	{
		new WinSvcApi("Win11TunnedTest", @"C:\foobar.exe").Install();
	}

	[TestCleanup]
	public void Cleanup()
	{
		WinSvcApi.Uninstall("Win11TunnedTest");
	}

	[TestMethod]
	public void Optimize()
	{
		var rule = new ServiceRule("Win11TunnedTest", "descr", ServiceState.Disabled);
		Assert.IsTrue(rule.NeedOptimize());

		rule.Optimize();
		using var controller = new ServiceController("Win11TunnedTest");
		Assert.AreEqual(controller.StartType, ServiceStartMode.Disabled);
	}
}
