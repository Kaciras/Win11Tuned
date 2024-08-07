using System;
using System.Drawing;
using System.Windows.Forms;

namespace Win11Tuned;

/// <summary>
/// 简单的消息框没有复制按钮，以及不支持滚动条，所以就自己搞个错误弹窗。
/// </summary>
public partial class ErrorBox : Form
{
	public ErrorBox(Exception ex, string message)
	{
		Icon = SystemIcons.Error;
		InitializeComponent();
		iconCaption.Image = SystemIcons.Error.ToBitmap();
		messageLabel.Text = message;
		stackBox.Text = ex.StackTrace;
	}

	public ErrorBox(Exception ex) : this(ex, ex.Message) {}

	void CloseButton_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
	}

	void CopyButton_Click(object sender, EventArgs e)
	{
		var text = messageLabel.Text + "\n" + stackBox.Text;
		STAExecutor.Run(() => Clipboard.SetText(text));
	}
}
