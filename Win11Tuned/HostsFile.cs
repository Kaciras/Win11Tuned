using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Win11Tuned;

// 简单的解析和保存 hosts 文件格式的类，尽量保持了原本的注释和空行。
public sealed class HostsFile
{
	readonly MultiValueDictionary<string, (string, int)> entries = new();
	readonly List<string?> lines = [];

	public HostsFile() {}

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
			for (int i = 1; i < parts.Length; i++)
			{
				var tuple = (parts[0], lines.Count);
				entries.Add(parts[i], tuple);
			}
			lines.Add(line);
			line = reader.ReadLine();
		}
	}

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
		if (!entries.ContainsKey(host))
		{
			return false;
		}
		var ips = entries[host];
		return ips.Count == 1 && ips[0].Item1 == ip;
	}

	public void RemoveAll(string host)
	{
		if (!entries.ContainsKey(host))
		{
			return;
		}
		var ips = entries[host];
		entries.Remove(host);

		foreach (var (_, i) in ips)
		{
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
		foreach (var kv in entries)
		{
			yield return (kv.Key, kv.Value.Item1);
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
