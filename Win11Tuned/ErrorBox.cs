using System;
using System.Drawing;
using System.Windows.Forms;

namespace Win11Tuned;

/// <summary>
/// 简单的消息框没有复制按钮，以及不支持滚动条，所以就自己搞个错误弹窗。
/// WinForms 默认的窗口也没有复制按钮，详细信息还要多点一步。
/// </summary>
public partial class ErrorBox : Form
{
	public ErrorBox(Exception ex) : this(ex, ex.Message) {}

	public ErrorBox(Exception exception, string message)
	{
		Icon = SystemIcons.Error;
		InitializeComponent();
		iconCaption.Image = SystemIcons.Error.ToBitmap();
		messageLabel.Text = message;
		stackBox.Text = exception.StackTrace;
	}

	void CopyButton_Click(object _, EventArgs e)
	{
		var text = messageLabel.Text + "\n" + stackBox.Text;
		STAExecutor.Run(() => Clipboard.SetText(text));
	}
	void CloseButton_Click(object _, EventArgs e) => Close();
}
