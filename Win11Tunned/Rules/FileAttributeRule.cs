using System;
using System.IO;

namespace Win11Tunned.Rules;

public sealed class FileAttributeRule : Rule
{
	readonly string path;
	readonly FileAttributes attributes;

	public string Name { get; }

	public string Description { get; }

	public FileAttributeRule(
		string path,
		FileAttributes attributes,
		string name,
		string description)
	{
		this.path = path;
		this.attributes = attributes;
		Name = name;
		Description = description;
	}

	public bool NeedOptimize()
	{
		try
		{
			return File.GetAttributes(path) != attributes;
		}
		catch (Exception ex)
		when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
		{
			// path 可以是文件或目录，但 FileInfo.Exists 除了检查存在外还限制了须是文件，
			// DirectoryInfo 同理，FileSystemInfo 还是私有的，没有一个公共类，
			// 导致了要检查路径存在还得判断两次，异常也有两种。
			// 这是一个很明显的设计错误，C# 开发者的 API 设计水平比其它语言低了不止一个档次。
			return false;
		}
	}

	public void Optimize()
	{
		File.SetAttributes(path, attributes);
	}
}
