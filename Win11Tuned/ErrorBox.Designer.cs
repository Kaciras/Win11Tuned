namespace Win11Tuned
{
	partial class ErrorBox
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
			System.Windows.Forms.Button closeButton;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorBox));
			System.Windows.Forms.Button copyButton;
			this.stackBox = new System.Windows.Forms.TextBox();
			this.messageLabel = new System.Windows.Forms.Label();
			this.iconCaption = new System.Windows.Forms.PictureBox();
			closeButton = new System.Windows.Forms.Button();
			copyButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.iconCaption)).BeginInit();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			resources.ApplyResources(closeButton, "closeButton");
			closeButton.Name = "closeButton";
			closeButton.UseVisualStyleBackColor = true;
			closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// copyButton
			// 
			resources.ApplyResources(copyButton, "copyButton");
			copyButton.Name = "copyButton";
			copyButton.UseVisualStyleBackColor = true;
			copyButton.Click += new System.EventHandler(this.CopyButton_Click);
			// 
			// stackBox
			// 
			resources.ApplyResources(this.stackBox, "stackBox");
			this.stackBox.BackColor = System.Drawing.SystemColors.Control;
			this.stackBox.Name = "stackBox";
			// 
			// messageLabel
			// 
			resources.ApplyResources(this.messageLabel, "messageLabel");
			this.messageLabel.Name = "messageLabel";
			// 
			// iconCaption
			// 
			resources.ApplyResources(this.iconCaption, "iconCaption");
			this.iconCaption.Name = "iconCaption";
			this.iconCaption.TabStop = false;
			// 
			// ErrorBox
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.iconCaption);
			this.Controls.Add(this.messageLabel);
			this.Controls.Add(this.stackBox);
			this.Controls.Add(copyButton);
			this.Controls.Add(closeButton);
			this.Name = "ErrorBox";
			((System.ComponentModel.ISupportInitialize)(this.iconCaption)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox stackBox;
		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.PictureBox iconCaption;
	}
}