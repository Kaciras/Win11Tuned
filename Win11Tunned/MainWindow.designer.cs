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
		this.textBox = new System.Windows.Forms.RichTextBox();
		this.scanButton = new System.Windows.Forms.Button();
		this.btnOptimize = new System.Windows.Forms.Button();
		this.btnSelectAll = new System.Windows.Forms.Button();
		this.btnClearAll = new System.Windows.Forms.Button();
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
		groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
		| System.Windows.Forms.AnchorStyles.Right)));
		groupBox1.Controls.Add(this.textBox);
		groupBox1.Location = new System.Drawing.Point(500, 47);
		groupBox1.Margin = new System.Windows.Forms.Padding(5);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new System.Drawing.Size(170, 208);
		groupBox1.TabIndex = 6;
		groupBox1.TabStop = false;
		groupBox1.Text = "描述";
		// 
		// textBox
		// 
		this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
		| System.Windows.Forms.AnchorStyles.Left)
		| System.Windows.Forms.AnchorStyles.Right)));
		this.textBox.BackColor = System.Drawing.SystemColors.Control;
		this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox.Location = new System.Drawing.Point(6, 20);
		this.textBox.Name = "textBox";
		this.textBox.ReadOnly = true;
		this.textBox.Size = new System.Drawing.Size(158, 182);
		this.textBox.TabIndex = 0;
		this.textBox.Text = "";
		// 
		// scanButton
		// 
		this.scanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.scanButton.Location = new System.Drawing.Point(500, 337);
		this.scanButton.Margin = new System.Windows.Forms.Padding(5);
		this.scanButton.Name = "scanButton";
		this.scanButton.Size = new System.Drawing.Size(80, 28);
		this.scanButton.TabIndex = 2;
		this.scanButton.Text = "扫描";
		this.scanButton.UseVisualStyleBackColor = true;
		this.scanButton.Click += new System.EventHandler(this.ScanButton_Click);
		// 
		// btnOptimize
		// 
		this.btnOptimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.btnOptimize.Enabled = false;
		this.btnOptimize.Location = new System.Drawing.Point(590, 337);
		this.btnOptimize.Margin = new System.Windows.Forms.Padding(5);
		this.btnOptimize.Name = "btnOptimize";
		this.btnOptimize.Size = new System.Drawing.Size(80, 28);
		this.btnOptimize.TabIndex = 3;
		this.btnOptimize.Text = "优化";
		this.btnOptimize.UseVisualStyleBackColor = true;
		this.btnOptimize.Click += new System.EventHandler(this.OptimizeButton_Click);
		// 
		// btnSelectAll
		// 
		this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.btnSelectAll.Enabled = false;
		this.btnSelectAll.Location = new System.Drawing.Point(500, 299);
		this.btnSelectAll.Margin = new System.Windows.Forms.Padding(5);
		this.btnSelectAll.Name = "btnSelectAll";
		this.btnSelectAll.Size = new System.Drawing.Size(80, 28);
		this.btnSelectAll.TabIndex = 4;
		this.btnSelectAll.Text = "全选";
		this.btnSelectAll.UseVisualStyleBackColor = true;
		this.btnSelectAll.Click += new System.EventHandler(this.SelectAllButton_Click);
		// 
		// btnClearAll
		// 
		this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.btnClearAll.Enabled = false;
		this.btnClearAll.Location = new System.Drawing.Point(590, 299);
		this.btnClearAll.Margin = new System.Windows.Forms.Padding(5);
		this.btnClearAll.Name = "btnClearAll";
		this.btnClearAll.Size = new System.Drawing.Size(80, 28);
		this.btnClearAll.TabIndex = 5;
		this.btnClearAll.Text = "全不选";
		this.btnClearAll.UseVisualStyleBackColor = true;
		this.btnClearAll.Click += new System.EventHandler(this.ClearAllButton_Click);
		// 
		// progressBar
		// 
		this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
		| System.Windows.Forms.AnchorStyles.Right)));
		this.progressBar.Location = new System.Drawing.Point(14, 375);
		this.progressBar.Margin = new System.Windows.Forms.Padding(5);
		this.progressBar.Name = "progressBar";
		this.progressBar.Size = new System.Drawing.Size(656, 24);
		this.progressBar.TabIndex = 7;
		// 
		// treeView
		// 
		this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
		| System.Windows.Forms.AnchorStyles.Left)
		| System.Windows.Forms.AnchorStyles.Right)));
		this.treeView.CheckBoxes = true;
		this.treeView.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
		this.treeView.FullRowSelect = true;
		this.treeView.Location = new System.Drawing.Point(14, 14);
		this.treeView.Margin = new System.Windows.Forms.Padding(5);
		this.treeView.Name = "treeView";
		this.treeView.Size = new System.Drawing.Size(476, 351);
		this.treeView.TabIndex = 8;
		this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterCheck);
		this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseClick);
		// 
		// roleLabel
		// 
		this.roleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this.roleLabel.BackColor = System.Drawing.SystemColors.Control;
		this.roleLabel.Font = new System.Drawing.Font("Microsoft YaHei Light", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
		this.roleLabel.ForeColor = System.Drawing.Color.Green;
		this.roleLabel.Location = new System.Drawing.Point(500, 14);
		this.roleLabel.Margin = new System.Windows.Forms.Padding(5);
		this.roleLabel.Name = "roleLabel";
		this.roleLabel.Size = new System.Drawing.Size(170, 23);
		this.roleLabel.TabIndex = 9;
		this.roleLabel.Text = "标准权限";
		this.roleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		// 
		// collapseButton
		// 
		this.collapseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.collapseButton.Enabled = false;
		this.collapseButton.Location = new System.Drawing.Point(500, 263);
		this.collapseButton.Name = "collapseButton";
		this.collapseButton.Size = new System.Drawing.Size(80, 28);
		this.collapseButton.TabIndex = 10;
		this.collapseButton.Text = "全部折叠";
		this.collapseButton.UseVisualStyleBackColor = true;
		this.collapseButton.Click += new System.EventHandler(this.CollapseButton_Click);
		// 
		// aboutButton
		// 
		this.aboutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.aboutButton.Location = new System.Drawing.Point(590, 263);
		this.aboutButton.Name = "aboutButton";
		this.aboutButton.Size = new System.Drawing.Size(80, 28);
		this.aboutButton.TabIndex = 11;
		this.aboutButton.Text = "关于";
		this.aboutButton.UseVisualStyleBackColor = true;
		this.aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
		// 
		// MainWindow
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(684, 411);
		this.Controls.Add(this.aboutButton);
		this.Controls.Add(this.collapseButton);
		this.Controls.Add(this.roleLabel);
		this.Controls.Add(this.treeView);
		this.Controls.Add(this.progressBar);
		this.Controls.Add(groupBox1);
		this.Controls.Add(this.btnOptimize);
		this.Controls.Add(this.scanButton);
		this.Controls.Add(this.btnClearAll);
		this.Controls.Add(this.btnSelectAll);
		this.MinimumSize = new System.Drawing.Size(600, 450);
		this.Name = "MainWindow";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Kaciras 的 Win8 优化工具";
		groupBox1.ResumeLayout(false);
		this.ResumeLayout(false);

	}

	#endregion
	private System.Windows.Forms.Button scanButton;
	private System.Windows.Forms.Button btnOptimize;
	private System.Windows.Forms.Button btnSelectAll;
	private System.Windows.Forms.Button btnClearAll;
	private System.Windows.Forms.ProgressBar progressBar;
	private System.Windows.Forms.TreeView treeView;
	private System.Windows.Forms.Label roleLabel;
	private System.Windows.Forms.RichTextBox textBox;
	private System.Windows.Forms.Button collapseButton;
	private System.Windows.Forms.Button aboutButton;
}

