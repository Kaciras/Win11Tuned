using System.Linq;
using Microsoft.Win32;
using RegistryEx;

namespace Win11Tuned.Rules;

public sealed class ExplorerFolderRule : Rule
{
	// HKEY_LOCAL_MACHINE
	const string KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace";

	readonly string[] clsids = [
		"{088e3905-0323-4b02-9826-5d99428e115f}", // Download
		"{24ad3ad4-a569-4530-98e1-ab02f9417aa8}", // Pictures
		"{3dfdf296-dbec-4fb4-81d1-6a3438bcf4de}", // Music
		"{d3162b92-9365-467a-956b-92703aca08af}", // Documents
		"{f86fa3ab-70d2-4fc7-9c99-fcbf05467f3a}", // Videos
	];

	public string Name => "删除我的电脑界面上的 5 个文件夹";

	public string Description => "就是视频、文档、下载这些，它们可以从用户目录里打开,放在我的电脑里只会占位置，而且我不咋用";

	public bool NeedOptimize()
	{
		using var nameSpage = Registry.LocalMachine.OpenSubKey(KEY);
		return clsids.Any(nameSpage.ContainsSubKey);
	}

	// 注册表 32 跟 64 位存储是分开的，系统自带的注册表编辑器能同时操作两者，但 C# 的 API 不能。
	// 如果架构对不上，则会出现找不到 key 的情况。
	public void Optimize()
	{
		using var nameSpage = Registry.LocalMachine.OpenSubKey(KEY, true);
		clsids.ForEach(nameSpage.DeleteSubKeyTree);
	}
}
