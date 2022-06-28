using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;
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
		};

		var appx = new AppxRuleSet();
		appx.Add("Microsoft.549981C3F5F10"); // Cortana
		appx.Add("Microsoft.BingNews");
		appx.Add("Microsoft.BingWeather");
		appx.Add("Microsoft.GamingApp");
		appx.Add("Microsoft.GetHelp");
		appx.Add("Microsoft.Getstarted");
		appx.Add("Microsoft.Messaging");
		appx.Add("Microsoft.Microsoft3DViewer*");
		appx.Add("Microsoft.MicrosoftOfficeHub");
		appx.Add("Microsoft.MicrosoftStickyNotes");
		appx.Add("Microsoft.MicrosoftSolitaireCollection");
		appx.Add("Microsoft.NetworkSpeedTest");
		appx.Add("Microsoft.Office.Sway");
		appx.Add("Microsoft.OneConnect");
		appx.Add("Microsoft.People");
		appx.Add("Microsoft.PowerAutomateDesktop"); // Require login
		appx.Add("Microsoft.Print3D");
		appx.Add("Microsoft.ScreenSketch");
		appx.Add("Microsoft.SkypeApp");
		appx.Add("Microsoft.Todos");
		appx.Add("Microsoft.WindowsAlarms");
		appx.Add("Microsoft.WindowsCamera");
		appx.Add("microsoft.windowscommunicationsapps");
		appx.Add("Microsoft.WindowsFeedbackHub");
		appx.Add("Microsoft.WindowsMaps");
		appx.Add("Microsoft.WindowsSoundRecorder");
		appx.Add("Microsoft.Xbox.TCUI");
		appx.Add("Microsoft.XboxApp");
		appx.Add("Microsoft.XboxGameOverlay");
		appx.Add("Microsoft.XboxGamingOverlay");
		appx.Add("Microsoft.XboxIdentityProvider");
		appx.Add("Microsoft.XboxSpeechToTextOverlay");
		appx.Add("Microsoft.YourPhone"); // Huawei phones only
		appx.Add("Microsoft.ZuneMusic");
		appx.Add("Microsoft.ZuneVideo");
		appx.Add("MicrosoftWindows.Client.WebExperience");

		appx.Add("AD2F1837.HPSystemInformation");
		RuleSets.Add(appx);

		if (AdminMode)
		{
			others.Add(new PowerShellPolicyRule());
			others.Add(new ExplorerFolderRule());
			others.Add(new RegFileRule(
				"把用记事本打开添加到右键菜单",
				"很常用的功能，不解释",
				GetEmbeddedRegFile("OpenWithNotepad")
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

			folders = RegHelper.Search(root, item)
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

	static Rule ReadGroupPolicy(RuleFileReader reader)
	{
		return new GroupPolicyRule(reader.Read(), reader.Read(), reader.Read(), reader.Read(), reader.Read());
	}
}
