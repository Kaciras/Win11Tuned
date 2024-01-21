using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Win11Tuned;

/// <summary>
/// 简单的解析和保存 hosts 文件格式的类，尽量保持了原本的注释和空行。
/// </summary>
public sealed class HostsFile
{
	// 主机名 -> (IP，行号)
	readonly MultiDictionary<string, (string, int)> entries = new();

	// 以为记录了索引，所以不能移动元素，删除的设为 null。
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
			var parts = SplitLine(line);
			for (var i = 1; i < parts.Length; i++)
			{
				var tuple = (parts[0], lines.Count);
				entries.Add(parts[i], tuple);
			}
			lines.Add(line);
			line = reader.ReadLine();
		}
	}

	/// <summary>
	/// 分割注释之前的部分，第一个是 IP，后面的是主机名。
	/// </summary>
	static string[] SplitLine(string line)
	{
		var e = line.IndexOf('#');
		if (e != -1)
		{
			line = line.Substring(0, e);
		}
		return line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
	}

	public bool ContainsExactly(string host, string ip)
	{
		if (!entries.TryGetValue(host, out var ips))
		{
			return false;
		}
		return ips.Count == 1 && ips[0].Item1 == ip;
	}

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

	public void Add(string host, string ip)
	{
		entries.Add(host, (ip, lines.Count));
		lines.Add($"{ip}\t{host}");
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

	public void Save(string path)
	{
		File.WriteAllLines(path, ValidLines);
	}

	public override string ToString()
	{
		return string.Join("\r\n", ValidLines.Concat([""]));
	}
}
