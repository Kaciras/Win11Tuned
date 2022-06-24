using System;
using System.Threading;

namespace Win11Tuned.Test;

static class TestHelper
{
	public static void RunInNewThread(Action action)
	{
		Exception exception = null;
		var thread = new Thread(() =>
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				exception = e;
			}
		});
		thread.Start();
		thread.Join();
		if (exception != null) throw exception;
	}
}
