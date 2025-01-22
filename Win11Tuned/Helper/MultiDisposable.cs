using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11Tuned.Helper;

public class MultiDisposable<T> : IDisposable
{
	readonly List<IDisposable> disposables;

	public MultiDisposable(IEnumerable<T> input, Func< T, IDisposable> factory)
	{
		try
		{
			disposables = input.Select(factory).ToList();
		} 
		catch (Exception ex)
		{
			Dispose();
			throw ex;
		}
	}

	public void Dispose()
	{
		var exceptions = new List<Exception>();
		for (int i = disposables.Count - 1; i >= 0; i--)
		{
			try
			{
				disposables[i].Dispose();
			}
			catch (Exception ex)
			{
				exceptions.Add(ex);
			}
		}
		if (exceptions.Count > 0)
		{
			throw new AggregateException(exceptions);
		}
	}
}
