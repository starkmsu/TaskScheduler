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
			this.settingsTabPage = new System.Windows.Forms.TabPage();
			this.setHolidaysButton = new System.Windows.Forms.Button();
			this.iterationsComboBox = new System.Windows.Forms.ComboBox();
			this.loadLeadTasksButton = new System.Windows.Forms.Button();
			this.loadDataButton = new System.Windows.Forms.Button();
			this.areaPathTextBox = new System.Windows.Forms.TextBox();
			this.areaPathLabel = new System.Windows.Forms.Label();
			this.iterationPathLabel = new System.Windows.Forms.Label();
			this.dataTabPage = new System.Windows.Forms.TabPage();
			this.usersLabel = new System.Windows.Forms.Label();
			this.usersСomboBox = new System.Windows.Forms.ComboBox();
			this.scheduleDataGridView = new System.Windows.Forms.DataGridView();
			this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LeadTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BlockedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AssignedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Past = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mainTabControl.SuspendLayout();
			this.settingsTabPage.SuspendLayout();
			this.dataTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.settingsTabPage);
			this.mainTabControl.Controls.Add(this.dataTabPage);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 0);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(884, 567);
			this.mainTabControl.TabIndex = 0;
			// 
			// settingsTabPage
			// 
			this.settingsTabPage.Controls.Add(this.setHolidaysButton);
			this.settingsTabPage.Controls.Add(this.iterationsComboBox);
			this.settingsTabPage.Controls.Add(this.loadLeadTasksButton);
			this.settingsTabPage.Controls.Add(this.loadDataButton);
			this.settingsTabPage.Controls.Add(this.areaPathTextBox);
			this.settingsTabPage.Controls.Add(this.areaPathLabel);
			this.settingsTabPage.Controls.Add(this.iterationPathLabel);
			this.settingsTabPage.Location = new System.Drawing.Point(4, 22);
			this.settingsTabPage.Name = "settingsTabPage";
			this.settingsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.settingsTabPage.Size = new System.Drawing.Size(876, 541);
			this.settingsTabPage.TabIndex = 1;
			this.settingsTabPage.Text = "Settings";
			this.settingsTabPage.UseVisualStyleBackColor = true;
			// 
			// setHolidaysButton
			// 
			this.setHolidaysButton.Location = new System.Drawing.Point(10, 102);
			this.setHolidaysButton.Name = "setHolidaysButton";
			this.setHolidaysButton.Size = new System.Drawing.Size(117, 23);
			this.setHolidaysButton.TabIndex = 7;
			this.setHolidaysButton.Text = "Configure holidays";
			this.setHolidaysButton.UseVisualStyleBackColor = true;
			this.setHolidaysButton.Click += new System.EventHandler(this.SetHolidaysButtonClick);
			// 
			// iterationsComboBox
			// 
			this.iterationsComboBox.Enabled = false;
			this.iterationsComboBox.FormattingEnabled = true;
			this.iterationsComboBox.Location = new System.Drawing.Point(86, 37);
			this.iterationsComboBox.Name = "iterationsComboBox";
			this.iterationsComboBox.Size = new System.Drawing.Size(376, 21);
			this.iterationsComboBox.TabIndex = 6;
			// 
			// loadLeadTasksButton
			// 
			this.loadLeadTasksButton.Location = new System.Drawing.Point(468, 8);
			this.loadLeadTasksButton.Name = "loadLeadTasksButton";
			this.loadLeadTasksButton.Size = new System.Drawing.Size(107, 23);
			this.loadLeadTasksButton.TabIndex = 5;
			this.loadLeadTasksButton.Text = "Load LeadTasks";
			this.loadLeadTasksButton.UseVisualStyleBackColor = true;
			this.loadLeadTasksButton.Click += new System.EventHandler(this.LoadLeadTasksButtonClick);
			// 
			// loadDataButton
			// 
			this.loadDataButton.Enabled = false;
			this.loadDataButton.Location = new System.Drawing.Point(468, 35);
			this.loadDataButton.Name = "loadDataButton";
			this.loadDataButton.Size = new System.Drawing.Size(107, 23);
			this.loadDataButton.TabIndex = 4;
			this.loadDataButton.Text = "Make Schedule";
			this.loadDataButton.UseVisualStyleBackColor = true;
			this.loadDataButton.Click += new System.EventHandler(this.LoadDataButtonClick);
			// 
			// areaPathTextBox
			// 
			this.areaPathTextBox.Location = new System.Drawing.Point(86, 11);
			this.areaPathTextBox.Name = "areaPathTextBox";
			this.areaPathTextBox.Size = new System.Drawing.Size(376, 20);
			this.areaPathTextBox.TabIndex = 3;
			this.areaPathTextBox.Text = "FORIS_Mobile\\Product Management Domain\\Order Catalogue";
			// 
			// areaPathLabel
			// 
			this.areaPathLabel.AutoSize = true;
			this.areaPathLabel.Location = new System.Drawing.Point(7, 14);
			this.areaPathLabel.Name = "areaPathLabel";
			this.areaPathLabel.Size = new System.Drawing.Size(57, 13);
			this.areaPathLabel.TabIndex = 2;
			this.areaPathLabel.Text = "Area Path:";
			// 
			// iterationPathLabel
			// 
			this.iterationPathLabel.AutoSize = true;
			this.iterationPathLabel.Location = new System.Drawing.Point(7, 40);
			this.iterationPathLabel.Name = "iterationPathLabel";
			this.iterationPathLabel.Size = new System.Drawing.Size(73, 13);
			this.iterationPathLabel.TabIndex = 0;
			this.iterationPathLabel.Text = "Iteration Path:";
			// 
			// dataTabPage
			// 
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
			this.scheduleDataGridView.AllowUserToResizeColumns = false;
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
			this.Priority.Width = 20;
			// 
			// LeadTask
			// 
			this.LeadTask.Frozen = true;
			this.LeadTask.HeaderText = "LeadTask";
			this.LeadTask.Name = "LeadTask";
			this.LeadTask.ReadOnly = true;
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
			this.Past.Width = 40;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 567);
			this.Controls.Add(this.mainTabControl);
			this.Name = "MainForm";
			this.Text = "Task Planning";
			this.mainTabControl.ResumeLayout(false);
			this.settingsTabPage.ResumeLayout(false);
			this.settingsTabPage.PerformLayout();
			this.dataTabPage.ResumeLayout(false);
			this.dataTabPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage dataTabPage;
		private System.Windows.Forms.DataGridView scheduleDataGridView;
		private System.Windows.Forms.TabPage settingsTabPage;
		private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
		private System.Windows.Forms.DataGridViewTextBoxColumn LeadTask;
		private System.Windows.Forms.DataGridViewTextBoxColumn Task;
		private System.Windows.Forms.DataGridViewTextBoxColumn BlockedBy;
		private System.Windows.Forms.DataGridViewTextBoxColumn AssignedTo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Past;
		private System.Windows.Forms.TextBox areaPathTextBox;
		private System.Windows.Forms.Label areaPathLabel;
		private System.Windows.Forms.Label iterationPathLabel;
		private System.Windows.Forms.Button loadDataButton;
		private System.Windows.Forms.Label usersLabel;
		private System.Windows.Forms.ComboBox usersСomboBox;
		private System.Windows.Forms.Button loadLeadTasksButton;
		private System.Windows.Forms.ComboBox iterationsComboBox;
		private System.Windows.Forms.Button setHolidaysButton;
	}
}

