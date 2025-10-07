# Win11Tuned

![Screenshot](https://github.com/Kaciras/Win11Tuned/raw/master/screenshot.png)

自用的 Windows11 优化工具，未在其它系统上测试过。

> [!WARNING]  
> 本工具没有撤销更改的功能，请慎重选择优化项，或者提前备份设置。

# 使用方法

点击 Scan 按钮，然后选中需要优化的项，最后按 Optimize 按钮即可，点击每条优化项右侧都有对应的说明。**一些配置需要重启才能生效，建议运行后重启系统。**

本程序可以在标准权限下运行，此时只能优化用户相关的设置，要优化系统设置请以管理员身份运行。

禁用 Microsoft Defender 的优化项需要运行两次，先执行一次，然后重启到安全模式再执行一次优化。

# 何时使用

建议在安装系统的最后一步，使用本程序做优化收尾，请先完成更新系统、安装驱动、启用或关闭 Windows 功能等。

本程序不是清理工具，无需经常运行，因为配置不会自己改变，优化一次即可，完成后可以删除本程序。

# 开发

部分功能及其测试用例需要管理员权限，建议以管理员权限运行 IDE。

如果编译报错无法找到`Windows.*`命名空间，请将`C:\Program Files (x86)\Windows Kits\10\UnionMetadata\<version>\Windows.winmd`加入到项目引用。

优化规则写由 RuleProvider.cs 载入，大部分定义在 Resources 目录下，少数直接写在代码里。
