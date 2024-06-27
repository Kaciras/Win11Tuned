using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Win11Tuned.Test;

[TestClass]
public sealed class TempWriteableSessionTest
{
    const string TEST_FILE = "writeable.test";

    [TestInitialize]
    public void Setup()
    {
		File.Create(TEST_FILE).Dispose();
		File.SetAttributes(TEST_FILE, FileAttributes.ReadOnly);
	}

    [TestCleanup]
    public void Cleanup()
    {
        if (!File.Exists(TEST_FILE))
        {
            return;
        }
		File.SetAttributes(TEST_FILE, FileAttributes.Normal);
		File.Delete(TEST_FILE);
	}

	[ExpectedException(typeof(FileNotFoundException))]
	[TestMethod]
	public void FileNotExists()
	{
		new TempWriteableSession("__NOT_EXISTS_");
	}

	[TestMethod]
    public void ShouldWorks()
    {
        using (new TempWriteableSession(TEST_FILE))
        {
            File.WriteAllText(TEST_FILE, "foo");
        }

        try
        {
			File.WriteAllText(TEST_FILE, "bar");
            Assert.Fail();
		}
        catch (UnauthorizedAccessException)
        {
            Assert.AreEqual("foo", File.ReadAllText(TEST_FILE));
        }
    }

    [TestMethod]
    public void DeleteBeforeDispose()
    {
		using (new TempWriteableSession(TEST_FILE))
		{
			File.Delete(TEST_FILE);
		}
        Assert.IsFalse(File.Exists(TEST_FILE));
	}

    [TestMethod]
    public void KeepAttributesExceptReadonly()
    {
        const FileAttributes OTHERS = FileAttributes.Temporary | FileAttributes.Hidden;
        const FileAttributes ATTRS = OTHERS | FileAttributes.ReadOnly;

        File.SetAttributes(TEST_FILE, ATTRS);
        using (new TempWriteableSession(TEST_FILE))
        {
            Assert.AreEqual(OTHERS, File.GetAttributes(TEST_FILE));
        }
        Assert.AreEqual(ATTRS, File.GetAttributes(TEST_FILE));
    }
}
