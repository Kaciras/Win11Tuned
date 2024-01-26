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
			System.Windows.Forms.LinkLabel linkLabel;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label nameLabel;
			this.closeButton = new System.Windows.Forms.Button();
			this.versionLabel = new System.Windows.Forms.Label();
			linkLabel = new System.Windows.Forms.LinkLabel();
			label1 = new System.Windows.Forms.Label();
			nameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// linkLabel
			// 
			resources.ApplyResources(linkLabel, "linkLabel");
			linkLabel.Name = "linkLabel";
			linkLabel.TabStop = true;
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// nameLabel
			// 
			resources.ApplyResources(nameLabel, "nameLabel");
			nameLabel.Name = "nameLabel";
			// 
			// closeButton
			// 
			resources.ApplyResources(this.closeButton, "closeButton");
			this.closeButton.Name = "closeButton";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
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
			this.Controls.Add(nameLabel);
			this.Controls.Add(label1);
			this.Controls.Add(linkLabel);
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
	private System.Windows.Forms.Label versionLabel;
}
