using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Win11Tuned.Test.Properties;

namespace Win11Tuned.Test;

[TestClass]
public sealed class RegHelperTest
{
	[TestCleanup]
	public void Cleanup()
	{
		Registry.CurrentUser.DeleteSubKeyTree("_Test_Import", false);
		Registry.CurrentUser.DeleteSubKeyTree("_Test_AutoConvert", false);
	}

	[DataRow(@"INVALID\Sub")]
	[DataRow(@"INVALID")]
	[DataTestMethod]
	public void OpenKeyNonExists(string path)
	{
		Assert.IsNull(RegHelper.OpenKey(path));
	}

	[ExpectedException(typeof(DirectoryNotFoundException))]
	[TestMethod]
	public void GetCLSIDValueException()
	{
		RegHelper.GetCLSIDValue("{66666666-0000-0000-6666-000000000000}");
	}

	[TestMethod]
	public void GetCLSIDValue()
	{
		var value = RegHelper.GetCLSIDValue("{C7657C4A-9F68-40fa-A4DF-96BC08EB3551}");
		Assert.AreEqual("Photo Thumbnail Provider", value);
	}

	[DataRow(@"INVALID\Sub", false)]
	[DataRow(@"INVALID", false)]
	[DataRow(@"HKCC\System", true)]
	[DataTestMethod]
	public void KeyExists(string path, bool expected)
	{
		Assert.AreEqual(expected, RegHelper.KeyExists(path));
	}

	[TestMethod]
	public void ContainsSubKey()
	{
		Assert.IsTrue(Registry.LocalMachine.ContainsSubKey(@"SOFTWARE\Microsoft"));
		Assert.IsFalse(Registry.LocalMachine.ContainsSubKey(@"SOFTWARE\Xicrosoft"));
	}

	[TestMethod]
	public void Import()
	{
		RegHelper.Import(@"Resources\Registry\ImportTest.reg");

		var value = Registry.GetValue(@"HKEY_CURRENT_USER\_Test_Import\Key", "StringValue", null);
		Assert.AreEqual("中文内容", value);
	}

	[TestMethod]
	public void Export()
	{
		using (var key = Registry.CurrentUser.CreateSubKey(@"_Test_Import\Key"))
		{
			key.SetValue("StringValue", "中文内容");
		}
		RegHelper.Export("ExportTest.reg", @"HKEY_CURRENT_USER\_Test_Import\Key");

		Assert.AreEqual(Resources.ImportTest, File.ReadAllText("ExportTest.reg"));
	}

	[TestMethod]
	public void Search()
	{
		RegHelper.Import(@"Resources\Registry\Search.reg");
		try
		{
			var result = RegHelper.Search(@"HKCR\测试项", "key");
			CollectionAssert.AreEquivalent(new string[] { "foo", "bar" }, result);
		}
		finally
		{
			Registry.ClassesRoot.DeleteSubKeyTree(@"测试项");
		}
	}

	[ExpectedException(typeof(DirectoryNotFoundException))]
	[TestMethod]
	public void ElevateNonExists()
	{
		RegHelper.Elevate(Registry.CurrentUser, "_NON_EXISTS");
	}

	[TestMethod]
	public void Elevate()
	{
		using var key = Registry.CurrentUser.CreateSubKey("_test_sec_0");

		var security = new RegistrySecurity();
		security.SetAccessRuleProtection(true, false);
		key.SetAccessControl(security);

		static void OpenWrite()
		{
			Registry.CurrentUser.OpenSubKey("_test_sec_0", true).Dispose();
		}

		Assert.ThrowsException<SecurityException>(OpenWrite);
		using (var _ = RegHelper.Elevate(Registry.CurrentUser, "_test_sec_0"))
		{
			OpenWrite();
		}
		Assert.ThrowsException<SecurityException>(OpenWrite);

		security.SetAccessRuleProtection(false, true);
		key.SetAccessControl(security);
		Registry.CurrentUser.DeleteSubKey("_test_sec_0");
	}

	/// <summary>
	/// 验证 Registry.SetValue() 无法直接接受 .reg 文件里的值格式，必须要先转换。
	/// </summary>
	[ExpectedException(typeof(ArgumentException))]
	[TestMethod]
	public void AutoConvertOnSetValue()
	{
		var key = @"HKEY_CURRENT_USER\_Test_AutoConvert";
		var text = "50,2d,02,09,60,d1,d6,01";
		var kind = RegistryValueKind.QWord;
		Registry.SetValue(key, "name", text, kind);
	}
}
