using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Win11Tuned.Test;

[TestClass]
public sealed class OptimizeActionTest
{
	[TestMethod]
	public void Properties()
	{
		var item = new OptimizeAction("foo", "bar", () => { });
		Assert.AreEqual("foo", item.Name);
		Assert.AreEqual("bar", item.Description);
	}

	[TestMethod]
	public void Optimize()
	{
		var called = false;
		var item = new OptimizeAction("foo", "bar", () => called = true);

		item.Optimize();
		Assert.IsTrue(called);
	}
}
