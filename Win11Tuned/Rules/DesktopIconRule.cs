using Microsoft.Win32;

namespace Win11Tuned.Rules;

public sealed class DesktopIconRule(string clsid) : Rule
{
	const string KEY = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel";

	public string Name { get; private set; }

	public string Description => "在桌面上显示这个图标";

	public bool NeedOptimize()
	{
		if (clsid == "{59031a47-3f72-44a7-89c5-5595fe6b30ee}")
		{
			Name = "用户的文件";
		}
		else
		{
			using var key = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + clsid);
			Name = Utils.ExtractStringResource((string)key.GetValue("LocalizedString"));
		}

		return (int)Registry.GetValue(KEY, clsid, 1) != 0;
	}

	public void Optimize() => Registry.SetValue(KEY, clsid, 0);
}
