using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Win11Tuned.Test;

[TestClass]
public sealed class SimpleIniFileTest
{
	[TestMethod]
	public void GetNonExistValue()
	{
		var file = new SimpleIniFile("Resources/Config.ini");
		Assert.AreEqual("s", file.Read("server", "non-exists", "s"));
	}

	[TestMethod]
	public void GetNonExistSection()
	{
		var file = new SimpleIniFile("Resources/Config.ini");
		Assert.AreEqual("s", file.Read("server", "non-exists", "s"));
	}

	[TestMethod]
	public void GetNonExistFile()
	{
		var file = new SimpleIniFile("non-exists.ini");
		Assert.AreEqual("s", file.Read("server", "non-exists", "s"));
	}

	[TestMethod]
	public void GetValue()
	{
		var file = new SimpleIniFile("Resources/Config.ini");
		Assert.AreEqual("hello world", file.Read("server", "key", null));
	}
}
