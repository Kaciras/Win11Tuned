using System.Collections.Generic;

namespace Win11Tuned;

/// <summary>
/// 表示一类优化规则，在扫描时返回优化项的集合。
/// <br/>
/// 该类在界面的 TreeView 里显示为顶层节点。
/// </summary>
public interface OptimizableSet
{
	string Name { get; }

	IEnumerable<Optimizable> Scan();
}
