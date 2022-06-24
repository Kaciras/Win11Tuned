namespace Win11Tuned;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
			this.closeButton = new System.Windows.Forms.Button();
			this.linkLabel = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			resources.ApplyResources(this.closeButton, "closeButton");
			this.closeButton.Name = "closeButton";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// linkLabel
			// 
			resources.ApplyResources(this.linkLabel, "linkLabel");
			this.linkLabel.Name = "linkLabel";
			this.linkLabel.TabStop = true;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// nameLabel
			// 
			resources.ApplyResources(this.nameLabel, "nameLabel");
			this.nameLabel.Name = "nameLabel";
			// 
			// versionLabel
			// 
			resources.ApplyResources(this.versionLabel, "versionLabel");
			this.versionLabel.Name = "versionLabel";
			// 
			// AboutWindow
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.versionLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabel);
			this.Controls.Add(this.closeButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutWindow";
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
