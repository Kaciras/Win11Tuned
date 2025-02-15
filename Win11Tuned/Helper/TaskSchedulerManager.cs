using System.IO;
using System.Linq;
using TaskScheduler;

namespace Win11Tuned;

/// <summary>
/// 对 TaskScheduler 的封装，提供了一些快捷方法。
/// </summary>
public static class TaskSchedulerManager
{
	static readonly TaskScheduler.TaskScheduler taskScheduler;

	static TaskSchedulerManager()
	{
		// 必须把 Class 后缀去掉，或是使用外置 Interop Types
		// https://stackoverflow.com/a/4553402/7065321
		taskScheduler = new TaskScheduler.TaskScheduler();
		taskScheduler.Connect();
		Root = taskScheduler.GetFolder(@"\");
	}

	public static TaskScheduler.TaskScheduler Instance => taskScheduler;

	public static ITaskFolder Root { get; }

	public static IRegisteredTask? Find(string path)
	{
        try
        {
            return Root.GetTask(path);
        }
        catch (IOException e)
        when (e is DirectoryNotFoundException || e is FileNotFoundException)
        {
            return null; // Task not found.
        }
    }

    /// <summary>
    /// 从任务计划程序中删除指定的任务。
    /// </summary>
    /// <param name="path">任务路径</param>
    /// <returns>是否成功删除</returns>
    public static bool DeleteTask(string path)
	{
		try
		{
			Root.DeleteTask(path, 0);
			return true;
		}
		catch (IOException e)
		when (e is DirectoryNotFoundException || e is FileNotFoundException)
		{
			return false; // 不存在的情况有两种异常
		}
	}

	/// <summary>
	/// 清空目录中的所有任务，考虑到有些目录无法删除所以把文件夹留着。
	/// </summary>
	/// <param name="path">目录路径</param>
	public static void ClearFolder(string path)
	{
		var folder = Root.GetFolder(path);

		folder.GetTasks((int)_TASK_ENUM_FLAGS.TASK_ENUM_HIDDEN)
			.Cast<IRegisteredTask>()
			.ForEach(t => folder.DeleteTask(t.Name, 0));

		folder.GetFolders(0).Cast<ITaskFolder>().ForEach(f => ClearFolder(f.Path));
	}
}
