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
			.Where(item => item.NeedOptimize());
	}

	TaskOptimizable CheckOptimizable(RuleFileReader reader)
	{
		var path = reader.Read();
		var description = reader.Read();
		var keep = true;
		var getTasks = FindSingle;

		foreach (var str in reader.Read().Split(':'))
		{
			switch (str)
			{
				case "PREFIX":
					getTasks = PrefixSearch;
					break;
				case "DIRECTORY": 
					getTasks = ListFolder;
					break;
				case "DELETE": 
					keep = false; 
					break;
				case "":
					break; // Split 不自动移除空白。
				default:
					throw new InvalidDataException();
			}
		}

		return new TaskOptimizable(path, description, keep, getTasks);
	}

	static IEnumerable<IRegisteredTask> ListFolder(string path)
	{
		var folder = TaskSchedulerManager.Root.GetFolder(path);
		return folder.GetTasks((int)_TASK_ENUM_FLAGS.TASK_ENUM_HIDDEN).Cast<IRegisteredTask>();
	}

	static IEnumerable<IRegisteredTask> FindSingle(string path)
	{
		return Enumerable.Repeat(TaskSchedulerManager.Root.GetTask(path), 1);
	}

	static IEnumerable<IRegisteredTask> PrefixSearch(string path)
	{
		var dir = Path.GetDirectoryName(path);
		var prefix = Path.GetFileName(path);
		return ListFolder(dir).Where(task => task.Name.StartsWith(prefix));
	}
}

sealed class TaskOptimizable : Rule
{
	readonly string path;
	readonly bool keep;
	readonly Func<string, IEnumerable<IRegisteredTask>> getTasks;

	public string Name { get; }

	public string Description { get; }

	IEnumerable<IRegisteredTask> tasks;

	public TaskOptimizable(
		string path, 
		string description, 
		bool keep, 
		Func<string, IEnumerable<IRegisteredTask>> getTasks)
	{
		this.path = path;
		this.keep = keep;
		this.getTasks = getTasks;
		Name = Path.GetFileName(path);
		Description = description;
	}

	public bool NeedOptimize()
	{
		try
		{
			tasks = getTasks(path);
			if (keep)
			{
				tasks = tasks.Where(task => task.Enabled);
			}
			return tasks.Any();
		}
		catch (IOException e)
		when (e is DirectoryNotFoundException || e is FileNotFoundException)
		{
			return false; // Task not found, no need to optimize.
		}
	}

	public void Optimize()
	{
		foreach (var task in tasks)
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
}
