namespace Win11Tunned;

partial class AboutWindow
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
			this.closeButton = new System.Windows.Forms.Button();
			this.linkLabel = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Location = new System.Drawing.Point(230, 109);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 26);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "关闭";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// linkLabel
			// 
			this.linkLabel.AutoSize = true;
			this.linkLabel.Location = new System.Drawing.Point(12, 89);
			this.linkLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.linkLabel.Name = "linkLabel";
			this.linkLabel.Size = new System.Drawing.Size(257, 12);
			this.linkLabel.TabIndex = 1;
			this.linkLabel.TabStop = true;
			this.linkLabel.Text = "https://github.com/Kaciras/Win11Tunned";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 67);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(251, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "本程序完全免费，并使用 GPL-3.0 协议开源：";
			// 
			// nameLabel
			// 
			this.nameLabel.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.nameLabel.Location = new System.Drawing.Point(12, 12);
			this.nameLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(293, 23);
			this.nameLabel.TabIndex = 3;
			this.nameLabel.Text = "Kaciras 的 Win8 优化工具";
			this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// versionLabel
			// 
			this.versionLabel.AutoSize = true;
			this.versionLabel.Location = new System.Drawing.Point(13, 45);
			this.versionLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size(179, 12);
			this.versionLabel.TabIndex = 4;
			this.versionLabel.Text = "版本 X.X.X, 更新于 2020-XX-XX";
			// 
			// AboutWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 147);
			this.Controls.Add(this.versionLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabel);
			this.Controls.Add(this.closeButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "关于";
			this.ResumeLayout(false);
			this.PerformLayout();

	}

	#endregion

	private System.Windows.Forms.Button closeButton;
	private System.Windows.Forms.LinkLabel linkLabel;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.Label nameLabel;
	private System.Windows.Forms.Label versionLabel;
}
