using Microsoft.Win32;

namespace Win11Tunned.Rules;

/// <summary>
/// 这个虽然也是设置注册表，但跟 RegFileRule 不同的是需要做一些计算，所以单独写个规则。
/// </summary>
public class WMILoggerRule : Rule
{
	public string Name { get; }

	public string Description { get; }

	readonly string key;

	readonly bool? cycle;
	readonly int? maxFileSize;

	int? fileModeTarget;
	int? fileSizeTarget;

	public WMILoggerRule(string name, string description, string key, bool? cycle, int? maxFileSize)
	{
		Name = name;
		Description = description;
		this.key = key;
		this.cycle = cycle;
		this.maxFileSize = maxFileSize;
	}

	public bool NeedOptimize()
	{
		if (cycle.HasValue)
		{
			var e = Registry.GetValue(key, "LogFileMode", 0);
			if (e == null) return false;
			var mode = (int)e;
			if ((mode & 2) == 0)
			{
				fileModeTarget = mode | 2;
			}
		}

		if (maxFileSize.HasValue)
		{
			var size = (int)Registry.GetValue(key, "MaxFileSize", 0);
			if (maxFileSize.Value > size)
			{
				fileSizeTarget = maxFileSize;
			}
		}

		return fileModeTarget.HasValue || fileSizeTarget.HasValue;
	}

	public void Optimize()
	{
		if (fileModeTarget.HasValue)
		{
			Registry.SetValue(key, "LogFileMode", fileModeTarget, RegistryValueKind.DWord);
		}
		if (fileSizeTarget.HasValue)
		{
			Registry.SetValue(key, "MaxFileSize", fileSizeTarget, RegistryValueKind.DWord);
		}
	}
}
