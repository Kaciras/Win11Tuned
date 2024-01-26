using System;
using System.IO;

namespace Win11Tuned;

sealed class TempWriteableSession : IDisposable
{
	const FileAttributes MASK = ~FileAttributes.ReadOnly;

	readonly FileAttributes attributes;
	readonly string path;

	public TempWriteableSession(string path)
	{
		this.path = path;
		attributes = File.GetAttributes(path);

		if ((attributes ^ MASK) != 0)
		{
			File.SetAttributes(path, attributes & MASK);
		}
	}

	public void Dispose()
	{
		if ((attributes ^ MASK) == 0)
		{
			return;
		}
		try
		{
			File.SetAttributes(path, attributes);
		}
		catch (FileNotFoundException)
		{
			// Ignore: the file has been deleted.
		}
	}
}
