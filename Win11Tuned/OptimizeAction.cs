using System;

namespace Win11Tuned;

/// <summary>
/// 函数式的优化项写法，在优化时调用指定的函数。
/// </summary>
public sealed class OptimizeAction : Optimizable
{
	readonly Action optimizeAction;

	public string Name { get; }

	public string Description { get; }

	public OptimizeAction(string name, string description, Action action)
	{
		optimizeAction = action;
		Name = name;
		Description = description;
	}

	public void Optimize() => optimizeAction();
}
