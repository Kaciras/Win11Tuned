using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Win11Tuned.Properties;
using static System.Environment;

namespace Win11Tuned.Rules;

/// <summary>
/// 清理当前用户右键菜单里的 “发送到” 一栏。
/// </summary>
public class SendToRuleSet : OptimizableSet
{
	readonly string folder;

	public string Name => "右键菜单 - 发送到";

	public SendToRuleSet()
	{
		var appRoaming = GetFolderPath(SpecialFolder.ApplicationData);
		folder = Path.Combine(appRoaming, @"Microsoft\Windows\SendTo");
	}

	public IEnumerable<Optimizable> Scan()
	{
		var cleanInList = RuleFileReader.Iter(Resources.SendToRules)
			.Select(DeleteByRuleList)
			.Where(item => item != null);

		var cleanInvalidLinks = Directory.GetFiles(folder)
			.Where(path => Path.GetExtension(path) == ".lnk")
			.Where(path => !File.Exists(Utils.GetShortcutTarget(path)))
			.Select(DeleteInvalidLink);

		return Enumerable.Concat(cleanInList, cleanInvalidLinks);
	}

	Optimizable DeleteByRuleList(RuleFileReader reader)
	{
		var filename = reader.Read();
		var path = Path.Combine(folder, filename);
		if (!File.Exists(path))
		{
			return null;
		}
		var localName = LocalizedName(filename);
		return new OptimizeAction(localName, reader.Read(), () => File.Delete(path));
	}

	Optimizable DeleteInvalidLink(string path)
	{
		var localName = LocalizedName(Path.GetFileName(path));
		return new OptimizeAction(localName, "该项是无效的链接", () => File.Delete(path));
	}

	string LocalizedName(string name)
	{
		var desktopIni = new SimpleIniFile(Path.Combine(folder, "desktop.ini"));
		var localized = desktopIni.Read("LocalizedFileNames", name, string.Empty);

		if (localized == string.Empty)
		{
			return Path.GetFileNameWithoutExtension(name);
		}
		if (localized[0] != '@')
		{
			return localized;
		}
		try
		{
			return Utils.ExtractStringResource(localized);
		}
		catch (SystemException)
		{
			// 资源文件可能不存在，回退到文件名
			return Path.GetFileNameWithoutExtension(name);
		}
	}
}
