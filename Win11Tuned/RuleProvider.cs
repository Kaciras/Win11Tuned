using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;
using RegistryEx;
using Win11Tuned.Properties;
using Win11Tuned.Rules;

namespace Win11Tuned;

/// <summary>
/// 统一管理优化项的类，在启动时载入所有的优化规则。
/// </summary>
public sealed class RuleProvider
{
	public ICollection<OptimizableSet> RuleSets { get; } = new List<OptimizableSet>();

	/// <summary>
	/// 是否载入只有管理员才能设置的优化项，对这些项目的优化需要管理员权限。
	/// </summary>
	public bool AdminMode { get; }

	public RuleProvider(bool adminMode)
	{
		AdminMode = adminMode;
	}

	internal void Initialize()
	{
		LoadRuleFile("系统设置（用户）", Resources.UserRegistryRules, ReadRegistry);
		RuleSets.Add(new SendToRuleSet());

		RuleSets.Add(new RuleList("显示桌面图标", new Rule[] {
			// {018D5C66-4533-4307-9B53-224DE2ED1FE6} - OneDrive
			new DesktopIconRule("{20D04FE0-3AEA-1069-A2D8-08002B30309D}"),
			new DesktopIconRule("{5399E694-6CE5-4D6C-8FCE-1D8870FDCBA0}"),
			new DesktopIconRule("{59031a47-3f72-44a7-89c5-5595fe6b30ee}"),
			new DesktopIconRule("{645FF040-5081-101B-9F08-00AA002F954E}"),
			new DesktopIconRule("{F02C1A0D-BE21-4350-88B0-7367FC96EF3C}"),
		}));

		var others = new List<Rule> {
			new AppEventsRule(".None"),
			new FileAttributeRule(
				Environment.ExpandEnvironmentVariables("%USERPROFILE%/AppData"),
				FileAttributes.Directory,
				"取消 AppData 的隐藏属性",
				"很多程序把配置和数据都存在这个目录，经常需要直接访问它，故取消隐藏"
			),
			new FileAttributeRule(
				Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
				FileAttributes.Directory | FileAttributes.NotContentIndexed,
				"取消 ProgramData 的隐藏属性",
				"很多程序把配置和数据都存在这个目录，经常需要直接访问它，故取消隐藏"
			),
		};

		var appx = new AppxRuleSet();
		//appx.Add("Microsoft.WindowsCamera");
		//appx.Add("microsoft.windowscommunicationsapps"); // Mail and Calender
		//appx.Add("Microsoft.WindowsSoundRecorder");
		appx.Uninstall("Microsoft.549981C3F5F10"); // Cortana
		appx.Uninstall("Microsoft.BingNews");
		appx.Uninstall("Microsoft.BingWeather");
		appx.Uninstall("Microsoft.GamingApp");
		appx.Uninstall("Microsoft.GetHelp");
		appx.Uninstall("Microsoft.Getstarted");
		appx.Uninstall("Microsoft.Messaging");
		appx.Uninstall("Microsoft.Microsoft3DViewer*");
		appx.Uninstall("Microsoft.MicrosoftOfficeHub");
		appx.Uninstall("Microsoft.MicrosoftStickyNotes");
		appx.Uninstall("Microsoft.MicrosoftSolitaireCollection");
		appx.Uninstall("Microsoft.NetworkSpeedTest");
		appx.Uninstall("Microsoft.Office.Sway");
		appx.Uninstall("Microsoft.OneConnect");
		appx.Uninstall("Microsoft.People");
		appx.Uninstall("Microsoft.PowerAutomateDesktop"); // Require login
		appx.Uninstall("Microsoft.Print3D");
		appx.Uninstall("Microsoft.ScreenSketch");
		appx.Uninstall("Microsoft.SkypeApp");
		appx.Uninstall("Microsoft.Todos");
		appx.Uninstall("Microsoft.WindowsAlarms");
		appx.Uninstall("Microsoft.WindowsFeedbackHub");
		appx.Uninstall("Microsoft.WindowsMaps");
		appx.Uninstall("Microsoft.Xbox.TCUI");
		appx.Uninstall("Microsoft.XboxApp");
		appx.Uninstall("Microsoft.XboxGameOverlay");
		appx.Uninstall("Microsoft.XboxGamingOverlay");
		appx.Uninstall("Microsoft.XboxIdentityProvider");
		appx.Uninstall("Microsoft.XboxSpeechToTextOverlay");
		appx.Uninstall("Microsoft.YourPhone"); // Huawei phones only
		appx.Uninstall("Microsoft.ZuneMusic");
		appx.Uninstall("Microsoft.ZuneVideo");
		appx.Uninstall("Microsoft.MicrosoftOfficeHub");
		appx.Uninstall("MicrosoftWindows.Client.WebExperience");

		appx.Uninstall("Clipchamp.Clipchamp");
		appx.Uninstall("AD2F1837.HPSystemInformation");
		RuleSets.Add(appx);

		var startupUser = new StartupRuleSet(false);
		startupUser.Add("^MicrosoftEdgeAutoLaunch");
		RuleSets.Add(startupUser);

		var startup = new StartupRuleSet(true);
		startup.Add("^RtkAudUService$");
		startup.Add("^SecurityHealth$");
		RuleSets.Add(startup);

		var userSoftware = new SoftwareRuleSet(false);
		userSoftware.Add("OneDriveSetup.exe");
		RuleSets.Add(userSoftware);

		if (AdminMode)
		{
			var systemSoftware = new SoftwareRuleSet(true);
			// 这个 ID 还会变，上一版是 6A2A8076-135F-4F55-BB02-DED67C8C6934
			systemSoftware.Add("{AF47B488-9780-4AB5-A97E-762E28013CA6}"); // Microsoft Update Health Tools
			RuleSets.Add(systemSoftware);

			others.Add(new PowerShellPolicyRule());
			others.Add(new ExplorerFolderRule());
			others.Add(new RegFileRule(
				"把用记事本打开添加到右键菜单",
				"很常用的功能，不解释",
				GetEmbeddedRegFile("OpenWithNotepad")
			));
			others.Add(new RegFileRule(
				"把注销 DLL/OCX 组件 添加到右键菜单",
				"方便地注销各种 Shell 扩展",
				GetEmbeddedRegFile("UnregisterDLL")
			));

			others.Add(new FileAttributeRule(
				Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
				FileAttributes.Directory | FileAttributes.ReadOnly,
				"取消公共桌面的隐藏属性",
				"常用的目录，不知为何微软要隐藏它"
			));

			var cdd = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
			others.Add(new FileAttributeRule(
				Path.Combine(Path.GetDirectoryName(cdd), "Libraries"),
				FileAttributes.Directory | FileAttributes.ReadOnly,
				"取消公共库的隐藏属性",
				"常用的目录，不知为何微软要隐藏它"
			));

			RuleSets.Add(new TaskSchedulerSet());

			LoadRuleFile("组策略", Resources.GroupPolicyRules, ReadGroupPolicy);
			LoadRuleFile("右键菜单清理", Resources.ContextMenuRules, ReadContextMenu);
			LoadRuleFile("服务", Resources.ServiceRules, ReadService);
			LoadRuleFile("系统设置", Resources.RegistryRules, ReadRegistry);
		}

		RuleSets.Add(new RuleList("其它优化项", others));
	}

	void LoadRuleFile(string name, string content, Func<RuleFileReader, Rule> func)
	{
		var rules = RuleFileReader.Iter(content).Select(func).ToList();
		RuleSets.Add(new RuleList(name, rules));
	}

	// 下面是各种规则的加载逻辑，为了省点字把 Rule 后缀去掉了（ReadTaskRule -> ReadTask）

	static Rule ReadRegistry(RuleFileReader reader)
	{
		return new RegFileRule(reader.Read(), reader.Read(), GetEmbeddedRegFile(reader.Read()));
	}

	static string GetEmbeddedRegFile(string name)
	{
		name = $"Win11Tuned.Resources.Registry.{name}.reg";
		var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}

	static Rule ReadService(RuleFileReader reader)
	{
		return new ServiceRule(reader.Read(), reader.Read(),
			(ServiceState)Enum.Parse(typeof(ServiceState), reader.Read()));
	}

	static Rule ReadContextMenu(RuleFileReader reader)
	{
		var item = reader.Read();
		var name = reader.Read();
		var description = reader.Read();
		var directive = reader.Read();

		IList<string> folders;

		if (directive == ":SEARCH")
		{
			var folder = reader.Read();
			var root = Path.Combine("HKEY_CLASSES_ROOT", folder);

			folders = Search(root, item)
				.Select(name => Path.Combine(folder, name))
				.ToList();
		}
		else
		{
			folders = reader.Drain().ToList();
			folders.Add(directive);
		}

		return new ContextMenuRule(item, folders, name, description);
	}

	/// <summary>
	/// 在指定的目录中搜索含有某个路径的项，只搜一层。
	/// <br/>
	/// 因为 rootKey 会销毁，必须在离开作用域前遍历完，所以返回IList。
	/// </summary>
	/// <param name="root">在此目录中搜索</param>
	/// <param name="key">要搜索的键路径</param>
	/// <returns>子项名字列表</returns>
	public static List<string> Search(string root, string key)
	{
		using var rootKey = RegistryHelper.OpenKey(root);
		return rootKey.GetSubKeyNames()
			.Where(name => rootKey.ContainsSubKey(Path.Combine(name, key)))
			.ToList();
	}

	static Rule ReadGroupPolicy(RuleFileReader reader)
	{
		return new GroupPolicyRule(reader.Read(), reader.Read(), reader.Read(), reader.Read(), reader.Read());
	}
}
