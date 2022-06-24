using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Win11Tuned.RegFile;

/// <summary>
/// Reg 文件读取器，是 RegFileTokenizer 的下一层，处理上下文相关的语法。
/// <br/>
/// 该读取器只返回完整的条目（键或值），其它部分如版本号和注释将被忽略。
/// </summary>
public ref struct RegFileReader
{
	/// <summary>
	/// 当前读取到的条目是键吗，如果不是则为值。
	/// </summary>
	public bool IsKey { get; private set; } = default;

	public string Key { get; private set; } = default;

	public string Name { get; private set; } = default;

	public RegistryValueKind Kind { get; private set; } = default;

	public object Value { get; private set; } = default;

	public bool IsDelete { get; private set; } = default;

	RegFileTokenizer tokenizer;

	public RegFileReader(string content)
	{
		tokenizer = new RegFileTokenizer(content);
	}

	/// <summary>
	/// 读取下一个键或值，读取到的内容由该对象的各个属性来获取。
	/// </summary>
	/// <returns>如果读完了则为 false，否则 true</returns>
	/// <exception cref="FormatException">文件语法出错</exception>
	public bool Read() => tokenizer.TokenType switch
	{
		RegTokenType.None => ReadFirst(),

		RegTokenType.CreateKey or
		RegTokenType.DeleteKey or
		RegTokenType.Value or
		RegTokenType.DeleteValue => ReadTopLevel(),

		_ => throw new FormatException("Unexcepted Token"),
	};

	/// <summary>
	/// 文件最开始的位置，首先是版本行，然后下一个句子必须是键。
	/// </summary>
	bool ReadFirst()
	{
		// Tokenizer 在第一行不是正确的版本号时会出异常。
		tokenizer.Read();

		while (tokenizer.Read())
		{
			switch (tokenizer.TokenType)
			{
				case RegTokenType.CreateKey:
				case RegTokenType.DeleteKey:
					GetKey();
					return true;
				case RegTokenType.Comment:
					break;
				default:
					throw new FormatException();
			}
		}

		return false; // 没读到可以返回的词文件就结束了。
	}

	bool ReadTopLevel()
	{
		while (tokenizer.Read())
		{
			switch (tokenizer.TokenType)
			{
				case RegTokenType.Name:
					GetValue();
					return true;
				case RegTokenType.CreateKey:
				case RegTokenType.DeleteKey:
					GetKey();
					return true;
				case RegTokenType.Comment:
					break;
				default:
					throw new FormatException();
			}
		}
		return false;
	}

	void GetKey()
	{
		IsKey = true;
		IsDelete = tokenizer.TokenType == RegTokenType.DeleteKey;
		Key = tokenizer.Value;
	}

	void GetValue()
	{
		Name = tokenizer.Value;
		IsKey = false;

		TryGetKind();

		switch (tokenizer.TokenType)
		{
			case RegTokenType.ValuePart:
				GetMultipartValue();
				break;
			case RegTokenType.Value:
				IsDelete = false;
				Value = ParseValue(tokenizer.Value, Kind);
				break;
			case RegTokenType.DeleteValue:
				IsDelete = true;
				break;
			default:
				throw new FormatException();
		}
	}

	/// <summary>
	/// 值名后面有两种情况：类型或字符串值，在这里读取并检查。
	/// </summary>
	void TryGetKind()
	{
		if (!tokenizer.Read())
		{
			throw new FormatException("缺少值或类型");
		}

		if (tokenizer.TokenType == RegTokenType.Kind)
		{
			Kind = ParseKind(tokenizer.Value);

			if (!tokenizer.Read())
			{
				throw new FormatException("缺少值");
			}
		}
		else
		{
			Kind = RegistryValueKind.String;
		}
	}

	void GetMultipartValue()
	{
		var buffer = new StringBuilder();
		buffer.Append(tokenizer.Value);

		while (tokenizer.Read())
		{
			switch (tokenizer.TokenType)
			{
				case RegTokenType.Comment:
					break;
				case RegTokenType.ValuePart:
					buffer.Append(tokenizer.Value);
					break;
				case RegTokenType.Value:
					goto SearchEnd;
				default:
					throw new FormatException();
			}
		}

		throw new FormatException("文件不完整");

	SearchEnd:

		buffer.Append(tokenizer.Value);
		IsDelete = false;
		Value = ParseValue(buffer.ToString(), Kind);
	}

	/// <summary>
	/// 将 Reg 文件中的值类型描述转换为对应的 RegistryValueKind 值。
	/// </summary>
	/// <param name="value">类型描述</param>
	/// <exception cref="FormatException">如果不能转换</exception>
	RegistryValueKind ParseKind(string value) => value switch
	{
		"dword" => RegistryValueKind.DWord,
		"hex" => RegistryValueKind.Binary,
		"hex(2)" => RegistryValueKind.ExpandString,
		"hex(7)" => RegistryValueKind.MultiString,
		"hex(b)" => RegistryValueKind.QWord,
		_ => throw new FormatException("未知的值类型: " + value),
	};

	/// <summary>
	/// 将 Reg 文件里的值文本转换为指定的类型，与 Registry.GetValue 返回的一致。
	/// </summary>
	/// <param name="text">值文本</param>
	/// <param name="kind">类型</param>
	/// <returns>转换后的值</returns>
	object ParseValue(string text, RegistryValueKind kind)
	{
		byte[] ToBytes() => text.Split(',')
			.Select(x => byte.Parse(x, NumberStyles.HexNumber))
			.ToArray();

		string ToString() => Encoding.Unicode.GetString(ToBytes()).Trim('\0');

		return kind switch
		{
			RegistryValueKind.ExpandString => ToString(),
			RegistryValueKind.MultiString => ToString().Split('\0'),
			RegistryValueKind.Binary => ToBytes(),
			RegistryValueKind.DWord => int.Parse(text, NumberStyles.HexNumber),
			RegistryValueKind.QWord => BitConverter.ToInt64(ToBytes(), 0),
			RegistryValueKind.String => text,
			_ => throw new InvalidProgramException("不会运行到这一句"),
		};
	}
}
