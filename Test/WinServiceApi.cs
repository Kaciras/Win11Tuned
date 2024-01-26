using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Win11Tuned.Test;

/// <summary>
/// 直接添加注册表可以创建服务，但是并不能立即被 ServiceController 获取，重启后则可以，推测可需要刷新一下。
/// <br/>
/// 但我找了半天也没发现怎么刷新，于是只能改用 win32 api 来创建测试服务。
/// <br/>
/// 代码参考了 PowerShell：
/// https://github.com/PowerShell/PowerShell/blob/master/src/Microsoft.PowerShell.Commands.Management/commands/management/Service.cs
/// </summary>
sealed class WinServiceApi(string name, string binPath)
{
	public string Name { get; } = name;

	public string BinPath { get; } = binPath;

	public string DisplayName { get; set; }

	public void Install()
	{
		var hScManager = IntPtr.Zero;
		var hService = IntPtr.Zero;
		try
		{
			hScManager = OpenSCManagerW(null, null, SC_MANAGER_CONNECT | SC_MANAGER_CREATE_SERVICE);
			CheckResult(nameof(OpenSCManagerW), hScManager);

			hService = CreateServiceW(
					hScManager,
					Name,
					DisplayName,
					SERVICE_CHANGE_CONFIG | WRITE_DAC | WRITE_OWNER,
					SERVICE_WIN32_OWN_PROCESS,
					SERVICE_AUTO_START,
					SERVICE_ERROR_NORMAL,
					BinPath,
					null,
					null,
					IntPtr.Zero,
					null,
					IntPtr.Zero);
			CheckResult(nameof(CreateServiceW), hService);
		}
		finally
		{
			CloseServiceHandle(hService);
			CloseServiceHandle(hScManager);
		}
	}

	public static void Uninstall(string name)
	{
		var hScManager = IntPtr.Zero;
		var hService = IntPtr.Zero;
		try
		{
			hScManager = hScManager = OpenSCManagerW(string.Empty, null, SC_MANAGER_ALL_ACCESS);
			CheckResult(nameof(OpenSCManagerW), hScManager);

			hService = OpenServiceW(hScManager, name, SERVICE_DELETE);
			CheckResult(nameof(OpenServiceW), hService);

			CheckResult(nameof(DeleteService), DeleteService(hService));
		}
		finally
		{
			CloseServiceHandle(hService);
			CloseServiceHandle(hScManager);
		}
	}

	static void CheckResult(string name, IntPtr pointer)
	{
		if (pointer == IntPtr.Zero)
		{
			var message = $"Error occurred on {name}";
			throw new Win32Exception(Marshal.GetLastWin32Error(), message);
		}
	}

	static void CheckResult(string name, bool status)
	{
		if (!status)
		{
			var message = $"Error occurred on {name}";
			throw new Win32Exception(Marshal.GetLastWin32Error(), message);
		}
	}

	const string DLL_NAME = "api-ms-win-service-management-l1-1-0.dll";

	const int ERROR_SERVICE_ALREADY_RUNNING = 1056;
	const int ERROR_SERVICE_NOT_ACTIVE = 1062;
	const int ERROR_INSUFFICIENT_BUFFER = 122;
	const uint ERROR_ACCESS_DENIED = 0x5;

	const uint SC_MANAGER_CONNECT = 1;
	const uint SC_MANAGER_CREATE_SERVICE = 2;
	const uint SC_MANAGER_ALL_ACCESS = 0xf003f;

	const uint SERVICE_QUERY_CONFIG = 1;
	const uint SERVICE_CHANGE_CONFIG = 2;
	const uint SERVICE_DELETE = 0x10000;
	const uint SERVICE_NO_CHANGE = 0xffffffff;

	const uint SERVICE_AUTO_START = 0x2;

	const uint SERVICE_CONFIG_DESCRIPTION = 1;
	const uint SERVICE_CONFIG_DELAYED_AUTO_START_INFO = 3;
	const uint SERVICE_CONFIG_SERVICE_SID_INFO = 5;
	const uint WRITE_DAC = 262144;
	const uint WRITE_OWNER = 524288;
	const uint SERVICE_WIN32_OWN_PROCESS = 0x10;
	const uint SERVICE_ERROR_NORMAL = 1;

	[DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern
	IntPtr OpenSCManagerW(
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpMachineName,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpDatabaseName,
		uint dwDesiredAccess);

	[DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern
	IntPtr CreateServiceW(
		IntPtr hSCManager,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpServiceName,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpDisplayName,
		uint dwDesiredAccess,
		uint dwServiceType,
		uint dwStartType,
		uint dwErrorControl,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpBinaryPathName,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpLoadOrderGroup,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpdwTagId,
		[In] IntPtr lpDependencies,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpServiceStartName,
		[In] IntPtr lpPassword);

	[DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern IntPtr OpenServiceW(
		IntPtr hSCManager,
		[In, MarshalAs(UnmanagedType.LPWStr)] string lpServiceName,
		uint dwDesiredAccess);

	[DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern bool DeleteService(IntPtr hService);

	[DllImport(DLL_NAME, CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern bool CloseServiceHandle(IntPtr hSCManagerOrService);
}
