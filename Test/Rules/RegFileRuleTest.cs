using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Win11Tunned.Rules;
using Win11Tunned.Test.Properties;

namespace Win11Tunned.Test.Rules;

[TestClass]
public sealed class RegFileRuleTest
{
	[TestInitialize]
	public void ImportTestData()
	{
		RegHelper.Import(@"Resources\Registry\Kinds.reg");
	}

	[TestCleanup]
	public void Cleanup()
	{
		Registry.CurrentUser.DeleteSubKeyTree("_Test_Kinds", false);
		Registry.CurrentUser.DeleteSubKeyTree("_Test_Import", false);
	}

	[TestMethod]
	public void CheckNoNeeded()
	{
		var rule = new RegFileRule("test", "test", Resources.Kinds);
		Assert.IsFalse(rule.NeedOptimize());
	}

	[TestMethod]
	public void Check()
	{
		var rule = new RegFileRule("test", "test", Resources.ImportTest);
		Assert.IsTrue(rule.NeedOptimize());
	}

	[TestMethod]
	public void Optimize()
	{
		var rule = new RegFileRule("test", "test", Resources.ImportTest);

		rule.Optimize();

		var value = Registry.GetValue(@"HKEY_CURRENT_USER\_Test_Import\Key", "StringValue", null);
		Assert.AreEqual("中文内容", value);
	}
}
