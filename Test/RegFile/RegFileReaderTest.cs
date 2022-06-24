using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win11Tuned.RegFile;
using Win11Tuned.Test.Properties;

namespace Win11Tuned.Test.RegFile;

[TestClass]
public sealed class RegFileReaderTest
{
	[TestMethod]
	public void ReadKey()
	{
		var reader = new RegFileReader(Resources.ImportTest);

		Assert.IsTrue(reader.Read());
		Assert.IsTrue(reader.IsKey);
		Assert.AreEqual(@"HKEY_CURRENT_USER\_Test_Import\Key", reader.Key);
	}

	[TestMethod]
	public void Multipart()
	{
		var reader = new RegFileReader(Resources.ValueParts);

		Assert.IsTrue(reader.Read());
		Assert.IsTrue(reader.Read());

		var expected = new string[] { "Str0", "Str1" };
		CollectionAssert.AreEqual(expected, (string[])reader.Value);
	}

	[ExpectedException(typeof(FormatException))]
	[TestMethod]
	public void TruncatedValue()
	{
		var reader = new RegFileReader(Resources.TruncatedValue);
		Assert.IsTrue(reader.Read());
		reader.Read();
	}

	[ExpectedException(typeof(FormatException))]
	[TestMethod]
	public void InvalidKind()
	{
		var reader = new RegFileReader(Resources.InvalidKind);

		Assert.IsTrue(reader.Read());
		reader.Read();
	}

	[TestMethod]
	public void ReadEmpty()
	{
		Assert.IsFalse(new RegFileReader("").Read());
	}

	[ExpectedException(typeof(FormatException))]
	[TestMethod]
	public void NoKey()
	{
		new RegFileReader(Resources.NoKey).Read();
	}
}
