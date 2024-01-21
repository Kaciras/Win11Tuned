using System.Collections.Generic;

namespace Win11Tuned;

public sealed class MultiValueDictionary<TKey, TValue> 
{
	readonly Dictionary<TKey, List<TValue>> dict = new();

	public IEnumerable<TKey> Keys => dict.Keys;

	public int Count => dict.Count;

	public void Add(TKey key, TValue val)
	{
		if (dict.TryGetValue(key, out var list))
		{
			list.Add(val);
		}
		else
		{
			list = [val];
			dict.Add(key, list);
		}
	}

	public void Clear()
	{
		dict.Clear();
	}

	public bool TryGetList(TKey key, out List<TValue> value)
	{
		return dict.TryGetValue(key, out value);
	}

	public void Remove(TKey key)
	{
		dict.Remove(key);
	}

	public void Remove(TKey key, TValue value)
	{
		if (dict.TryGetValue(key, out var list))
		{
			list.Remove(value);
			if (list.Count == 0)
			{
				dict.Remove(key);
			}
		}
	}

	public bool Contains(TKey key, TValue val)
	{
		return dict.TryGetValue(key, out var list) && list.Contains(val);
	}

	public List<TValue> this[TKey key] => dict[key];

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		foreach (var entry in dict)
		{
			var collection = entry.Value;

			foreach (var item in collection)
			{
				yield return new KeyValuePair<TKey, TValue>(entry.Key, item);
			}
		}
	}
}
