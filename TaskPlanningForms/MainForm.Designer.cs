namespace TaskPlanningForms
{
	partial class MainForm
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
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.mainTabPage = new System.Windows.Forms.TabPage();
			this.iterationPathGroupBox = new System.Windows.Forms.GroupBox();
			this.iterationPathRemoveButton = new System.Windows.Forms.Button();
			this.iterationPathListBox = new System.Windows.Forms.ListBox();
			this.iterationPathAddButton = new System.Windows.Forms.Button();
			this.iterationsComboBox = new System.Windows.Forms.ComboBox();
			this.loadDataButton = new System.Windows.Forms.Button();
			this.areaPathGroupBox = new System.Windows.Forms.GroupBox();
			this.areaPathRemoveButton = new System.Windows.Forms.Button();
			this.areaPathAddButton = new System.Windows.Forms.Button();
			this.areaPathListBox = new System.Windows.Forms.ListBox();
			this.areaPathTextBox = new System.Windows.Forms.TextBox();
			this.loadLeadTasksButton = new System.Windows.Forms.Button();
			this.dataTabPage = new System.Windows.Forms.TabPage();
			this.refreshButton = new System.Windows.Forms.Button();
			this.usersLabel = new System.Windows.Forms.Label();
			this.usersСomboBox = new System.Windows.Forms.ComboBox();
			this.scheduleDataGridView = new System.Windows.Forms.DataGridView();
			this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LeadTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BlockedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AssignedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Past = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.settingsPage = new System.Windows.Forms.TabPage();
			this.setHolidaysButton = new System.Windows.Forms.Button();
			this.tfsUrlTextBox = new System.Windows.Forms.TextBox();
			this.tfsUrlLabel = new System.Windows.Forms.Label();
			this.devCmpletedCheckBox = new System.Windows.Forms.CheckBox();
			this.mainTabControl.SuspendLayout();
			this.mainTabPage.SuspendLayout();
			this.iterationPathGroupBox.SuspendLayout();
			this.areaPathGroupBox.SuspendLayout();
			this.dataTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).BeginInit();
			this.settingsPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.mainTabPage);
			this.mainTabControl.Controls.Add(this.dataTabPage);
			this.mainTabControl.Controls.Add(this.settingsPage);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 0);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(884, 567);
			this.mainTabControl.TabIndex = 0;
			// 
			// mainTabPage
			// 
			this.mainTabPage.Controls.Add(this.iterationPathGroupBox);
			this.mainTabPage.Controls.Add(this.areaPathGroupBox);
			this.mainTabPage.Location = new System.Drawing.Point(4, 22);
			this.mainTabPage.Name = "mainTabPage";
			this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.mainTabPage.Size = new System.Drawing.Size(876, 541);
			this.mainTabPage.TabIndex = 1;
			this.mainTabPage.Text = "Main";
			this.mainTabPage.UseVisualStyleBackColor = true;
			// 
			// iterationPathGroupBox
			// 
			this.iterationPathGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.iterationPathGroupBox.Controls.Add(this.devCmpletedCheckBox);
			this.iterationPathGroupBox.Controls.Add(this.iterationPathRemoveButton);
			this.iterationPathGroupBox.Controls.Add(this.iterationPathListBox);
			this.iterationPathGroupBox.Controls.Add(this.iterationPathAddButton);
			this.iterationPathGroupBox.Controls.Add(this.iterationsComboBox);
			this.iterationPathGroupBox.Controls.Add(this.loadDataButton);
			this.iterationPathGroupBox.Location = new System.Drawing.Point(408, 6);
			this.iterationPathGroupBox.Name = "iterationPathGroupBox";
			this.iterationPathGroupBox.Size = new System.Drawing.Size(460, 420);
			this.iterationPathGroupBox.TabIndex = 15;
			this.iterationPathGroupBox.TabStop = false;
			this.iterationPathGroupBox.Text = "Iteration path";
			// 
			// iterationPathRemoveButton
			// 
			this.iterationPathRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.iterationPathRemoveButton.Enabled = false;
			this.iterationPathRemoveButton.Location = new System.Drawing.Point(379, 391);
			this.iterationPathRemoveButton.Name = "iterationPathRemoveButton";
			this.iterationPathRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.iterationPathRemoveButton.TabIndex = 10;
			this.iterationPathRemoveButton.Text = "Remove";
			this.iterationPathRemoveButton.UseVisualStyleBackColor = true;
			this.iterationPathRemoveButton.Click += new System.EventHandler(this.IterationPathRemoveButtonClick);
			// 
			// iterationPathListBox
			// 
			this.iterationPathListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.iterationPathListBox.FormattingEnabled = true;
			this.iterationPathListBox.Location = new System.Drawing.Point(6, 45);
			this.iterationPathListBox.Name = "iterationPathListBox";
			this.iterationPathListBox.Size = new System.Drawing.Size(448, 342);
			this.iterationPathListBox.TabIndex = 9;
			// 
			// iterationPathAddButton
			// 
			this.iterationPathAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.iterationPathAddButton.Enabled = false;
			this.iterationPathAddButton.Location = new System.Drawing.Point(388, 17);
			this.iterationPathAddButton.Name = "iterationPathAddButton";
			this.iterationPathAddButton.Size = new System.Drawing.Size(62, 23);
			this.iterationPathAddButton.TabIndex = 8;
			this.iterationPathAddButton.Text = "Add";
			this.iterationPathAddButton.UseVisualStyleBackColor = true;
			this.iterationPathAddButton.Click += new System.EventHandler(this.IterationPathAddButtonClick);
			// 
			// iterationsComboBox
			// 
			this.iterationsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.iterationsComboBox.Enabled = false;
			this.iterationsComboBox.FormattingEnabled = true;
			this.iterationsComboBox.Location = new System.Drawing.Point(6, 19);
			this.iterationsComboBox.Name = "iterationsComboBox";
			this.iterationsComboBox.Size = new System.Drawing.Size(376, 21);
			this.iterationsComboBox.TabIndex = 6;
			// 
			// loadDataButton
			// 
			this.loadDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.loadDataButton.Enabled = false;
			this.loadDataButton.Location = new System.Drawing.Point(6, 391);
			this.loadDataButton.Name = "loadDataButton";
			this.loadDataButton.Size = new System.Drawing.Size(107, 23);
			this.loadDataButton.TabIndex = 4;
			this.loadDataButton.Text = "Make Schedule";
			this.loadDataButton.UseVisualStyleBackColor = true;
			this.loadDataButton.Click += new System.EventHandler(this.LoadDataButtonClick);
			// 
			// areaPathGroupBox
			// 
			this.areaPathGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.areaPathGroupBox.Controls.Add(this.areaPathRemoveButton);
			this.areaPathGroupBox.Controls.Add(this.areaPathAddButton);
			this.areaPathGroupBox.Controls.Add(this.areaPathListBox);
			this.areaPathGroupBox.Controls.Add(this.areaPathTextBox);
			this.areaPathGroupBox.Controls.Add(this.loadLeadTasksButton);
			this.areaPathGroupBox.Location = new System.Drawing.Point(8, 6);
			this.areaPathGroupBox.Name = "areaPathGroupBox";
			this.areaPathGroupBox.Size = new System.Drawing.Size(394, 420);
			this.areaPathGroupBox.TabIndex = 14;
			this.areaPathGroupBox.TabStop = false;
			this.areaPathGroupBox.Text = "Area path";
			// 
			// areaPathRemoveButton
			// 
			this.areaPathRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.areaPathRemoveButton.Location = new System.Drawing.Point(313, 391);
			this.areaPathRemoveButton.Name = "areaPathRemoveButton";
			this.areaPathRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.areaPathRemoveButton.TabIndex = 8;
			this.areaPathRemoveButton.Text = "Remove";
			this.areaPathRemoveButton.UseVisualStyleBackColor = true;
			this.areaPathRemoveButton.Click += new System.EventHandler(this.AreaPathRemoveButtonClick);
			// 
			// areaPathAddButton
			// 
			this.areaPathAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.areaPathAddButton.Location = new System.Drawing.Point(326, 17);
			this.areaPathAddButton.Name = "areaPathAddButton";
			this.areaPathAddButton.Size = new System.Drawing.Size(62, 23);
			this.areaPathAddButton.TabIndex = 7;
			this.areaPathAddButton.Text = "Add";
			this.areaPathAddButton.UseVisualStyleBackColor = true;
			this.areaPathAddButton.Click += new System.EventHandler(this.AreaPathAddButtonClick);
			// 
			// areaPathListBox
			// 
			this.areaPathListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.areaPathListBox.FormattingEnabled = true;
			this.areaPathListBox.Location = new System.Drawing.Point(6, 45);
			this.areaPathListBox.Name = "areaPathListBox";
			this.areaPathListBox.Size = new System.Drawing.Size(382, 342);
			this.areaPathListBox.TabIndex = 6;
			// 
			// areaPathTextBox
			// 
			this.areaPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.areaPathTextBox.Location = new System.Drawing.Point(6, 19);
			this.areaPathTextBox.Name = "areaPathTextBox";
			this.areaPathTextBox.Size = new System.Drawing.Size(314, 20);
			this.areaPathTextBox.TabIndex = 3;
			// 
			// loadLeadTasksButton
			// 
			this.loadLeadTasksButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.loadLeadTasksButton.Location = new System.Drawing.Point(6, 391);
			this.loadLeadTasksButton.Name = "loadLeadTasksButton";
			this.loadLeadTasksButton.Size = new System.Drawing.Size(107, 23);
			this.loadLeadTasksButton.TabIndex = 5;
			this.loadLeadTasksButton.Text = "Load LeadTasks";
			this.loadLeadTasksButton.UseVisualStyleBackColor = true;
			this.loadLeadTasksButton.Click += new System.EventHandler(this.LoadLeadTasksButtonClick);
			// 
			// dataTabPage
			// 
			this.dataTabPage.Controls.Add(this.refreshButton);
			this.dataTabPage.Controls.Add(this.usersLabel);
			this.dataTabPage.Controls.Add(this.usersСomboBox);
			this.dataTabPage.Controls.Add(this.scheduleDataGridView);
			this.dataTabPage.Location = new System.Drawing.Point(4, 22);
			this.dataTabPage.Name = "dataTabPage";
			this.dataTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.dataTabPage.Size = new System.Drawing.Size(876, 541);
			this.dataTabPage.TabIndex = 0;
			this.dataTabPage.Text = "Schedule";
			this.dataTabPage.UseVisualStyleBackColor = true;
			// 
			// refreshButton
			// 
			this.refreshButton.Location = new System.Drawing.Point(294, 4);
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(75, 23);
			this.refreshButton.TabIndex = 6;
			this.refreshButton.Text = "Refresh";
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler(this.RefreshButtonClick);
			// 
			// usersLabel
			// 
			this.usersLabel.AutoSize = true;
			this.usersLabel.Enabled = false;
			this.usersLabel.Location = new System.Drawing.Point(7, 9);
			this.usersLabel.Name = "usersLabel";
			this.usersLabel.Size = new System.Drawing.Size(66, 13);
			this.usersLabel.TabIndex = 5;
			this.usersLabel.Text = "Filter by user";
			// 
			// usersСomboBox
			// 
			this.usersСomboBox.Enabled = false;
			this.usersСomboBox.FormattingEnabled = true;
			this.usersСomboBox.Location = new System.Drawing.Point(79, 6);
			this.usersСomboBox.Name = "usersСomboBox";
			this.usersСomboBox.Size = new System.Drawing.Size(209, 21);
			this.usersСomboBox.TabIndex = 4;
			this.usersСomboBox.SelectionChangeCommitted += new System.EventHandler(this.UsersСomboBoxSelectionChangeCommitted);
			// 
			// scheduleDataGridView
			// 
			this.scheduleDataGridView.AllowUserToAddRows = false;
			this.scheduleDataGridView.AllowUserToDeleteRows = false;
			this.scheduleDataGridView.AllowUserToResizeRows = false;
			this.scheduleDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.scheduleDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.scheduleDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Priority,
            this.LeadTask,
            this.Task,
            this.BlockedBy,
            this.AssignedTo,
            this.Past});
			this.scheduleDataGridView.EnableHeadersVisualStyles = false;
			this.scheduleDataGridView.Location = new System.Drawing.Point(3, 33);
			this.scheduleDataGridView.Name = "scheduleDataGridView";
			this.scheduleDataGridView.ReadOnly = true;
			this.scheduleDataGridView.RowHeadersVisible = false;
			this.scheduleDataGridView.Size = new System.Drawing.Size(870, 505);
			this.scheduleDataGridView.TabIndex = 0;
			// 
			// Priority
			// 
			this.Priority.Frozen = true;
			this.Priority.HeaderText = "Pr";
			this.Priority.Name = "Priority";
			this.Priority.ReadOnly = true;
			this.Priority.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Priority.Width = 20;
			// 
			// LeadTask
			// 
			this.LeadTask.Frozen = true;
			this.LeadTask.HeaderText = "LeadTask";
			this.LeadTask.Name = "LeadTask";
			this.LeadTask.ReadOnly = true;
			this.LeadTask.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.LeadTask.Width = 60;
			// 
			// Task
			// 
			this.Task.Frozen = true;
			this.Task.HeaderText = "Task";
			this.Task.Name = "Task";
			this.Task.ReadOnly = true;
			this.Task.Width = 60;
			// 
			// BlockedBy
			// 
			this.BlockedBy.Frozen = true;
			this.BlockedBy.HeaderText = "BlockedBy";
			this.BlockedBy.Name = "BlockedBy";
			this.BlockedBy.ReadOnly = true;
			this.BlockedBy.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.BlockedBy.Width = 60;
			// 
			// AssignedTo
			// 
			this.AssignedTo.Frozen = true;
			this.AssignedTo.HeaderText = "AssignedTo";
			this.AssignedTo.Name = "AssignedTo";
			this.AssignedTo.ReadOnly = true;
			this.AssignedTo.Width = 80;
			// 
			// Past
			// 
			this.Past.HeaderText = "Past";
			this.Past.Name = "Past";
			this.Past.ReadOnly = true;
			this.Past.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Past.Width = 40;
			// 
			// settingsPage
			// 
			this.settingsPage.Controls.Add(this.setHolidaysButton);
			this.settingsPage.Controls.Add(this.tfsUrlTextBox);
			this.settingsPage.Controls.Add(this.tfsUrlLabel);
			this.settingsPage.Location = new System.Drawing.Point(4, 22);
			this.settingsPage.Name = "settingsPage";
			this.settingsPage.Size = new System.Drawing.Size(876, 541);
			this.settingsPage.TabIndex = 2;
			this.settingsPage.Text = "Settings";
			this.settingsPage.UseVisualStyleBackColor = true;
			// 
			// setHolidaysButton
			// 
			this.setHolidaysButton.Location = new System.Drawing.Point(8, 40);
			this.setHolidaysButton.Name = "setHolidaysButton";
			this.setHolidaysButton.Size = new System.Drawing.Size(117, 23);
			this.setHolidaysButton.TabIndex = 12;
			this.setHolidaysButton.Text = "Configure holidays";
			this.setHolidaysButton.UseVisualStyleBackColor = true;
			this.setHolidaysButton.Click += new System.EventHandler(this.SetHolidaysButtonClick);
			// 
			// tfsUrlTextBox
			// 
			this.tfsUrlTextBox.Location = new System.Drawing.Point(55, 14);
			this.tfsUrlTextBox.Name = "tfsUrlTextBox";
			this.tfsUrlTextBox.Size = new System.Drawing.Size(405, 20);
			this.tfsUrlTextBox.TabIndex = 11;
			// 
			// tfsUrlLabel
			// 
			this.tfsUrlLabel.AutoSize = true;
			this.tfsUrlLabel.Location = new System.Drawing.Point(5, 17);
			this.tfsUrlLabel.Name = "tfsUrlLabel";
			this.tfsUrlLabel.Size = new System.Drawing.Size(44, 13);
			this.tfsUrlLabel.TabIndex = 10;
			this.tfsUrlLabel.Text = "TFS url:";
			// 
			// devCmpletedCheckBox
			// 
			this.devCmpletedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.devCmpletedCheckBox.AutoSize = true;
			this.devCmpletedCheckBox.Checked = true;
			this.devCmpletedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.devCmpletedCheckBox.Location = new System.Drawing.Point(119, 395);
			this.devCmpletedCheckBox.Name = "devCmpletedCheckBox";
			this.devCmpletedCheckBox.Size = new System.Drawing.Size(121, 17);
			this.devCmpletedCheckBox.TabIndex = 11;
			this.devCmpletedCheckBox.Text = "with Dev Completed";
			this.devCmpletedCheckBox.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 567);
			this.Controls.Add(this.mainTabControl);
			this.Name = "MainForm";
			this.Text = "Task Scheduler";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.mainTabControl.ResumeLayout(false);
			this.mainTabPage.ResumeLayout(false);
			this.iterationPathGroupBox.ResumeLayout(false);
			this.iterationPathGroupBox.PerformLayout();
			this.areaPathGroupBox.ResumeLayout(false);
			this.areaPathGroupBox.PerformLayout();
			this.dataTabPage.ResumeLayout(false);
			this.dataTabPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).EndInit();
			this.settingsPage.ResumeLayout(false);
			this.settingsPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage dataTabPage;
		private System.Windows.Forms.DataGridView scheduleDataGridView;
		private System.Windows.Forms.TabPage mainTabPage;
		private System.Windows.Forms.TextBox areaPathTextBox;
		private System.Windows.Forms.Button loadDataButton;
		private System.Windows.Forms.Label usersLabel;
		private System.Windows.Forms.ComboBox usersСomboBox;
		private System.Windows.Forms.Button loadLeadTasksButton;
		private System.Windows.Forms.ComboBox iterationsComboBox;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.TabPage settingsPage;
		private System.Windows.Forms.TextBox tfsUrlTextBox;
		private System.Windows.Forms.Label tfsUrlLabel;
		private System.Windows.Forms.Button setHolidaysButton;
		private System.Windows.Forms.GroupBox areaPathGroupBox;
		private System.Windows.Forms.ListBox areaPathListBox;
		private System.Windows.Forms.Button areaPathAddButton;
		private System.Windows.Forms.Button areaPathRemoveButton;
		private System.Windows.Forms.GroupBox iterationPathGroupBox;
		private System.Windows.Forms.Button iterationPathAddButton;
		private System.Windows.Forms.ListBox iterationPathListBox;
		private System.Windows.Forms.Button iterationPathRemoveButton;
		private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
		private System.Windows.Forms.DataGridViewTextBoxColumn LeadTask;
		private System.Windows.Forms.DataGridViewTextBoxColumn Task;
		private System.Windows.Forms.DataGridViewTextBoxColumn BlockedBy;
		private System.Windows.Forms.DataGridViewTextBoxColumn AssignedTo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Past;
		private System.Windows.Forms.CheckBox devCmpletedCheckBox;
	}
}

