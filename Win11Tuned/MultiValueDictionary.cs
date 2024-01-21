using System.Collections.Generic;

namespace Win11Tuned;

public sealed class MultiValueDictionary<TKey, TValue> 
{
	private Dictionary<TKey, List<TValue>> dict = new();

	public IEnumerable<TKey> Keys => dict.Keys;

	public int Count => dict.Count;

	public void Add(TKey key, TValue val)
	{
		if (dict.ContainsKey(key))
		{
			dict[key].Add(val);
		}
		else
		{
			dict.Add(key, new List<TValue>() { val });
		}
	}

	public void Clear()
	{
		dict.Clear();
	}

	public bool ContainsKey(TKey key)
	{
		return dict.ContainsKey(key);
	}

	public void Remove(TKey key)
	{
		if (dict.ContainsKey(key))
		{
			dict.Remove(key);
		}
	}

	public void Remove(TKey key, TValue val)
	{
		if (dict.TryGetValue(key, out List<TValue> collection) && collection.Remove(val))
		{
			if (collection.Count == 0)
				dict.Remove(key);
		}
	}

	public bool Contains(TKey key, TValue val)
	{
		return (dict.TryGetValue(key, out List<TValue> collection) && collection.Contains(val));
	}

	public List<TValue> this[TKey key]
	{
		get
		{
			if (dict.TryGetValue(key, out List<TValue> collection))
				return collection;

			throw new KeyNotFoundException();
		}
	}

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
