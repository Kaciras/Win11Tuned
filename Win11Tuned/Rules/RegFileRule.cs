using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Win11Tuned.RegFile;

namespace Win11Tuned.Rules;

/// <summary>
/// 注册表规则，能够判断是否需要导入内置的注册表文件（.reg），并在优化时将其导入。
/// <br/>
/// 注册表文件需要放在 RegFiles 目录下，并设置为 Embedded Resource.
/// <br/>
/// 导入注册表文件一条命令即可，但没有现成的方法可以比较其跟注册表是否一致，故自己实现了这功能。
/// </summary>
public class RegFileRule : Rule
{
	public string Name { get; }

	public string Description { get; }

	readonly string content;

	public RegFileRule(string name, string description, string content)
	{
		this.content = content;
		Name = name;
		Description = description;
	}

	/// <summary>
	/// 对比 Reg 文件和注册表，判断是否存在不同，如果不同就说明需要优化。
	/// </summary>
	public bool NeedOptimize()
	{
		var reader = new RegFileReader(content);
		var expected = true;

		while (expected && reader.Read())
		{
			if (reader.IsKey)
			{
				var exists = RegHelper.KeyExists(reader.Key);
				expected = reader.IsDelete ^ exists;
			}
			else if (reader.IsDelete)
			{
				expected = Registry.GetValue(reader.Key, reader.Name, null) == null;
			}
			else
			{
				expected = CheckValueInDB(reader.Key,
					reader.Name, reader.Value, reader.Kind);
			}
		}

		return !expected;
	}

	/// <summary>
	/// 检查 Reg 文件里的一个值是否已经存在于注册表中。
	/// </summary>
	/// <param name="key">键路径</param>
	/// <param name="name">值名</param>
	/// <param name="valueStr">Reg文件里字符串形式的值</param>
	/// <param name="kind">值类型</param>
	bool CheckValueInDB(string key, string name, object expected, RegistryValueKind kind)
	{
		using var keyObj = RegHelper.OpenKey(key);
		var valueInDB = keyObj.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames);

		// Binary 和 MultiString 返回的是数组，需要用 SequenceEqual 对比。
		bool ConvertAndCheck<T>()
		{
			if (valueInDB is not T[] || valueInDB == null)
			{
				return false;
			}
			return ((T[])expected).SequenceEqual((T[])valueInDB);
		}

		return kind switch
		{
			RegistryValueKind.MultiString => ConvertAndCheck<string>(),
			RegistryValueKind.Binary => ConvertAndCheck<byte>(),
			RegistryValueKind.Unknown or RegistryValueKind.None => throw new Exception("无效的值类型"),
			_ => expected.Equals(valueInDB),
		};
	}

	// 注意 Reg 文件必须是 UTF-16 LE 编码，弄错了会出现中文乱码。
	public void Optimize()
	{
		using var file = Utils.CreateTempFile();
		File.WriteAllText(file.Path, content, Encoding.Unicode);
		RegHelper.Import(file.Path);
	}
}
