using System;
using System.Runtime.InteropServices;

namespace Win11Tuned;

/// <summary>
/// 
/// 
/// <see href="https://stackoverflow.com/a/38727406/7065321"></see>
/// <see href="https://stackoverflow.com/a/17047190/7065321"></see>
/// </summary>
static class TokenManipulator
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct TokPriv1Luid
	{
		public int Count;
		public long Luid;
		public int Attributes;
	}

	const int SE_PRIVILEGE_DISABLED = 0x00000000;
	const int SE_PRIVILEGE_ENABLED = 0x00000002;

	const int TOKEN_QUERY = 0x00000008;
	const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;

	[DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
	internal static extern bool AdjustTokenPrivileges(
		IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen
	);

	[DllImport("kernel32.dll", ExactSpelling = true)]
	internal static extern IntPtr GetCurrentProcess();

	[DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
	internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

	[DllImport("advapi32.dll", SetLastError = true)]
	internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

	public static void AddPrivilege(string privilege)
	{
		Set(privilege, SE_PRIVILEGE_ENABLED);
	}

	public static void RemovePrivilege(string privilege)
	{
		Set(privilege, SE_PRIVILEGE_DISABLED);
	}

	static void Set(string privilege, int attr)
	{
		var hproc = GetCurrentProcess();
		var htok = IntPtr.Zero;
		Check(OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok));

		TokPriv1Luid tp;
		tp.Count = 1;
		tp.Luid = 0;
		tp.Attributes = attr;

		Check(LookupPrivilegeValue(null, privilege, ref tp.Luid));
		Check(AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero));
	}

	static void Check(bool @return)
	{
		if (!@return)
		{
			var code = Marshal.GetLastWin32Error();
			throw new SystemException($"Win32 API 调用失败，{code}");
		}
	}
}
