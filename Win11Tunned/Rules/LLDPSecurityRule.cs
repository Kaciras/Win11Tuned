namespace Win11Tunned.Rules;

/// <summary>
/// 事件查看器中有时会出现 “CAPI2 513 加密服务处理系统写入程序对象中的 OnIdentity() 调用时失败” 错误。
/// 原因是 VSS Writer 没有读取 NT AUTHORITY\SERVICE （服务帐户） 的权限，当其尝试访问 Mslldp.sys 时会出现此错误。
/// <br/>
/// 解决方案是给 LLDP 驱动添加权限，通过 sc 命令可以做到。
/// <br/>
/// <seealso cref="https://support.microsoft.com/en-us/help/3209092/event-id-513-when-running-vss-in-windows-server"/>
/// <seealso cref="https://social.technet.microsoft.com/Forums/forefront/en-US/156d3b56-0863-47fb-851f-82ea78a7cff2/error-source-capi2-id-513-cryptographic-services-failed-while-processing-the-onidentity-call-in?forum=w8itprogeneral"/>
/// </summary>
public sealed class LLDPSecurityRule : Rule
{
	/// <summary>
	/// 权限描述符的字符串形式，被分号分为三部分。
	/// <br/>
	/// 开头的 A 表示 Allow；中间每两个字母一组代表权限，最后是账户。
	/// <br/>
	/// <seealso cref="https://docs.microsoft.com/zh-cn/windows/win32/secauthz/security-descriptor-string-format"/>
	/// </summary>
	const string SERVICE_PERM = "(A;;CCLCSWLOCRRC;;;SU)";

	public string Name => "给 LLDP 驱动添加服务帐户权限";

	public string Description => "解决事件日志里的 CAPI2 513 “加密服务处理系统写入程序对象中的 OnIdentity() 调用时失败” 错误";

	string descriptor;

	public bool NeedOptimize()
	{
		var sc = Utils.Execute("sc.exe", "sdshow mslldp");
		descriptor = sc.StandardOutput.ReadToEnd().Trim();
		return !descriptor.Contains(";SU)");
	}

	public void Optimize()
	{
		var i = descriptor.IndexOf("S:");
		var newDescriptor = (i != -1)
			? descriptor.Insert(i, SERVICE_PERM)
			: descriptor + SERVICE_PERM;

		Utils.Execute("sc.exe", "sdset mslldp " + newDescriptor);
	}
}
