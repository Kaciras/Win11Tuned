using System.Collections.Generic;

namespace Win11Tuned;

/// <summary>
/// 一种最简单的 MultiDict 实现，直接继承 Dictionary 并添加列表相关的方法即可。
/// </summary>
sealed class MultiDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
{
	public void Add(TKey key, TValue val)
	{
		if (TryGetValue(key, out var list))
		{
			list.Add(val);
		}
		else
		{
			list = [val];
			Add(key, list);
		}
	}

	public void Remove(TKey key, TValue value)
	{
		if (TryGetValue(key, out var list))
		{
			list.Remove(value);
			if (list.Count == 0) Remove(key);
		}
	}

	public bool Contains(TKey key, TValue val)
	{
		return TryGetValue(key, out var list) && list.Contains(val);
	}
}
