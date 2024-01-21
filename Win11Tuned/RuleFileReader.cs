using System;
using System.Collections;
using System.Collections.Generic;

namespace Win11Tuned;

/// <summary>
/// 一大片规则写在代码里看着不舒服，所以就单独提出来一个文本文件放在资源里，本类专门用于读取这些文本文件。
/// <br/>
/// 规则文件是一种以行为单位，低冗余的紧凑格式，具有以下特点：
/// <list>
/// <item>只能用LF换行符</item>
/// <item>使用空行分隔项目，项目内不允许空行</item>
/// <item>最后一行结尾也必须有换行符</item>
/// <item>按行读取，上层怎么解析随意</item>
/// </list>
/// </summary>
public sealed class RuleFileReader(string content) : IEnumerator<RuleFileReader>
{
	int i = 0;

	/// <summary>
	/// 反正都读取的是预定义的资源，限制死换行符可以避免一些的麻烦。
	/// 
	/// 该函数返回异常而不是抛出，调用方使用 throw 这样不会破坏控制流分析。
	/// </summary>
	Exception CR() => new ArgumentException("规则文件只能用 LF 换行");

	/// <summary>
	/// 跳过空白和注释行，准备读取新的条目。
	/// <br/>
	/// 调用该方法前必须使用 Read() 或 Drain() 读取完当前的项目，否则无法前进到新项目。
	/// </summary>
	/// <returns>如果读完则为false，否则返回true</returns>
	public bool MoveNext()
	{
		for (; i < content.Length; i++)
		{
			switch (content[i])
			{
				case '\r':
					throw CR();
				case '#':
					i = content.IndexOf('\n', i);
					break;
				case '\n':
				case '\t':
				case ' ':
					break;
				default:
					return true;
			}
		}
		return false; // 文件读完了
	}

	/// <summary>
	/// Read() 的枚举形式，迭代返回项目的每一行，在项目读完后终止。
	/// </summary>
	public IEnumerable<string> Drain()
	{
		var line = Read();
		while (line != string.Empty)
		{
			yield return line;
			line = Read();
		}
	}

	/// <summary>
	/// 读取一行，请先使用 MoveNext() 确保读取位置在有效行上。
	/// </summary>
	/// <returns>一行内容</returns>
	public string Read()
	{
		var j = i;
		var k = j;

		for (; k < content.Length; k++)
		{
			switch (content[k])
			{
				case '\r':
					throw CR();
				case '\n':
					goto SearchEnd;
			}
		}

	SearchEnd:

		if (k > content.Length)
		{
			return string.Empty;
		}

		i = k + 1;
		return content.Substring(j, k - j);
	}

	/// <summary>
	/// RuleFileReader 支持枚举模式，迭代每一个项目，这样就能利用 LINQ 函数。
	/// <br/>
	/// 注意与规范不同的是 MoveNext() 前必须读完当前项目，否则无法前进到新项目。
	/// </summary>
	public static IEnumerable<RuleFileReader> Iter(string content)
	{
		return new JustEnumerable(content);
	}

	#region 实现 IEnumerator...

	public RuleFileReader Current => this;
	object IEnumerator.Current => Current;
	public void Dispose() { }
	public void Reset() => Current.i = 0;

	#endregion

	// 还可以分为两层：每一项和每一行，这样的话暴露的方法就更少，不过我懒得搞叻。
	class JustEnumerable(string content) : IEnumerable<RuleFileReader>
	{
		readonly string content = content;

		public IEnumerator<RuleFileReader> GetEnumerator()
		{
			return new RuleFileReader(content);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
