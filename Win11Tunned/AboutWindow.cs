using System;
using System.Windows.Forms;

namespace Win11Tunned;

sealed partial class AboutWindow : Form
{
	public AboutWindow()
	{
		InitializeComponent();

		var version = typeof(AboutWindow).Assembly.GetName().Version;
		versionLabel.Text = $"版本 {version.ToString(3)}，更新于 2022-4-20";
	}

	private void CloseButton_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
	}
}
