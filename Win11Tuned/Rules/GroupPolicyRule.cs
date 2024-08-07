using Microsoft.Win32;

namespace Win11Tuned.Rules;

/// <summary>
/// 修改一个组策略（gpedit.msc）项的规则，组策略的项目跟注册表类似，通常位于：
/// <code>HKEY_LOCAL_MACHINE\SOFTWARE\Policies\</code>
/// </summary>
public sealed class GroupPolicyRule : Rule
{
	readonly string key;
	readonly string item;
	readonly string value;

	public string Name { get; }

	public string Description { get; }

	public GroupPolicyRule(string key, string item, string value, string name, string description)
	{
		this.key = key;
		this.item = item;
		this.value = value;
		Name = name;
		Description = description;
	}

	public bool NeedOptimize()
	{
		return GroupPolicy.Get(key, item)?.ToString() != value;
	}

	public void Optimize()
	{
		GroupPolicy.Set(key, item, value, RegistryValueKind.DWord);
	}
}
