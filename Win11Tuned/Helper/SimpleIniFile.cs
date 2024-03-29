﻿using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Win11Tuned;

/// <summary>
/// 使用 Windows 自带的底层 API 读取 INI 配置文件，当然功能还是比较简陋。
/// </summary>
/// <seealso cref="https://stackoverflow.com/a/14906422/7065321"/>
readonly struct SimpleIniFile(string path)
{
	static readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name;

	// 好像没法读取无 Section 的键
	public string Read(string section, string key, string @default)
	{
		var retVal = new StringBuilder(255);
		GetPrivateProfileString(section ?? EXE, key, @default, retVal, 255, path);
		return retVal.ToString();
	}

	/*
	 * 这个函数返回读取到的值的长度，错误代码请用 GetLastError 获取，但无论哪种错误都返回 2（FILE_NOT_FOUND）。
	 * 如果找不到键，则使用默认值，但此时错误码仍不为 0，所以没法做检查。
	 * https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getprivateprofilestring
	 */
	[DllImport("kernel32", CharSet = CharSet.Unicode)]
	static extern int GetPrivateProfileString(
		string appName, string key, string @default,
		StringBuilder reurnString, int nSize, string filename);
}
