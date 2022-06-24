using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Win11Tuned.RegFile;
using Win11Tuned.Test.Properties;

namespace Win11Tuned.Test.RegFile;

[TestClass]
public sealed class RegFileTokenizerTest
{
	const string VERSION_LINE = "Windows Registry Editor Version 5.00\r\n\r\n";

	[DataRow("@=dword:00000123\r\n")]
	[DataRow("; comments\r\n")]
	[ExpectedException(typeof(FormatException))]
	[DataTestMethod]
	public void VersionFirst(string content)
	{
		new RegFileTokenizer(content).Read();
	}

	//[DataRow("[ke\r\ny]")]      // Key name contains a new line
	//[DataRow("[")]				// Unclosed key
	//[DataRow("\"na\r\nme\"")]   // Name contains a new line
	[DataRow("foobar")]         // Name not wrapped with quote
	[ExpectedException(typeof(FormatException))]
	[DataTestMethod]
	public void InvalidTopLevel(string content)
	{
		content = VERSION_LINE + content + "\r\n";
		var tokenizer = new RegFileTokenizer(content);
		Assert.IsTrue(tokenizer.Read());

		tokenizer.Read(); // ref struct cannot used in Assert.ThrowsException
	}

	[DataRow("=\"value")]       // Unclosed quote in value
	[DataRow("hex:00")]         // Missing = 
	[DataRow("=he\r\nx:00")]    // Kind contains a new line
	[ExpectedException(typeof(FormatException))]
	[DataTestMethod]
	public void InvalidValue(string content)
	{
		content = VERSION_LINE + "@" + content + "\r\n";
		var tokenizer = new RegFileTokenizer(content);
		tokenizer.Read();   // Version
		tokenizer.Read();   // Name
		tokenizer.Read();   // Value or Kind
	}

	[DataRow("\"name\"=-", RegTokenType.Name, RegTokenType.DeleteValue)]
	[DataRow("[key] ;注释", RegTokenType.CreateKey, RegTokenType.Comment)]
	[DataRow("[-key]", RegTokenType.DeleteKey)]
	[DataRow("\"name\"=dword:00000123;",
		RegTokenType.Name, RegTokenType.Kind, RegTokenType.Value, RegTokenType.Comment)]
	[DataTestMethod]
	public void ReadTokens(string content, params RegTokenType[] tokens)
	{
		content = VERSION_LINE + content + "\r\n";
		var tokenizer = new RegFileTokenizer(content);
		tokenizer.Read();

		foreach (var token in tokens)
		{
			Assert.IsTrue(tokenizer.Read());
			Assert.AreEqual(token, tokenizer.TokenType);
		}
		Assert.IsFalse(tokenizer.Read()); // 检查已经读完了。
	}

	[TestMethod]
	public void StringEscape()
	{
		var content = VERSION_LINE + "@=\"a\\\"b\";注释" + "\r\n";
		var tokenizer = new RegFileTokenizer(content);
		tokenizer.Read();
		tokenizer.Read();

		tokenizer.Read();
		Assert.AreEqual(RegTokenType.Value, tokenizer.TokenType);
		Assert.AreEqual("a\"b", tokenizer.Value);

		tokenizer.Read();
		Assert.AreEqual(RegTokenType.Comment, tokenizer.TokenType);
		Assert.AreEqual("注释", tokenizer.Value);
	}

	[TestMethod]
	public void ValueParts()
	{
		var tokenizer = new RegFileTokenizer(Resources.ValueParts);
		tokenizer.Read(); // Version
		tokenizer.Read(); // Key
		tokenizer.Read(); // Name
		tokenizer.Read(); // Kind

		Assert.IsTrue(tokenizer.Read());
		Assert.AreEqual(RegTokenType.ValuePart, tokenizer.TokenType);
		Assert.AreEqual("53,00,74,00,", tokenizer.Value);

		Assert.IsTrue(tokenizer.Read());
		Assert.AreEqual(RegTokenType.Comment, tokenizer.TokenType);
		Assert.AreEqual("测试多行 + 注释", tokenizer.Value);

		Assert.IsTrue(tokenizer.Read());
		Assert.AreEqual(RegTokenType.ValuePart, tokenizer.TokenType);
		Assert.AreEqual("72,00,30,00,", tokenizer.Value);

		Assert.IsTrue(tokenizer.Read());
		Assert.AreEqual(RegTokenType.Comment, tokenizer.TokenType);
		Assert.AreEqual("注释在值内占一行", tokenizer.Value);

		Assert.IsTrue(tokenizer.Read());
		Assert.AreEqual(RegTokenType.ValuePart, tokenizer.TokenType);
		Assert.AreEqual("00,00,53,00,74,00,72,00,31,00,00,00,00,", tokenizer.Value);

		Assert.IsTrue(tokenizer.Read());
		Assert.AreEqual(RegTokenType.Value, tokenizer.TokenType);
		Assert.AreEqual("00", tokenizer.Value);
	}
}
