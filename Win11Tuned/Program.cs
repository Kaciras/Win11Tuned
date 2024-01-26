using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using RegistryEx;

[assembly: InternalsVisibleTo("Test")]
[assembly: InternalsVisibleTo("Benchmark")]
namespace Win11Tuned;

/*
 * 为了实现的简洁，本程序假设用户的系统满足以下条件：
 * 1）没有手动修改过注册表等底层数据，只用系统提供的控制中心调整配置，这避免了各种边界情况。
 * 2）在扫描和优化两个操作之间不对系统设置做修改，这避免了不一致的状态。
 */
static class Program
{
	[STAThread]
	static void Main()
	{
		Application.SetCompatibleTextRenderingDefault(false);
		Application.EnableVisualStyles();
		Application.Idle += CaptureSyncContext;

		RegistryHelper.AddTokenPrivileges();

		var isAdmin = Utils.CheckIsAdministrator();
		var provider = new RuleProvider(isAdmin);
		provider.Initialize();

		Application.Run(new MainWindow(provider));
	}

	/// <summary>
	/// 初始化 STAExecutor，直接复用 WinForm 的同步上下文。
	/// </summary>
	private static void CaptureSyncContext(object sender, EventArgs e)
	{
		Application.Idle -= CaptureSyncContext;
		STAExecutor.SetSyncContext(SynchronizationContext.Current);
	}
}
