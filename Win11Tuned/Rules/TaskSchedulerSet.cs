using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskScheduler;
using Win11Tuned.Properties;

namespace Win11Tuned.Rules;

public class TaskSchedulerSet : OptimizableSet
{
	public string Name => Resources.TaskScheduler;

	public IEnumerable<Optimizable> Scan()
	{
		return RuleFileReader
			.Iter(Resources.TaskSchdulerRules)
			.Select(CheckOptimizable)
			.Where(item => item != null);
	}

	Optimizable CheckOptimizable(RuleFileReader reader)
	{
		var path = reader.Read();
		var description = reader.Read();
		var keep = reader.Read() == ":DISABLE";

		try
		{
			var folder = TaskSchedulerManager.Root.GetFolder(path);
			if (folder.GetTasks((int)_TASK_ENUM_FLAGS.TASK_ENUM_HIDDEN).Count == 0)
			{
				return null;
			}
			return new FolderOptimizeItem(folder, description);
		}
		catch (IOException e)
		when (e is DirectoryNotFoundException || e is FileNotFoundException)
		{
			// Ignore, maybe the path is a task.
		}

		try
		{
			var task = TaskSchedulerManager.Root.GetTask(path);
			if (keep && !task.Enabled)
			{
				return null;
			}
			return new TaskOptimizeItem(task, keep, description);
		}
		catch (IOException e)
		when (e is DirectoryNotFoundException || e is FileNotFoundException)
		{
			return null; // Task not found, cannot optimize
		}
	}
}

internal sealed class FolderOptimizeItem : Optimizable
{
	private readonly ITaskFolder folder;

	public string Name => folder.Name;

	public string Description { get; }

	public FolderOptimizeItem(ITaskFolder folder, string description)
	{
		this.folder = folder;
		Description = description;
	}

	public void Optimize()
	{
		TaskSchedulerManager.ClearFolder(folder.Path);
	}
}

internal sealed class TaskOptimizeItem : Optimizable
{
	private readonly IRegisteredTask task;
	private readonly bool keep;

	public string Name => task.Name;

	public string Description { get; }

	public TaskOptimizeItem(IRegisteredTask task, bool keep, string description)
	{
		this.task = task;
		this.keep = keep;
		Description = description;
	}

	public void Optimize()
	{
		if (keep)
		{
			task.Enabled = false;
		}
		else
		{
			TaskSchedulerManager.DeleteTask(task.Path);
		}
	}
}
