using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win11Tunned.Rules;

namespace Win11Tunned.Test.Rules;

[TestClass]
public sealed class FileAttributeRuleTest
{
	const FileAttributes TARGET = FileAttributes.Directory | FileAttributes.Hidden;

	[TestInitialize]
	public void SetUp()
	{
		var directory = Directory.CreateDirectory("test");

		// 默认多个 NoContentIndexed 给去掉。
		directory.Attributes = FileAttributes.Directory;
	}

	[TestCleanup]
	public void CleanUp()
	{
		Directory.Delete("test", false);
	}

	[TestMethod]
	public void FileNotExists()
	{
		var rule = new FileAttributeRule("NON_EXISTS", default, "test", "for test");
		Assert.IsFalse(rule.NeedOptimize());
	}

	[DataRow(FileAttributes.Directory, false)]
	[DataRow(TARGET, true)]
	[DataTestMethod]
	public void Check(FileAttributes attrs, bool expected)
	{
		var rule = new FileAttributeRule("test", attrs, "test", "for test");
		Assert.AreEqual(expected, rule.NeedOptimize());
	}

	[TestMethod]
	public void Optimize()
	{
		var rule = new FileAttributeRule("test", TARGET, "test", "for test");

		rule.Optimize();

		Assert.AreEqual(TARGET, File.GetAttributes("test"));
	}
}
