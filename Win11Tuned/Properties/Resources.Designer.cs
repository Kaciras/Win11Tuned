﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Win11Tuned.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Win11Tuned.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Admin.
        /// </summary>
        internal static string AdminName {
            get {
                return ResourceManager.GetString("AdminName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # 没有移除“用 Windows Defender 扫描”，因为禁用 Windows Defender 之后该项自然会移除。
        ///
        ///shellex\ContextMenuHandlers\Compatibility
        ///执行文件 - 兼容性疑难解答
        ///出了问题不去谷歌，看这解答有个卵用。
        ///exefile
        ///
        ///shellex\ContextMenuHandlers\Library Location
        ///文件夹 - 包含到库中
        ///我不用这个系统自带的各种库文件夹，如果要用请勿删除。
        ///Folder
        ///
        ///# 属性页 PropertySheetHandlers 里也有，但不是右键菜单就不管它。
        ///shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}
        ///还原以前的版本
        ///不用文件版本功能的可以移除，不影响属性页。
        ///AllFilesystemObjects
        ///CLSID\{450D8FBA-AD25-11D0-98A8-0800361B1103}
        ///Directory
        ///Drive
        ///WOW6432Node\CLSID\{450D8FBA-AD25-11D0-98A8-0800 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ContextMenuRules {
            get {
                return ResourceManager.GetString("ContextMenuRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Desktop Icons.
        /// </summary>
        internal static string DesktopIcons {
            get {
                return ResourceManager.GetString("DesktopIcons", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Group Policy.
        /// </summary>
        internal static string GroupPolicy {
            get {
                return ResourceManager.GetString("GroupPolicy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer
        ///NoDriveTypeAutoRun
        ///255
        ///关闭自动播放
        ///纯属垃圾功能还有安全风险，不是残疾人都该禁掉。
        ///
        ///HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection
        ///AllowTelemetry
        ///0
        ///禁止发送诊断数据
        ///不向 Microsoft 发送诊断数据，该策略的说明上写着教育版和企业版才能禁止。
        ///
        ///HKLM\SOFTWARE\Policies\Microsoft\Windows\System
        ///UploadUserActivities
        ///0
        ///禁止上传用户活动数据
        ///禁止把浏览历史、搜索记录、什么时候打开了 UWP 应用这些东西上传到 Microsoft。
        ///
        ///HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization
        ///NoLockScreen
        ///1
        ///关闭锁屏界面
        ///即使关了也可以锁定用户，此时显示登录界面，我不知道这个锁屏有什么用。
        ///
        ///HKCU\Software\Policies\ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GroupPolicyRules {
            get {
                return ResourceManager.GetString("GroupPolicyRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hosts File.
        /// </summary>
        internal static string HostsFile {
            get {
                return ResourceManager.GetString("HostsFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Install.
        /// </summary>
        internal static string Install {
            get {
                return ResourceManager.GetString("Install", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Others.
        /// </summary>
        internal static string OtherRules {
            get {
                return ResourceManager.GetString("OtherRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 禁止 TrustedInstaller 生成垃圾
        ///在修改 Windows 功能、更新系统时，TrustedInstaller 会记录日志并备份 WinSxS 中的旧组件，占用大量空间，你用不着它们就禁了。
        ///AvoidCBSGarbage
        ///
        ///不播放 Windows 启动声音
        ///看个人喜好，我是比较喜欢安静一点的
        ///DisableStartupSound
        ///
        ///不下载恶意软件删除工具
        ///Windows 更新会夹带一个屁用没有的恶意软件删除工具，不需要的话可以禁止下载它。
        ///DontOfferMRT
        ///
        ///禁止系统失败后写入调试信息
        ///不做系统相关开发的话这些调试信息鸟用没有，如果禁用了虚拟内存还会在事件日志里报错。
        ///DisableCrashDump
        ///
        ///关闭 PerfDiag 事件日志
        ///解决事件查看器中的会话“PerfDiag Logger”未能启动，存在以下错误: 0xC0000035 问题。
        ///DisablePerfDiagLogger
        ///
        ///开启长路径支持
        ///传统的 Windows 路径最多 255 个字符，从 Windows10 开始能够取消这个限制，建议开启。
        ///EnableLongPath
        ///
        ///启用开发人员模式
        ///启用设置 / 隐私和 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RegistryRules {
            get {
                return ResourceManager.GetString("RegistryRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Context Menu Clean.
        /// </summary>
        internal static string RemoveContextMenu {
            get {
                return ResourceManager.GetString("RemoveContextMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SendTo Clean.
        /// </summary>
        internal static string SendToClean {
            get {
                return ResourceManager.GetString("SendToClean", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # 发传真的那个关闭系统功能后会自动删除。
        ///
        ///Compressed (zipped) Folder.ZFSendToTarget
        ///有专门压缩软件来做了。
        ///
        ///文档.mydocs
        ///就是个直接复制到用户的文档目录，可惜我不用它。
        ///.
        /// </summary>
        internal static string SendToRules {
            get {
                return ResourceManager.GetString("SendToRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # WinHttpAutoProxySvc 禁用后【网络和 Internet/代理】显示异常
        ///# TabletInputService 禁用墨迹服务会导致输入法无法启动，表现为无限访问 OOBE\LaunchUserOOBE 键。
        ///
        ///BDESVC
        ///不使用 BitLocker 加密的话可以禁了，禁用后 设置/BitLocker 不可用，默认手动但必定会被启动。
        ///Disabled
        ///
        ///DPS
        ///屁用没有的诊断功能，出了问题不 Google 谁看这玩意
        ///Disabled
        ///
        ///DiagTrack
        ///收集用户信息的，如果不想分享给微软就可以关了，据某些用户说会占用较多资源
        ///Disabled
        ///
        ///hidserv
        ///如果你没有 HID 设备就可以禁用，该服务还容易被攻击者利用。
        ///Disabled
        ///
        ///ShellHWDetection
        ///这年头谁还玩自动播放，该服务也容易被攻击者利用，建议禁了
        ///Disabled
        ///
        ///WSearch
        ///系统自带的搜索功能，挺占磁盘的，如果不需要高级搜索功能就关了吧
        ///Disabled
        ///
        ///PcaSvc
        ///啥卵用都没有的兼容性助手
        ///Disabled
        ///
        ///IKEEXT
        ///某些协议的 VPN 需要这个服务，不用的话可以关掉
        ///Di [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ServiceRules {
            get {
                return ResourceManager.GetString("ServiceRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Services.
        /// </summary>
        internal static string Services {
            get {
                return ResourceManager.GetString("Services", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Startup Clean.
        /// </summary>
        internal static string StartupClean {
            get {
                return ResourceManager.GetString("StartupClean", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  (System).
        /// </summary>
        internal static string SystemScope {
            get {
                return ResourceManager.GetString("SystemScope", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System Settings.
        /// </summary>
        internal static string SystenSettings {
            get {
                return ResourceManager.GetString("SystenSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Microsoft\Windows\Application Experience
        ///客户体验改善计划相关的任务，没同意的可以删了
        ///
        ///Microsoft\Windows\Autochk
        ///客户体验改善计划相关的任务，没同意的可以删了
        ///
        ///Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector
        ///客户体验改善计划相关的任务，没同意的可以删了
        ///
        ///Microsoft\Windows\Customer Experience Improvement Program
        ///全是客户体验改善计划，不参加的直接删光光
        ///
        ///\Microsoft\Windows\Feedback\Siuf
        ///Windows 反馈的任务，会收集用户信息，建议删了
        ///
        ///# TODO
        ///Microsoft\Windows\Application Experience\StartupAppTask
        ///启动项应该自己注意，而不是靠它来扫描
        ///:DISABLE
        ///
        ///Microsoft\Windows\DiskCleanup\SilentCleanup
        ///什么傻逼任务，磁盘空间不足还用你来 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TaskSchdulerRules {
            get {
                return ResourceManager.GetString("TaskSchdulerRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Task Scheduler Clean.
        /// </summary>
        internal static string TaskScheduler {
            get {
                return ResourceManager.GetString("TaskScheduler", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uninstall.
        /// </summary>
        internal static string Uninstall {
            get {
                return ResourceManager.GetString("Uninstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uninstall Preinstalled Apps.
        /// </summary>
        internal static string UninstallApps {
            get {
                return ResourceManager.GetString("UninstallApps", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Microsoft.BingNews
        ///Microsoft.BingWeather
        ///Microsoft.GamingApp
        ///Microsoft.GetHelp
        ///Microsoft.Getstarted
        ///Microsoft.Messaging
        ///Microsoft.Microsoft3DViewer
        ///Microsoft.MicrosoftOfficeHub
        ///Microsoft.MicrosoftStickyNotes
        ///Microsoft.MicrosoftSolitaireCollection
        ///Microsoft.NetworkSpeedTest
        ///Microsoft.Office
        ///Microsoft.OneConnect
        ///Microsoft.People
        ///Microsoft.Print3D
        ///Microsoft.ScreenSketch
        ///Microsoft.SkypeApp
        ///Microsoft.Todos
        ///Microsoft.WindowsAlarms
        ///Microsoft.WindowsFeedbackHub
        ///Microsoft.WindowsMaps
        ///Microsoft.Xbox
        ///Microsoft.XboxApp [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UninstallAppxRules {
            get {
                return ResourceManager.GetString("UninstallAppxRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 禁用自动播放
        ///这年头谁还用这功能，而且自动行为容易有安全问题。
        ///DisableAutoPlay
        ///
        ///优化资源管理器选项
        ///显示文件扩展名、自动扩展到当前目录、不显示库等，优化后需要在资源管理器里刷新一下。
        ///ExplorerOptions
        ///
        ///优化任务栏设置
        ///不合并按钮，显示所有托盘图标等，总之不要隐藏信息，除非你会同时开十几个程序否则任务栏是用不满的。
        ///Taskbar
        ///
        ///恢复经典右键菜单
        ///Win11 的新菜单设计的像个傻逼，赶紧退回去！
        ///ClassicContextMenu
        ///
        ///禁用辅助功能
        ///就是禁用筛选键、粘滞键、鼠标键、切换键等等东西，如果不是残疾人就没用，一不小心就按出来相当烦人。
        ///DisableAccessibility
        ///
        ///简化视觉效果
        ///除了“显示缩略图而不是显示图标”和“拖动时显示窗口内容”外全部关闭，避免各种效果浪费时间和性能。
        ///VisualEffects
        ///
        ///关闭 Windows 新增内容和建议
        ///取消勾选设置和选项 / 系统 / 通知 最下面的两个框。
        ///DisableSuggestNotification
        ///
        ///移除广告 ID
        ///不允许使用我的广告 ID 来展示个性化广告。
        ///RemoveAdvertisingI [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UserRegistryRules {
            get {
                return ResourceManager.GetString("UserRegistryRules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  (User).
        /// </summary>
        internal static string UserScope {
            get {
                return ResourceManager.GetString("UserScope", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some optimizations require administrator privileges.
        /// </summary>
        internal static string UserTooltip {
            get {
                return ResourceManager.GetString("UserTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UWP Apps.
        /// </summary>
        internal static string UWPApps {
            get {
                return ResourceManager.GetString("UWPApps", resourceCulture);
            }
        }
    }
}
