using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Win11Tunned.Test;

[TestClass]
public sealed class STAExecutorTest
{
	// STAExecutor 是全局的，如果该测试失败可能影响到其他测试。
	[ExpectedException(typeof(ArgumentException))]
	[TestMethod]
	public void SetSyncContext()
	{
		STAExecutor.SetSyncContext(new ThreadSyncContext());
	}

	[TestMethod]
	public void Run()
	{
		var state = ApartmentState.Unknown;
		STAExecutor.Run(() =>
		{
			state = Thread.CurrentThread.GetApartmentState();
		});
		Assert.AreEqual(ApartmentState.STA, state);
	}

	class ThreadSyncContext : SynchronizationContext
	{
		public override void Send(SendOrPostCallback action, object state)
		{
			TestHelper.RunInNewThread(() => action(state));
		}
	}
}
