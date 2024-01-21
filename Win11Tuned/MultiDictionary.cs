using System.Collections.Generic;

namespace Win11Tuned;

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
