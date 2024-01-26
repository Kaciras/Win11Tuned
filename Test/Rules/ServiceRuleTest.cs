using System.ServiceProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win11Tuned.Rules;

namespace Win11Tuned.Test.Rules;

[TestClass]
public sealed class ServiceRuleTest
{
	[TestInitialize]
	public void Init()
	{
		new WinServiceApi("Win11TunedTest", @"C:\foobar.exe").Install();
	}

	[TestCleanup]
	public void Cleanup()
	{
		WinServiceApi.Uninstall("Win11TunedTest");
	}

	[TestMethod]
	public void Optimize()
	{
		var rule = new ServiceRule("Win11TunedTest", "descr", ServiceState.Disabled);
		Assert.IsTrue(rule.NeedOptimize());

		rule.Optimize();
		using var controller = new ServiceController("Win11TunedTest");
		Assert.AreEqual(controller.StartType, ServiceStartMode.Disabled);
	}
}
