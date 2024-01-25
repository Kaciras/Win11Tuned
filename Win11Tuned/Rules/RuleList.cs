using System.Collections.Generic;
using System.Linq;

namespace Win11Tuned.Rules;

/// <summary>
/// 一个简单的 OptimizableSet 实现，手动设置名字并提供返回规则列表的委托，
/// 在扫描时调用每条规则的 Check() 方法。
/// </summary>
public class RuleList(string name, IEnumerable<Rule> rules) : OptimizableSet
{
	public string Name { get; } = name;

	public IEnumerable<Optimizable> Scan()
	{
		return rules.Where(rule => rule.NeedOptimize());
	}
}
