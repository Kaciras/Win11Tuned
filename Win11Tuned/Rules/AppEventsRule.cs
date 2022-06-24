using System;

namespace Win11Tuned.Rules;

/// <summary>
/// 优化 控制面板/硬件和声音/更改系统声音/声音方案 的规则，如果当前方案与目标不同则修改为目标方案。
/// </summary>
public sealed class AppEventsRule : Rule
{
	const string ROOT = @"HKCU\AppEvents\Schemes";

	public string Name { get; }

	public string Description { get; }

	readonly string target;

	/// <summary>
	/// 创建规则的实例，将声音方案调整为制定的项，方案名是注册表里的键名比如".Default"、".None"。
	/// </summary>
	/// <param name="target">方案名</param>
	public AppEventsRule(string target)
	{
		this.target = target;

		using var schemes = RegHelper.OpenKey(@$"{ROOT}\Names\{target}");
		var name = (string)schemes.GetValue("");
		try
		{
			name = Utils.ExtractStringFromDLL(name);
		}
		catch (FormatException)
		{
			// 坑爹！有的用户直接就是名字，内置管理员则是一个资源引用。
		}

		Name = "设置系统音效为：" + name;
		Description = "将 Windows 系统声音方案设置为：" + name;
	}

	/// <summary>
	/// 因为本工具默认用户不会乱改导致不一致的情况，所以只检查预设名即可。
	/// </summary>
	public bool NeedOptimize()
	{
		using var schemes = RegHelper.OpenKey(ROOT);
		return !schemes.GetValue("").Equals(target);
	}

	public void Optimize()
	{
		using var schemes = RegHelper.OpenKey(ROOT, true);

		schemes.SetValue("", target);

		using var apps = schemes.OpenSubKey("Apps");
		foreach (var appName in apps.GetSubKeyNames())
		{
			using var app = apps.OpenSubKey(appName);
			foreach (var item in app.GetSubKeyNames())
			{
				using var key = app.OpenSubKey(@$"{item}\{target}");
				if (key == null) continue;

				using var current = app.OpenSubKey(@$"{item}\.Current", true);
				current.SetValue(string.Empty, key.GetValue(""));
			}
		}
	}
}
