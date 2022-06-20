using System.Collections.Generic;
using System.Linq;

namespace Win11Tunned.Rules;

/// <summary>
/// 一个简单的 OptimizableSet 实现，手动设置名字并提供返回规则列表的委托，
/// 在扫描时调用每条规则的 Check() 方法。
/// </summary>
public class RuleList : OptimizableSet
{
	readonly IEnumerable<Rule> rules;

	public string Name { get; }

	public RuleList(string name, IEnumerable<Rule> rules)
	{
		Name = name;
		this.rules = rules;
	}

	public IEnumerable<Optimizable> Scan()
	{
		return rules.Where(rule => rule.NeedOptimize());
	}
}
