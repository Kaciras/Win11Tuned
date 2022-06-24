using System;
using System.IO;
using System.Text;

namespace Win11Tuned.RegFile;

/// <summary>
/// 解析注册表导出文件（.reg）的工具，属于编译器里最前端的分词器。
/// 我觉得应该有开源库读取 reg 文件的，但是找了一圈也没找到，只能自己撸了。
/// <br/>
/// 本分词器有一些额外的限制：
/// <list>
/// <item>1）必须使用 \r\n 换行</item>
/// <item>2）文件末尾必须有一个空行</item>
/// </list>
/// <seealso cref="https://support.microsoft.com/en-us/help/310516/how-to-add-modify-or-delete-registry-subkeys-and-values-by-using-a-reg"/>
/// </summary>
public ref struct RegFileTokenizer
{
	/*
	 * 每个标记加上换行符，用于 IndexOfAny 搜索同时检查是否有换行。
	 * 因为 switch 只能用于常量，懒得写三个所以只能这样了。
	 */
	static readonly char[] KEY_END = { ']', '\r' };
	static readonly char[] QUOTE = { '"', '\r' };
	static readonly char[] KIND_END = { ':', '\r' };

	readonly string content;

	int i;
	bool hasMoreLines;

	public RegTokenType TokenType { get; private set; }

	public string Value { get; private set; }

	public RegFileTokenizer(string content)
	{
		this.content = content;

		Value = null;
		i = 0;
		hasMoreLines = false;
		TokenType = RegTokenType.None;
	}

	/// <summary>
	/// 读取下一个 Token，如果已经读完则返回 false，否则返回 true
	/// </summary>
	public bool Read()
	{
		// 用异常表示结束，省去了大量的 return。
		// 虽然有违异常的原则，但仅内部使用也没问题。
		// 实测无论哪种写性能都是微妙级，无需关心。
		try
		{
			Dispatch();
			return true;
		}
		catch (EndOfStreamException)
		{
			return false;
		}
	}

	void Dispatch()
	{
		if (hasMoreLines)
		{
			ConsumeNextPart();
			return;
		}
		switch (TokenType)
		{
			case RegTokenType.None:
				ReadVersion();
				break;
			case RegTokenType.Name:
				ConsumeKindOrString();
				break;
			case RegTokenType.Kind:
				ReadValue();
				break;
			default:
				ConsumeTopLevel();
				break;
		}
	}

	void ConsumeTopLevel()
	{
		SkipBlankLines();

		switch (content[i])
		{
			case '@':
				ReadDefaultName();
				break;
			case '"':
				ReadName();
				break;
			case '[':
				ReadKey();
				break;
			case ';':
				ReadComment();
				break;
			default:
				throw Unexpected(content[i]);
		}
	}

	// 上一个词是不完整的值，接下来只允许值和注释
	void ConsumeNextPart()
	{
		SkipBlankLines();

		if (content[i] != ';')
		{
			ReadValue();
		}
		else
		{
			ReadComment();
		}
	}

	void ReadComment()
	{
		TokenType = RegTokenType.Comment;
		var j = i + 1;
		i = content.IndexOf('\r', j);
		Value = content.Substring(j, i - j);
	}

	// 旧版的 REGEDIT4 就不支持了，Win 7 以上都是 5.0 的了。
	void ReadVersion()
	{
		const string VER_LINE = "Windows Registry Editor Version 5.00";

		CheckHasValue();

		var j = i;
		i = content.IndexOf('\r', j);
		if (i == -1)
		{
			throw new FormatException("Reg 文件末尾必须要有空行");
		}

		TokenType = RegTokenType.Version;
		Value = content.Substring(j, i - j);

		if (Value != VER_LINE)
		{
			throw new FormatException("Invalid version: " + Value);
		}
	}

	void ReadKey()
	{
		if (content[++i] == '-')
		{
			TokenType = RegTokenType.DeleteKey;
			i += 1;
		}
		else
		{
			TokenType = RegTokenType.CreateKey;
		}

		Value = InlineReadTo(KEY_END);
	}

	void ReadDefaultName()
	{
		i += 1;
		TokenType = RegTokenType.Name;
		Value = string.Empty;
	}

	void ReadName()
	{
		i += 1;
		TokenType = RegTokenType.Name;
		Value = InlineReadTo(QUOTE);
	}

	// 字符串值也放在这了，因为已经读了一个引号，免得回看。
	void ConsumeKindOrString()
	{
		if (content[i] != '=')
		{
			throw Unexpected(content[i]);
		}
		switch (content[++i])
		{
			case '"':
				TokenType = RegTokenType.Value;
				i += 1;
				Value = ReadQuoted();
				break;
			case '-':
				i += 1;
				TokenType = RegTokenType.DeleteValue;
				break;
			default:
				Value = InlineReadTo(KIND_END);
				TokenType = RegTokenType.Kind;
				break;
		}
	}

	// 类型后面必须立即跟着值，不能是注释然后把值写到下一行。
	void ReadValue()
	{
		var j = i;

		for (; i < content.Length; i++)
		{
			switch (content[i])
			{
				case '\\':
					TokenType = RegTokenType.ValuePart;
					hasMoreLines = true;
					Value = content.Substring(j, i - j);
					i += 1;
					return;
				case ';':
				case '\r':
					goto SearchEnd;
			}
		}

	SearchEnd:

		TokenType = RegTokenType.Value;
		hasMoreLines = false;
		Value = content.Substring(j, i - j);
	}

	/// <summary>
	/// 读取被双引号包围的内容，自动处理转义，内容不能有换行。
	/// </summary>
	string ReadQuoted()
	{
		var buffer = new StringBuilder();
		var j = i;

		for (; i < content.Length; i++)
		{
			switch (content[i])
			{
				case '\\':
					buffer.Append(content, j, i - j);
					j = i += 1;
					break;
				case '"':
					buffer.Append(content, j, i - j);
					i += 1;
					return buffer.ToString();
				case '\r':
					throw new FormatException("字符串不能换行");
			}
		}

		throw new FormatException("Reg 文件末尾必须要有空行");
	}

	/// <summary>
	/// 从当前位置读取到指定字符出现的位置，中间不允许换行。
	/// </summary>
	/// <param name="ch">结束字符</param>
	/// <returns>读取到的字符串</returns>
	string InlineReadTo(char[] chars)
	{
		var j = i;
		i = content.IndexOfAny(chars, i);

		if (i == -1)
		{
			throw new FormatException("数据不完整");
		}
		if (content[i] == '\r')
		{
			throw new FormatException("不能换行");
		}

		return content.Substring(j, (i++) - j);
	}

	/// <summary>
	/// 创建一个表示遇到非预期字符的异常，因为代码较长所以单独提出来。
	/// </summary>
	/// <param name="ch">读到的字符</param>
	Exception Unexpected(char ch)
	{
		return new FormatException("Unexpected char: " + ch);
	}

	void CheckHasValue()
	{
		if (i >= content.Length)
		{
			throw new EndOfStreamException();
		}
	}

	/// <summary>
	/// 跳过空白部分，顺带检查了下还有没有更多内容。
	/// </summary>
	void SkipBlankLines()
	{
		for (; i < content.Length; i++)
		{
			switch (content[i])
			{
				case '\r':
				case '\n':
				case ' ':
				case '\t':
					break;
				default:
					return;
			}
		}
		if (i >= content.Length)
		{
			throw new EndOfStreamException();
		}
	}
}
