namespace Win11Tunned.RegFile;

public enum RegTokenType : byte
{
	None,           // 未读取到有效数据
	Comment,        // 注释
	Version,        // 文件版本
	CreateKey,      // 创建注册表键
	DeleteKey,      // 删除注册表键
	Name,           // 注册表值的名字
	ValuePart,      // 不完整的值，下一行还有
	Value,          // 读完一个注册表值
	Kind,           // 值的类型
	DeleteValue,    // 删除注册表值
}
