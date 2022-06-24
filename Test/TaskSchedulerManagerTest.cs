using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskScheduler;

namespace Win11Tuned.Test;

[TestClass]
public sealed class TaskSchedulerManagerTest
{
	void CreateTestTasks(string path)
	{
		var task = TaskSchedulerManager.Instance.NewTask(0);
		var action = (IExecAction)task.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
		action.Id = "id";
		action.Path = "cmd.exe";

		// 目录会自动创建
		TaskSchedulerManager.Root.RegisterTaskDefinition(
			path,
			task,
			(int)_TASK_CREATION.TASK_CREATE,
			null,
			null,
			_TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN);
	}

	[TestMethod]
	public void InitRoot()
	{
		var windows = TaskSchedulerManager.Root.GetFolder(@"Microsoft\Windows");
		Assert.IsTrue(windows.GetFolders(0).Count > 50);
	}

	[TestMethod]
	public void GetEnabled()
	{
		var task = TaskSchedulerManager.Root.GetTask(@"Microsoft\Windows\Chkdsk\ProactiveScan");
		Assert.IsTrue(task.Enabled);
	}

	[TestMethod]
	public void DeleteTask()
	{
		CreateTestTasks("TestTask");
		Assert.IsTrue(TaskSchedulerManager.DeleteTask(@"TestTask"));
		Assert.IsFalse(TaskSchedulerManager.DeleteTask(@"TestTask"));
	}

	[TestMethod]
	public void ClearFolder()
	{
		CreateTestTasks(@"Test\SubFolder\测试任务");
		TaskSchedulerManager.ClearFolder("Test");

		try
		{
			TaskSchedulerManager.Root.GetTask(@"Test\SubFolder\测试任务");
			Assert.Fail("Expect to throw exception");
		}
		catch (IOException e)
		when (e is DirectoryNotFoundException || e is FileNotFoundException)
		{
			// Expect task is not exists.
		}
		finally
		{
			TaskSchedulerManager.Root.DeleteFolder(@"Test\SubFolder", 0);
			TaskSchedulerManager.Root.DeleteFolder(@"Test", 0);
		}
	}
}
