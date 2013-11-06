namespace TaskSchedulerForms
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
			this.components = new System.ComponentModel.Container();
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.mainTabPage = new System.Windows.Forms.TabPage();
			this.exchangeButton = new System.Windows.Forms.Button();
			this.subTreesCheckBox = new System.Windows.Forms.CheckBox();
			this.secondGroupBox = new System.Windows.Forms.GroupBox();
			this.secondRemoveButton = new System.Windows.Forms.Button();
			this.secondListBox = new System.Windows.Forms.ListBox();
			this.secondAddButton = new System.Windows.Forms.Button();
			this.secondComboBox = new System.Windows.Forms.ComboBox();
			this.firstGroupBox = new System.Windows.Forms.GroupBox();
			this.firstRemoveButton = new System.Windows.Forms.Button();
			this.firstAddButton = new System.Windows.Forms.Button();
			this.firstListBox = new System.Windows.Forms.ListBox();
			this.firstTextBox = new System.Windows.Forms.TextBox();
			this.loadLeadTasksButton = new System.Windows.Forms.Button();
			this.makeScheduleButton = new System.Windows.Forms.Button();
			this.dataTabPage = new System.Windows.Forms.TabPage();
			this.expandBlockersCheckBox = new System.Windows.Forms.CheckBox();
			this.ltOnlyCheckBox = new System.Windows.Forms.CheckBox();
			this.devCmpletedCheckBox = new System.Windows.Forms.CheckBox();
			this.refreshButton = new System.Windows.Forms.Button();
			this.usersLabel = new System.Windows.Forms.Label();
			this.usersFilterСomboBox = new System.Windows.Forms.ComboBox();
			this.scheduleDataGridView = new System.Windows.Forms.DataGridView();
			this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LeadTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Docs = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Blockers = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AssignedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Past = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.settingsPage = new System.Windows.Forms.TabPage();
			this.vacationsButton = new System.Windows.Forms.Button();
			this.usersVacationsComboBox = new System.Windows.Forms.ComboBox();
			this.holidaysButton = new System.Windows.Forms.Button();
			this.tfsUrlTextBox = new System.Windows.Forms.TextBox();
			this.tfsUrlLabel = new System.Windows.Forms.Label();
			this.secondToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.mainTabControl.SuspendLayout();
			this.mainTabPage.SuspendLayout();
			this.secondGroupBox.SuspendLayout();
			this.firstGroupBox.SuspendLayout();
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
			this.mainTabControl.Size = new System.Drawing.Size(601, 505);
			this.mainTabControl.TabIndex = 0;
			// 
			// mainTabPage
			// 
			this.mainTabPage.Controls.Add(this.exchangeButton);
			this.mainTabPage.Controls.Add(this.subTreesCheckBox);
			this.mainTabPage.Controls.Add(this.secondGroupBox);
			this.mainTabPage.Controls.Add(this.firstGroupBox);
			this.mainTabPage.Controls.Add(this.loadLeadTasksButton);
			this.mainTabPage.Controls.Add(this.makeScheduleButton);
			this.mainTabPage.Location = new System.Drawing.Point(4, 22);
			this.mainTabPage.Name = "mainTabPage";
			this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.mainTabPage.Size = new System.Drawing.Size(593, 479);
			this.mainTabPage.TabIndex = 1;
			this.mainTabPage.Text = "Main";
			this.mainTabPage.UseVisualStyleBackColor = true;
			// 
			// exchangeButton
			// 
			this.exchangeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.exchangeButton.Image = global::TaskSchedulerForms.Properties.Resources.Exchange;
			this.exchangeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.exchangeButton.Location = new System.Drawing.Point(504, 186);
			this.exchangeButton.Name = "exchangeButton";
			this.exchangeButton.Size = new System.Drawing.Size(75, 23);
			this.exchangeButton.TabIndex = 17;
			this.exchangeButton.Text = "Exchange";
			this.exchangeButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.exchangeButton.UseVisualStyleBackColor = true;
			this.exchangeButton.Click += new System.EventHandler(this.ExchangeButtonClick);
			// 
			// subTreesCheckBox
			// 
			this.subTreesCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.subTreesCheckBox.AutoSize = true;
			this.subTreesCheckBox.Checked = true;
			this.subTreesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.subTreesCheckBox.Location = new System.Drawing.Point(342, 190);
			this.subTreesCheckBox.Name = "subTreesCheckBox";
			this.subTreesCheckBox.Size = new System.Drawing.Size(92, 17);
			this.subTreesCheckBox.TabIndex = 16;
			this.subTreesCheckBox.Text = "with subTrees";
			this.subTreesCheckBox.UseVisualStyleBackColor = true;
			// 
			// secondGroupBox
			// 
			this.secondGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondGroupBox.Controls.Add(this.secondRemoveButton);
			this.secondGroupBox.Controls.Add(this.secondListBox);
			this.secondGroupBox.Controls.Add(this.secondAddButton);
			this.secondGroupBox.Controls.Add(this.secondComboBox);
			this.secondGroupBox.Location = new System.Drawing.Point(14, 213);
			this.secondGroupBox.Name = "secondGroupBox";
			this.secondGroupBox.Size = new System.Drawing.Size(571, 229);
			this.secondGroupBox.TabIndex = 15;
			this.secondGroupBox.TabStop = false;
			this.secondGroupBox.Text = "Iteration";
			// 
			// secondRemoveButton
			// 
			this.secondRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.secondRemoveButton.Enabled = false;
			this.secondRemoveButton.Location = new System.Drawing.Point(490, 17);
			this.secondRemoveButton.Name = "secondRemoveButton";
			this.secondRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.secondRemoveButton.TabIndex = 10;
			this.secondRemoveButton.Text = "Remove";
			this.secondRemoveButton.UseVisualStyleBackColor = true;
			this.secondRemoveButton.Click += new System.EventHandler(this.SecondRemoveButtonClick);
			// 
			// secondListBox
			// 
			this.secondListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondListBox.FormattingEnabled = true;
			this.secondListBox.Location = new System.Drawing.Point(6, 45);
			this.secondListBox.Name = "secondListBox";
			this.secondListBox.Size = new System.Drawing.Size(559, 160);
			this.secondListBox.TabIndex = 9;
			// 
			// secondAddButton
			// 
			this.secondAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.secondAddButton.Enabled = false;
			this.secondAddButton.Location = new System.Drawing.Point(422, 17);
			this.secondAddButton.Name = "secondAddButton";
			this.secondAddButton.Size = new System.Drawing.Size(62, 23);
			this.secondAddButton.TabIndex = 8;
			this.secondAddButton.Text = "Add";
			this.secondAddButton.UseVisualStyleBackColor = true;
			this.secondAddButton.Click += new System.EventHandler(this.SecondAddButtonClick);
			// 
			// secondComboBox
			// 
			this.secondComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondComboBox.Enabled = false;
			this.secondComboBox.FormattingEnabled = true;
			this.secondComboBox.Location = new System.Drawing.Point(6, 19);
			this.secondComboBox.Name = "secondComboBox";
			this.secondComboBox.Size = new System.Drawing.Size(410, 21);
			this.secondComboBox.TabIndex = 6;
			// 
			// firstGroupBox
			// 
			this.firstGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.firstGroupBox.Controls.Add(this.firstRemoveButton);
			this.firstGroupBox.Controls.Add(this.firstAddButton);
			this.firstGroupBox.Controls.Add(this.firstListBox);
			this.firstGroupBox.Controls.Add(this.firstTextBox);
			this.firstGroupBox.Location = new System.Drawing.Point(8, 6);
			this.firstGroupBox.Name = "firstGroupBox";
			this.firstGroupBox.Size = new System.Drawing.Size(577, 174);
			this.firstGroupBox.TabIndex = 14;
			this.firstGroupBox.TabStop = false;
			this.firstGroupBox.Text = "Area";
			// 
			// firstRemoveButton
			// 
			this.firstRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.firstRemoveButton.Location = new System.Drawing.Point(496, 17);
			this.firstRemoveButton.Name = "firstRemoveButton";
			this.firstRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.firstRemoveButton.TabIndex = 8;
			this.firstRemoveButton.Text = "Remove";
			this.firstRemoveButton.UseVisualStyleBackColor = true;
			this.firstRemoveButton.Click += new System.EventHandler(this.FirstRemoveButtonClick);
			// 
			// firstAddButton
			// 
			this.firstAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.firstAddButton.Location = new System.Drawing.Point(428, 17);
			this.firstAddButton.Name = "firstAddButton";
			this.firstAddButton.Size = new System.Drawing.Size(62, 23);
			this.firstAddButton.TabIndex = 7;
			this.firstAddButton.Text = "Add";
			this.firstAddButton.UseVisualStyleBackColor = true;
			this.firstAddButton.Click += new System.EventHandler(this.FirstAddButtonClick);
			// 
			// firstListBox
			// 
			this.firstListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.firstListBox.FormattingEnabled = true;
			this.firstListBox.Location = new System.Drawing.Point(6, 45);
			this.firstListBox.Name = "firstListBox";
			this.firstListBox.Size = new System.Drawing.Size(565, 108);
			this.firstListBox.TabIndex = 6;
			// 
			// firstTextBox
			// 
			this.firstTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.firstTextBox.Location = new System.Drawing.Point(6, 19);
			this.firstTextBox.Name = "firstTextBox";
			this.firstTextBox.Size = new System.Drawing.Size(416, 20);
			this.firstTextBox.TabIndex = 3;
			// 
			// loadLeadTasksButton
			// 
			this.loadLeadTasksButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.loadLeadTasksButton.Location = new System.Drawing.Point(229, 186);
			this.loadLeadTasksButton.Name = "loadLeadTasksButton";
			this.loadLeadTasksButton.Size = new System.Drawing.Size(107, 23);
			this.loadLeadTasksButton.TabIndex = 5;
			this.loadLeadTasksButton.Text = "Load LeadTasks";
			this.loadLeadTasksButton.UseVisualStyleBackColor = true;
			this.loadLeadTasksButton.Click += new System.EventHandler(this.LoadLeadTasksButtonClick);
			// 
			// makeScheduleButton
			// 
			this.makeScheduleButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.makeScheduleButton.Enabled = false;
			this.makeScheduleButton.Location = new System.Drawing.Point(229, 448);
			this.makeScheduleButton.Name = "makeScheduleButton";
			this.makeScheduleButton.Size = new System.Drawing.Size(107, 23);
			this.makeScheduleButton.TabIndex = 4;
			this.makeScheduleButton.Text = "Make Schedule";
			this.makeScheduleButton.UseVisualStyleBackColor = true;
			this.makeScheduleButton.Click += new System.EventHandler(this.MakeScheduleButtonClick);
			// 
			// dataTabPage
			// 
			this.dataTabPage.Controls.Add(this.expandBlockersCheckBox);
			this.dataTabPage.Controls.Add(this.ltOnlyCheckBox);
			this.dataTabPage.Controls.Add(this.devCmpletedCheckBox);
			this.dataTabPage.Controls.Add(this.refreshButton);
			this.dataTabPage.Controls.Add(this.usersLabel);
			this.dataTabPage.Controls.Add(this.usersFilterСomboBox);
			this.dataTabPage.Controls.Add(this.scheduleDataGridView);
			this.dataTabPage.Location = new System.Drawing.Point(4, 22);
			this.dataTabPage.Name = "dataTabPage";
			this.dataTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.dataTabPage.Size = new System.Drawing.Size(593, 479);
			this.dataTabPage.TabIndex = 0;
			this.dataTabPage.Text = "Schedule";
			this.dataTabPage.UseVisualStyleBackColor = true;
			// 
			// expandBlockersCheckBox
			// 
			this.expandBlockersCheckBox.AutoSize = true;
			this.expandBlockersCheckBox.Location = new System.Drawing.Point(571, 8);
			this.expandBlockersCheckBox.Name = "expandBlockersCheckBox";
			this.expandBlockersCheckBox.Size = new System.Drawing.Size(106, 17);
			this.expandBlockersCheckBox.TabIndex = 14;
			this.expandBlockersCheckBox.Text = "Expand Blockers";
			this.expandBlockersCheckBox.UseVisualStyleBackColor = true;
			this.expandBlockersCheckBox.CheckedChanged += new System.EventHandler(this.ShowBlockersCheckBoxCheckedChanged);
			// 
			// ltOnlyCheckBox
			// 
			this.ltOnlyCheckBox.AutoSize = true;
			this.ltOnlyCheckBox.Location = new System.Drawing.Point(503, 8);
			this.ltOnlyCheckBox.Name = "ltOnlyCheckBox";
			this.ltOnlyCheckBox.Size = new System.Drawing.Size(61, 17);
			this.ltOnlyCheckBox.TabIndex = 13;
			this.ltOnlyCheckBox.Text = "LT only";
			this.ltOnlyCheckBox.UseVisualStyleBackColor = true;
			this.ltOnlyCheckBox.CheckedChanged += new System.EventHandler(this.LtOnlyCheckBoxCheckedChanged);
			// 
			// devCmpletedCheckBox
			// 
			this.devCmpletedCheckBox.AutoSize = true;
			this.devCmpletedCheckBox.Checked = true;
			this.devCmpletedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.devCmpletedCheckBox.Location = new System.Drawing.Point(376, 8);
			this.devCmpletedCheckBox.Name = "devCmpletedCheckBox";
			this.devCmpletedCheckBox.Size = new System.Drawing.Size(124, 17);
			this.devCmpletedCheckBox.TabIndex = 12;
			this.devCmpletedCheckBox.Text = "With Dev Completed";
			this.devCmpletedCheckBox.UseVisualStyleBackColor = true;
			this.devCmpletedCheckBox.CheckedChanged += new System.EventHandler(this.DevCmpletedCheckBoxCheckedChanged);
			// 
			// refreshButton
			// 
			this.refreshButton.Enabled = false;
			this.refreshButton.Location = new System.Drawing.Point(8, 4);
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
			this.usersLabel.Location = new System.Drawing.Point(89, 9);
			this.usersLabel.Name = "usersLabel";
			this.usersLabel.Size = new System.Drawing.Size(66, 13);
			this.usersLabel.TabIndex = 5;
			this.usersLabel.Text = "Filter by user";
			// 
			// usersFilterСomboBox
			// 
			this.usersFilterСomboBox.Enabled = false;
			this.usersFilterСomboBox.FormattingEnabled = true;
			this.usersFilterСomboBox.Location = new System.Drawing.Point(161, 6);
			this.usersFilterСomboBox.Name = "usersFilterСomboBox";
			this.usersFilterСomboBox.Size = new System.Drawing.Size(209, 21);
			this.usersFilterСomboBox.TabIndex = 4;
			this.usersFilterСomboBox.SelectionChangeCommitted += new System.EventHandler(this.UsersFilterСomboBoxSelectionChangeCommitted);
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
            this.Docs,
            this.Task,
            this.Blockers,
            this.AssignedTo,
            this.Past});
			this.scheduleDataGridView.EnableHeadersVisualStyles = false;
			this.scheduleDataGridView.Location = new System.Drawing.Point(3, 33);
			this.scheduleDataGridView.Name = "scheduleDataGridView";
			this.scheduleDataGridView.ReadOnly = true;
			this.scheduleDataGridView.RowHeadersVisible = false;
			this.scheduleDataGridView.Size = new System.Drawing.Size(590, 446);
			this.scheduleDataGridView.TabIndex = 0;
			// 
			// Priority
			// 
			this.Priority.Frozen = true;
			this.Priority.HeaderText = "Pr";
			this.Priority.Name = "Priority";
			this.Priority.ReadOnly = true;
			this.Priority.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Priority.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Priority.Width = 30;
			// 
			// LeadTask
			// 
			this.LeadTask.Frozen = true;
			this.LeadTask.HeaderText = "LeadTask";
			this.LeadTask.Name = "LeadTask";
			this.LeadTask.ReadOnly = true;
			this.LeadTask.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.LeadTask.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LeadTask.Width = 60;
			// 
			// Docs
			// 
			this.Docs.Frozen = true;
			this.Docs.HeaderText = "Docs";
			this.Docs.Name = "Docs";
			this.Docs.ReadOnly = true;
			this.Docs.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Docs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Docs.Width = 50;
			// 
			// Task
			// 
			this.Task.Frozen = true;
			this.Task.HeaderText = "Task";
			this.Task.Name = "Task";
			this.Task.ReadOnly = true;
			this.Task.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Task.Width = 200;
			// 
			// Blockers
			// 
			this.Blockers.Frozen = true;
			this.Blockers.HeaderText = "Blockers";
			this.Blockers.Name = "Blockers";
			this.Blockers.ReadOnly = true;
			this.Blockers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Blockers.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Blockers.Width = 60;
			// 
			// AssignedTo
			// 
			this.AssignedTo.Frozen = true;
			this.AssignedTo.HeaderText = "AssignedTo";
			this.AssignedTo.Name = "AssignedTo";
			this.AssignedTo.ReadOnly = true;
			this.AssignedTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.AssignedTo.Width = 80;
			// 
			// Past
			// 
			this.Past.HeaderText = "Past";
			this.Past.Name = "Past";
			this.Past.ReadOnly = true;
			this.Past.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Past.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Past.Width = 40;
			// 
			// settingsPage
			// 
			this.settingsPage.Controls.Add(this.vacationsButton);
			this.settingsPage.Controls.Add(this.usersVacationsComboBox);
			this.settingsPage.Controls.Add(this.holidaysButton);
			this.settingsPage.Controls.Add(this.tfsUrlTextBox);
			this.settingsPage.Controls.Add(this.tfsUrlLabel);
			this.settingsPage.Location = new System.Drawing.Point(4, 22);
			this.settingsPage.Name = "settingsPage";
			this.settingsPage.Size = new System.Drawing.Size(593, 479);
			this.settingsPage.TabIndex = 2;
			this.settingsPage.Text = "Settings";
			this.settingsPage.UseVisualStyleBackColor = true;
			// 
			// vacationsButton
			// 
			this.vacationsButton.Location = new System.Drawing.Point(302, 67);
			this.vacationsButton.Name = "vacationsButton";
			this.vacationsButton.Size = new System.Drawing.Size(158, 23);
			this.vacationsButton.TabIndex = 14;
			this.vacationsButton.Text = "Configure Vacations";
			this.vacationsButton.UseVisualStyleBackColor = true;
			this.vacationsButton.Click += new System.EventHandler(this.VacationsButtonClick);
			// 
			// usersVacationsComboBox
			// 
			this.usersVacationsComboBox.FormattingEnabled = true;
			this.usersVacationsComboBox.Location = new System.Drawing.Point(8, 69);
			this.usersVacationsComboBox.Name = "usersVacationsComboBox";
			this.usersVacationsComboBox.Size = new System.Drawing.Size(288, 21);
			this.usersVacationsComboBox.TabIndex = 13;
			// 
			// holidaysButton
			// 
			this.holidaysButton.Location = new System.Drawing.Point(8, 40);
			this.holidaysButton.Name = "holidaysButton";
			this.holidaysButton.Size = new System.Drawing.Size(117, 23);
			this.holidaysButton.TabIndex = 12;
			this.holidaysButton.Text = "Configure holidays";
			this.holidaysButton.UseVisualStyleBackColor = true;
			this.holidaysButton.Click += new System.EventHandler(this.HolidaysButtonClick);
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(601, 505);
			this.Controls.Add(this.mainTabControl);
			this.Name = "MainForm";
			this.Text = "Task Scheduler";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.mainTabControl.ResumeLayout(false);
			this.mainTabPage.ResumeLayout(false);
			this.mainTabPage.PerformLayout();
			this.secondGroupBox.ResumeLayout(false);
			this.firstGroupBox.ResumeLayout(false);
			this.firstGroupBox.PerformLayout();
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
		private System.Windows.Forms.TextBox firstTextBox;
		private System.Windows.Forms.Button makeScheduleButton;
		private System.Windows.Forms.Label usersLabel;
		private System.Windows.Forms.ComboBox usersFilterСomboBox;
		private System.Windows.Forms.Button loadLeadTasksButton;
		private System.Windows.Forms.ComboBox secondComboBox;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.TabPage settingsPage;
		private System.Windows.Forms.TextBox tfsUrlTextBox;
		private System.Windows.Forms.Label tfsUrlLabel;
		private System.Windows.Forms.Button holidaysButton;
		private System.Windows.Forms.GroupBox firstGroupBox;
		private System.Windows.Forms.ListBox firstListBox;
		private System.Windows.Forms.Button firstAddButton;
		private System.Windows.Forms.Button firstRemoveButton;
		private System.Windows.Forms.GroupBox secondGroupBox;
		private System.Windows.Forms.Button secondAddButton;
		private System.Windows.Forms.ListBox secondListBox;
		private System.Windows.Forms.Button secondRemoveButton;
		private System.Windows.Forms.CheckBox devCmpletedCheckBox;
		private System.Windows.Forms.CheckBox subTreesCheckBox;
		private System.Windows.Forms.CheckBox ltOnlyCheckBox;
		private System.Windows.Forms.Button vacationsButton;
		private System.Windows.Forms.ComboBox usersVacationsComboBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
		private System.Windows.Forms.DataGridViewTextBoxColumn LeadTask;
		private System.Windows.Forms.DataGridViewTextBoxColumn Docs;
		private System.Windows.Forms.DataGridViewTextBoxColumn Task;
		private System.Windows.Forms.DataGridViewTextBoxColumn Blockers;
		private System.Windows.Forms.DataGridViewTextBoxColumn AssignedTo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Past;
		private System.Windows.Forms.CheckBox expandBlockersCheckBox;
		private System.Windows.Forms.ToolTip secondToolTip;
		private System.Windows.Forms.Button exchangeButton;
	}
}

