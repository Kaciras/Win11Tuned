using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Win11Tuned.Test;

[TestClass]
public sealed class GroupPolicyTest
{
	// 单元测试的线程也是STA，需要手动创建新的
	[TestMethod]
	public void NonSTAThread()
	{
		static void action() => TestHelper.RunInNewThread(() => new ComputerGroupPolicyObject());
		Assert.ThrowsException<Exception>(action, "GPO can only be accessed in STA thread");
	}

	// 其他方法不好测，都依赖系统环境或有副作用
}
