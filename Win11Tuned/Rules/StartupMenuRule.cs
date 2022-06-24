using System.IO;
using static System.Environment;

namespace Win11Tuned.Rules;

/// <summary>
/// 开始菜单清理规则，有些八百年用不到的程序看着就烦，安装时还不能取消创建。
/// </summary>
public sealed class StartupMenuRule : Rule
{
	readonly string path;

	public string Name { get; }

	public string Description { get; }

	public StartupMenuRule(bool isSystem, string name, string description)
	{
		Name = name;
		Description = description;
		var folder = isSystem ? SpecialFolder.CommonStartMenu : SpecialFolder.StartMenu;
		path = Path.Combine(GetFolderPath(folder), "Programs", name);
	}

	public bool NeedOptimize() => Directory.Exists(path);

	public void Optimize() => Directory.Delete(path, true);
}
