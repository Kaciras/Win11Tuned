namespace Win11Tunned;

partial class MainWindow
{
	/// <summary>
	/// 必需的设计器变量。
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// 清理所有正在使用的资源。
	/// </summary>
	/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows 窗体设计器生成的代码

	/// <summary>
	/// 设计器支持所需的方法 - 不要修改
	/// 使用代码编辑器修改此方法的内容。
	/// </summary>
	private void InitializeComponent()
	{
			System.Windows.Forms.GroupBox groupBox1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.textBox = new System.Windows.Forms.RichTextBox();
			this.scanButton = new System.Windows.Forms.Button();
			this.btnOptimize = new System.Windows.Forms.Button();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.btnReverse = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.treeView = new System.Windows.Forms.TreeView();
			this.roleLabel = new System.Windows.Forms.Label();
			this.collapseButton = new System.Windows.Forms.Button();
			this.aboutButton = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			resources.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Controls.Add(this.textBox);
			groupBox1.Name = "groupBox1";
			groupBox1.TabStop = false;
			// 
			// textBox
			// 
			resources.ApplyResources(this.textBox, "textBox");
			this.textBox.BackColor = System.Drawing.SystemColors.Control;
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			// 
			// scanButton
			// 
			resources.ApplyResources(this.scanButton, "scanButton");
			this.scanButton.Name = "scanButton";
			this.scanButton.UseVisualStyleBackColor = true;
			this.scanButton.Click += new System.EventHandler(this.ScanButton_Click);
			// 
			// btnOptimize
			// 
			resources.ApplyResources(this.btnOptimize, "btnOptimize");
			this.btnOptimize.Name = "btnOptimize";
			this.btnOptimize.UseVisualStyleBackColor = true;
			this.btnOptimize.Click += new System.EventHandler(this.OptimizeButton_Click);
			// 
			// btnSelectAll
			// 
			resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			this.btnSelectAll.Click += new System.EventHandler(this.SelectAllButton_Click);
			// 
			// btnReverse
			// 
			resources.ApplyResources(this.btnReverse, "btnReverse");
			this.btnReverse.Name = "btnReverse";
			this.btnReverse.UseVisualStyleBackColor = true;
			this.btnReverse.Click += new System.EventHandler(this.ReverseButton_Click);
			// 
			// progressBar
			// 
			resources.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			// 
			// treeView
			// 
			resources.ApplyResources(this.treeView, "treeView");
			this.treeView.CheckBoxes = true;
			this.treeView.FullRowSelect = true;
			this.treeView.Name = "treeView";
			this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterCheck);
			this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseClick);
			// 
			// roleLabel
			// 
			resources.ApplyResources(this.roleLabel, "roleLabel");
			this.roleLabel.BackColor = System.Drawing.SystemColors.Control;
			this.roleLabel.ForeColor = System.Drawing.Color.Green;
			this.roleLabel.Name = "roleLabel";
			// 
			// collapseButton
			// 
			resources.ApplyResources(this.collapseButton, "collapseButton");
			this.collapseButton.Name = "collapseButton";
			this.collapseButton.UseVisualStyleBackColor = true;
			this.collapseButton.Click += new System.EventHandler(this.CollapseButton_Click);
			// 
			// aboutButton
			// 
			resources.ApplyResources(this.aboutButton, "aboutButton");
			this.aboutButton.Name = "aboutButton";
			this.aboutButton.UseVisualStyleBackColor = true;
			this.aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
			// 
			// MainWindow
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.aboutButton);
			this.Controls.Add(this.collapseButton);
			this.Controls.Add(this.roleLabel);
			this.Controls.Add(this.treeView);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(groupBox1);
			this.Controls.Add(this.btnOptimize);
			this.Controls.Add(this.scanButton);
			this.Controls.Add(this.btnReverse);
			this.Controls.Add(this.btnSelectAll);
			this.Name = "MainWindow";
			groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

	}

	#endregion
	private System.Windows.Forms.Button scanButton;
	private System.Windows.Forms.Button btnOptimize;
	private System.Windows.Forms.Button btnSelectAll;
	private System.Windows.Forms.Button btnReverse;
	private System.Windows.Forms.ProgressBar progressBar;
	private System.Windows.Forms.TreeView treeView;
	private System.Windows.Forms.Label roleLabel;
	private System.Windows.Forms.RichTextBox textBox;
	private System.Windows.Forms.Button collapseButton;
	private System.Windows.Forms.Button aboutButton;
}

