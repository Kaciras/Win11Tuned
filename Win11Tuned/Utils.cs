using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using Shell32;

namespace Win11Tuned;

internal static class Utils
{
	/// <summary>
	/// 这么常用的函数标准库竟然不自带。
	/// </summary>
	public static void ForEach<T>(this IEnumerable<T> ienum, Action<T> action)
	{
		foreach (var item in ienum) action(item);
	}

	/// <summary>
	/// 获取当前运行程序的用户，并检测其是否具有管理员权限。
	/// </summary>
	/// <seealso cref="https://stackoverflow.com/a/5953294/7065321"/>
	public static bool CheckIsAdministrator()
	{
		using var identity = WindowsIdentity.GetCurrent();
		var principal = new WindowsPrincipal(identity);
		return principal.IsInRole(WindowsBuiltInRole.Administrator);
	}

	/// <summary>
	/// 获取快捷方式所指向文件的路径。
	/// </summary>
	/// <seealso cref="https://stackoverflow.com/a/9414495/7065321"/>
	/// <param name="filename">快捷方式文件</param>
	/// <returns>目标文件的路径，如果不存在则为 null</returns>
	/// <exception cref="InvalidOperationException">文件不是快捷方式</exception>
	/// <exception cref="UnauthorizedAccessException">无权限，或者是特殊的链接，比如开始菜单里的桌面</exception>
	/// <exception cref="FileNotFoundException">如果所给的快捷方式不存在</exception>
	public static string GetShortcutTarget(string filename)
	{
		return STAExecutor.Run(() =>
		{
			var pathOnly = Path.GetDirectoryName(filename);
			var filenameOnly = Path.GetFileName(filename);
			var shell = new Shell();

			var folderItem = shell.NameSpace(Path.GetFullPath(pathOnly))
				?.ParseName(filenameOnly)
				?? throw new FileNotFoundException("File not exists", filename);

			// 如果不是链接 GetLink 会抛 NotImplementedException
			if (!folderItem.IsLink)
			{
				throw new InvalidOperationException("File is not a link");
			}

			try
			{
				return ((ShellLinkObject)folderItem.GetLink).Path;
			}
			catch (COMException)
			{
				return null; // 新装的系统有个无效的 Fax Recipient 会出异常
			}
		});
	}

	/// <summary>
	/// 解析类似 "@shell32.dll,-1#immutable1" 这样的字符串，并读取其引用的字符串资源。
	/// </summary>
	/// <param name="string">字符串引用</param>
	/// <returns>字符串资源</returns>
	/// <exception cref="FormatException">如果参数不是合法的资源引用</exception>
	public static string ExtractStringResource(string @string)
	{
		var splited = @string.Split(',');
		if (splited.Length != 2)
		{
			throw new FormatException("参数格式错误");
		}
		if (splited[0].EndsWith(".inf"))
		{
			return @string.Split(';')[1];
		}
		var file = splited[0].TrimStart('@');
		var index = Math.Abs(int.Parse(splited[1]));
		return ExtractStringFromDLL(file, index);
	}

	/// <summary>
	/// 从 Windows 动态链接库文件里读取内置的字符串资源。
	/// </summary>
	/// <param name="file">DLL文件</param>
	/// <param name="id">资源索引，不能是负数</param>
	/// <returns>字符串资源</returns>
	public static string ExtractStringFromDLL(string file, int id)
	{
		file = Environment.ExpandEnvironmentVariables(file);

		// 使用 LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE + LOAD_LIBRARY_AS_IMAGE_RESOURCE 两个标志，
		// 避免加载多余的依赖，防止出现 12 错误码。
		// https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryexw
		var lib = LoadLibraryEx(file, IntPtr.Zero, 0x00000020 | 0x00000002);

		var code = Marshal.GetLastWin32Error();
		if (code == 0)
		{
			var buffer = new StringBuilder(2048);
			LoadString(lib, id, buffer, buffer.Capacity);
			FreeLibrary(lib);
			return buffer.ToString();
		}

		throw new SystemException($"无法加载 {file}，错误代码:{code}");
	}

	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, int flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	static extern int LoadString(IntPtr hInstance, int ID, StringBuilder lpBuffer, int nBufferMax);

	[DllImport("kernel32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool FreeLibrary(IntPtr hModule);

	public sealed class TempFileSession : IDisposable
	{
		public readonly string Path;

		internal TempFileSession(string path)
		{
			Path = path;
		}

		public void Dispose() => File.Delete(Path);
	}
}
