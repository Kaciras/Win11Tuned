using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Win11Tuned.Test;

/// <summary>
/// 为了方便把 STAExecutor 做成了静态类，所以需要在全局初始化一下。
/// <br/>
/// 因为 MSTest 的主线程就是 STA，所以直接创建一个 WindowsFormsSynchronizationContext 使用当前线程即可。
/// </summary>
[TestClass]
public sealed class GlobalSetup
{
	[AssemblyInitialize]
	public static void Setup(TestContext _)
	{
		STAExecutor.SetSyncContext(new WindowsFormsSynchronizationContext());
	}
}
