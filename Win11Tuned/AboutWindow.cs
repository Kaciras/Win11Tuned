using System;
using System.Windows.Forms;

namespace Win11Tuned;

sealed partial class AboutWindow : Form
{
	public AboutWindow()
	{
		InitializeComponent();

		var version = typeof(AboutWindow).Assembly.GetName().Version.ToString(3);
		versionLabel.Text = string.Format(versionLabel.Text, version, "2022-4-20");
	}

	private void CloseButton_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
	}
}
