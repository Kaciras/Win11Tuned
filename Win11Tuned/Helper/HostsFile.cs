﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Win11Tuned;

/// <summary>
/// 简单的解析和保存 hosts 文件格式的类，尽量保持了原本的注释和空行。
/// </summary>
public sealed class HostsFile
{
	const int MIN_HOSTS_START_COLUMN = 20;

	// 主机名 -> (IP，行号)
	readonly MultiDictionary<string, (string, int)> entries = [];

	// 因为记录了索引，所以不能移动元素，删除的设为 null。
	readonly List<string> lines = [];

	public HostsFile() { }

	public HostsFile(string path)
	{
		using var reader = new StreamReader(path);
		Load(reader);
	}

	public void Load(StreamReader reader)
	{
		var line = reader.ReadLine();
		while (line != null)
		{
			Add(line);
			line = reader.ReadLine();
		}
	}

	/// <summary>
	/// 分割注释之前的部分，第一个是 IP，后面的是主机名。
	/// </summary>
	/// <example>
	/// SplitLine("::1 example.com v6.local #Comments");
	/// // ["::1", "example.com", "v6.local"]
	/// </example>
	static string[] SplitLine(string line)
	{
		var e = line.IndexOf('#');
		if (e != -1)
		{
			line = line.Substring(0, e);
		}
		return line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
	}

	/// <summary>
	/// 判断是否指定的主机名是否仅有一条映射，且为指定的 IP。
	/// </summary>
	/// <param name="host">主机名</param>
	/// <param name="ip">IP 地址</param>
	public bool ContainsExactly(string host, string ip)
	{
		if (!entries.TryGetValue(host, out var list))
		{
			return false;
		}
		return list.Count == 1 && list[0].Item1 == ip;
	}

	/// <summary>
	/// 从该 Hosts 文件中删除所有指定主机名的映射。
	/// </summary>
	/// <param name="host">主机名</param>
	public void RemoveAll(string host)
	{
		if (!entries.TryGetValue(host, out var ips))
		{
			return;
		}
		entries.Remove(host);
		foreach (var (_, i) in ips)
		{
			// 会把注释里的也给删除，但应该不是什么大问题。
			var x = lines[i].Replace(host, "");
			lines[i] = SplitLine(x).Length == 1 ? null : x;
		}
	}

	public void AddEmptyLine()
	{
		lines.Add(string.Empty);
	}

	/// <summary>
	/// 添加一行记录，主机名和 IP 将从字符串中解析。
	/// </summary>
	public void Add(string line)
	{
		var parts = SplitLine(line);
		for (var i = 1; i < parts.Length; i++)
		{
			var tuple = (parts[0], lines.Count);
			entries.Add(parts[i], tuple);
		}
		lines.Add(line);
	}

	/// <summary>
	/// 添加一条记录，未检查重复，新记录位于单独的一行，IP 和 host 间以空格分隔。
	/// <br/>
	/// 该方法会调整空格数量（最多 20 个）以求对齐，但这还取决于你的字体是否等宽。
	/// </summary>
	public void Add(string host, string ip)
	{
		entries.Add(host, (ip, lines.Count));

		var n = MIN_HOSTS_START_COLUMN - ip.Length;
		n = Math.Max(1, n);
		lines.Add(ip + new string(' ', n) + host);
	}

	public IEnumerable<(string, string)> Entries()
	{
		foreach (var pair in entries)
		{
			foreach (var (ip, _) in pair.Value)
			{
				yield return (pair.Key, ip);
			}
		}
	}

	IEnumerable<string> ValidLines => lines.Where(x => x != null);

	public void WriteTo(string path)
	{
		File.WriteAllLines(path, ValidLines);
	}

	public override string ToString()
	{
		return string.Join("\r\n", ValidLines.Concat([""]));
	}
}
