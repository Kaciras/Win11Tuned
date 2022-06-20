namespace Win11Tunned.Rules;

/// <summary>
/// 实现上因为没找到 System.Management.Automation.dll 的引用，所以采用了命令行的方式，灵活性会差点。
/// 详情见：
/// https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies
/// </summary>
public sealed class PowerShellPolicyRule : Rule
{
	public string Name => "设置 PowerShell 的执行策略为 RemoteSigned";

	public string Description => "默认不让执行脚本太严了，该策略并不能提高系统的安全性，只是用来防止无意的操作，故改为与 Windows Server 一致的宽松策略。";

	public bool NeedOptimize()
	{
		var proc = Utils.Execute("powershell", "Get-ExecutionPolicy");
		return proc.StandardOutput.ReadToEnd().TrimEnd() == "Restricted";
	}

	public void Optimize()
	{
		Utils.Execute("powershell", "Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine");
	}
}
