namespace TaskSchedulerForms.Forms
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
			this.orLabel = new System.Windows.Forms.Label();
			this.makeScheduleButton = new System.Windows.Forms.Button();
			this.ParamsGroupBox = new System.Windows.Forms.GroupBox();
			this.sprintCheckBox = new System.Windows.Forms.CheckBox();
			this.loadLeadTasksButton = new System.Windows.Forms.Button();
			this.firstGroupBox = new System.Windows.Forms.GroupBox();
			this.firstComboBox = new System.Windows.Forms.ComboBox();
			this.firstRemoveButton = new System.Windows.Forms.Button();
			this.firstAddButton = new System.Windows.Forms.Button();
			this.firstListBox = new System.Windows.Forms.ListBox();
			this.secondGroupBox = new System.Windows.Forms.GroupBox();
			this.secondRemoveButton = new System.Windows.Forms.Button();
			this.secondListBox = new System.Windows.Forms.ListBox();
			this.secondAddButton = new System.Windows.Forms.Button();
			this.secondComboBox = new System.Windows.Forms.ComboBox();
			this.subTreesCheckBox = new System.Windows.Forms.CheckBox();
			this.exchangeButton = new System.Windows.Forms.Button();
			this.queryGroupBox = new System.Windows.Forms.GroupBox();
			this.queryLabel = new System.Windows.Forms.Label();
			this.queryTextBox = new System.Windows.Forms.TextBox();
			this.dataTabPage = new System.Windows.Forms.TabPage();
			this.sprintLabel = new System.Windows.Forms.Label();
			this.sprintFilterComboBox = new System.Windows.Forms.ComboBox();
			this.refreshButton = new System.Windows.Forms.Button();
			this.usersLabel = new System.Windows.Forms.Label();
			this.usersFilterСomboBox = new System.Windows.Forms.ComboBox();
			this.scheduleDataGridView = new System.Windows.Forms.DataGridView();
			this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Iteration = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Sprint = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Docs = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Blockers = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AssignedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Past = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toggleMenuStrip = new System.Windows.Forms.MenuStrip();
			this.columnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleIterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleSprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleDevCompletedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleLTOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleBlockersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.planningTabPage = new System.Windows.Forms.TabPage();
			this.addUserButton = new System.Windows.Forms.Button();
			this.addUserTextBox = new System.Windows.Forms.TextBox();
			this.planButton = new System.Windows.Forms.Button();
			this.usersLabel2 = new System.Windows.Forms.Label();
			this.usersFilterComboBox2 = new System.Windows.Forms.ComboBox();
			this.planningDataGridView = new System.Windows.Forms.DataGridView();
			this.Priority2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Iteration2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Sprint2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Id2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Docs2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Task2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Blockers2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AssignedTo2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Past2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toggleMenuStrip2 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleIterationToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleSprintToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleDevCompletedToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleLTOnlyToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleBlockersToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsTabPage = new System.Windows.Forms.TabPage();
			this.vacationsButton = new System.Windows.Forms.Button();
			this.usersVacationsComboBox = new System.Windows.Forms.ComboBox();
			this.holidaysButton = new System.Windows.Forms.Button();
			this.tfsUrlTextBox = new System.Windows.Forms.TextBox();
			this.tfsUrlLabel = new System.Windows.Forms.Label();
			this.secondToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.autoplanButton = new System.Windows.Forms.Button();
			this.mainTabControl.SuspendLayout();
			this.mainTabPage.SuspendLayout();
			this.ParamsGroupBox.SuspendLayout();
			this.firstGroupBox.SuspendLayout();
			this.secondGroupBox.SuspendLayout();
			this.queryGroupBox.SuspendLayout();
			this.dataTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).BeginInit();
			this.toggleMenuStrip.SuspendLayout();
			this.planningTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.planningDataGridView)).BeginInit();
			this.toggleMenuStrip2.SuspendLayout();
			this.settingsTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.mainTabPage);
			this.mainTabControl.Controls.Add(this.dataTabPage);
			this.mainTabControl.Controls.Add(this.planningTabPage);
			this.mainTabControl.Controls.Add(this.settingsTabPage);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 0);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(887, 574);
			this.mainTabControl.TabIndex = 0;
			// 
			// mainTabPage
			// 
			this.mainTabPage.Controls.Add(this.orLabel);
			this.mainTabPage.Controls.Add(this.makeScheduleButton);
			this.mainTabPage.Controls.Add(this.ParamsGroupBox);
			this.mainTabPage.Controls.Add(this.queryGroupBox);
			this.mainTabPage.Location = new System.Drawing.Point(4, 22);
			this.mainTabPage.Name = "mainTabPage";
			this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.mainTabPage.Size = new System.Drawing.Size(879, 548);
			this.mainTabPage.TabIndex = 1;
			this.mainTabPage.Text = "Main";
			this.mainTabPage.UseVisualStyleBackColor = true;
			// 
			// orLabel
			// 
			this.orLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.orLabel.AutoSize = true;
			this.orLabel.BackColor = System.Drawing.Color.Transparent;
			this.orLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.orLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.orLabel.Location = new System.Drawing.Point(416, 414);
			this.orLabel.Name = "orLabel";
			this.orLabel.Size = new System.Drawing.Size(35, 22);
			this.orLabel.TabIndex = 19;
			this.orLabel.Text = "OR";
			// 
			// makeScheduleButton
			// 
			this.makeScheduleButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.makeScheduleButton.Enabled = false;
			this.makeScheduleButton.Location = new System.Drawing.Point(385, 474);
			this.makeScheduleButton.Name = "makeScheduleButton";
			this.makeScheduleButton.Size = new System.Drawing.Size(107, 23);
			this.makeScheduleButton.TabIndex = 4;
			this.makeScheduleButton.Text = "Make Schedule";
			this.makeScheduleButton.UseVisualStyleBackColor = true;
			this.makeScheduleButton.Click += new System.EventHandler(this.MakeScheduleButtonClick);
			// 
			// ParamsGroupBox
			// 
			this.ParamsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ParamsGroupBox.Controls.Add(this.sprintCheckBox);
			this.ParamsGroupBox.Controls.Add(this.loadLeadTasksButton);
			this.ParamsGroupBox.Controls.Add(this.firstGroupBox);
			this.ParamsGroupBox.Controls.Add(this.secondGroupBox);
			this.ParamsGroupBox.Controls.Add(this.subTreesCheckBox);
			this.ParamsGroupBox.Controls.Add(this.exchangeButton);
			this.ParamsGroupBox.Location = new System.Drawing.Point(3, 1);
			this.ParamsGroupBox.Name = "ParamsGroupBox";
			this.ParamsGroupBox.Size = new System.Drawing.Size(873, 410);
			this.ParamsGroupBox.TabIndex = 18;
			this.ParamsGroupBox.TabStop = false;
			// 
			// sprintCheckBox
			// 
			this.sprintCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.sprintCheckBox.AutoSize = true;
			this.sprintCheckBox.Checked = true;
			this.sprintCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.sprintCheckBox.Location = new System.Drawing.Point(593, 181);
			this.sprintCheckBox.Name = "sprintCheckBox";
			this.sprintCheckBox.Size = new System.Drawing.Size(73, 17);
			this.sprintCheckBox.TabIndex = 18;
			this.sprintCheckBox.Text = "with sprint";
			this.sprintCheckBox.UseVisualStyleBackColor = true;
			// 
			// loadLeadTasksButton
			// 
			this.loadLeadTasksButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.loadLeadTasksButton.Location = new System.Drawing.Point(382, 177);
			this.loadLeadTasksButton.Name = "loadLeadTasksButton";
			this.loadLeadTasksButton.Size = new System.Drawing.Size(107, 23);
			this.loadLeadTasksButton.TabIndex = 5;
			this.loadLeadTasksButton.Text = "Load LeadTasks";
			this.loadLeadTasksButton.UseVisualStyleBackColor = true;
			this.loadLeadTasksButton.Click += new System.EventHandler(this.LoadLeadTasksButtonClick);
			// 
			// firstGroupBox
			// 
			this.firstGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.firstGroupBox.Controls.Add(this.firstComboBox);
			this.firstGroupBox.Controls.Add(this.firstRemoveButton);
			this.firstGroupBox.Controls.Add(this.firstAddButton);
			this.firstGroupBox.Controls.Add(this.firstListBox);
			this.firstGroupBox.Location = new System.Drawing.Point(6, 10);
			this.firstGroupBox.Name = "firstGroupBox";
			this.firstGroupBox.Size = new System.Drawing.Size(860, 162);
			this.firstGroupBox.TabIndex = 14;
			this.firstGroupBox.TabStop = false;
			this.firstGroupBox.Text = "Area";
			// 
			// firstComboBox
			// 
			this.firstComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.firstComboBox.FormattingEnabled = true;
			this.firstComboBox.Location = new System.Drawing.Point(6, 18);
			this.firstComboBox.Name = "firstComboBox";
			this.firstComboBox.Size = new System.Drawing.Size(699, 21);
			this.firstComboBox.TabIndex = 9;
			// 
			// firstRemoveButton
			// 
			this.firstRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.firstRemoveButton.Location = new System.Drawing.Point(779, 16);
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
			this.firstAddButton.Location = new System.Drawing.Point(711, 16);
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
			this.firstListBox.Location = new System.Drawing.Point(6, 44);
			this.firstListBox.Name = "firstListBox";
			this.firstListBox.Size = new System.Drawing.Size(848, 108);
			this.firstListBox.TabIndex = 6;
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
			this.secondGroupBox.Location = new System.Drawing.Point(6, 206);
			this.secondGroupBox.Name = "secondGroupBox";
			this.secondGroupBox.Size = new System.Drawing.Size(860, 201);
			this.secondGroupBox.TabIndex = 15;
			this.secondGroupBox.TabStop = false;
			this.secondGroupBox.Text = "Iteration";
			// 
			// secondRemoveButton
			// 
			this.secondRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.secondRemoveButton.Enabled = false;
			this.secondRemoveButton.Location = new System.Drawing.Point(779, 17);
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
			this.secondListBox.Location = new System.Drawing.Point(6, 48);
			this.secondListBox.Name = "secondListBox";
			this.secondListBox.Size = new System.Drawing.Size(848, 134);
			this.secondListBox.TabIndex = 9;
			// 
			// secondAddButton
			// 
			this.secondAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.secondAddButton.Enabled = false;
			this.secondAddButton.Location = new System.Drawing.Point(711, 17);
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
			this.secondComboBox.Size = new System.Drawing.Size(699, 21);
			this.secondComboBox.TabIndex = 6;
			// 
			// subTreesCheckBox
			// 
			this.subTreesCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.subTreesCheckBox.AutoSize = true;
			this.subTreesCheckBox.Checked = true;
			this.subTreesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.subTreesCheckBox.Location = new System.Drawing.Point(495, 181);
			this.subTreesCheckBox.Name = "subTreesCheckBox";
			this.subTreesCheckBox.Size = new System.Drawing.Size(91, 17);
			this.subTreesCheckBox.TabIndex = 16;
			this.subTreesCheckBox.Text = "with sub trees";
			this.subTreesCheckBox.UseVisualStyleBackColor = true;
			// 
			// exchangeButton
			// 
			this.exchangeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.exchangeButton.Image = global::TaskSchedulerForms.Properties.Resources.Exchange;
			this.exchangeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.exchangeButton.Location = new System.Drawing.Point(785, 177);
			this.exchangeButton.Name = "exchangeButton";
			this.exchangeButton.Size = new System.Drawing.Size(75, 23);
			this.exchangeButton.TabIndex = 17;
			this.exchangeButton.Text = "Exchange";
			this.exchangeButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.exchangeButton.UseVisualStyleBackColor = true;
			this.exchangeButton.Click += new System.EventHandler(this.ExchangeButtonClick);
			// 
			// queryGroupBox
			// 
			this.queryGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.queryGroupBox.Controls.Add(this.queryLabel);
			this.queryGroupBox.Controls.Add(this.queryTextBox);
			this.queryGroupBox.Location = new System.Drawing.Point(3, 437);
			this.queryGroupBox.Name = "queryGroupBox";
			this.queryGroupBox.Size = new System.Drawing.Size(873, 36);
			this.queryGroupBox.TabIndex = 22;
			this.queryGroupBox.TabStop = false;
			// 
			// queryLabel
			// 
			this.queryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.queryLabel.AutoSize = true;
			this.queryLabel.Location = new System.Drawing.Point(12, 14);
			this.queryLabel.Name = "queryLabel";
			this.queryLabel.Size = new System.Drawing.Size(81, 13);
			this.queryLabel.TabIndex = 21;
			this.queryLabel.Text = "Tfs Query Path:";
			// 
			// queryTextBox
			// 
			this.queryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.queryTextBox.ForeColor = System.Drawing.Color.Gray;
			this.queryTextBox.Location = new System.Drawing.Point(96, 11);
			this.queryTextBox.Name = "queryTextBox";
			this.queryTextBox.Size = new System.Drawing.Size(764, 20);
			this.queryTextBox.TabIndex = 20;
			this.queryTextBox.Text = "example: FORIS_Mobile/My Queries/My query";
			this.queryTextBox.Enter += new System.EventHandler(this.QueryTextBoxEnter);
			this.queryTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.QueryTextBoxKeyUp);
			this.queryTextBox.Leave += new System.EventHandler(this.QueryTextBoxLeave);
			// 
			// dataTabPage
			// 
			this.dataTabPage.Controls.Add(this.sprintLabel);
			this.dataTabPage.Controls.Add(this.sprintFilterComboBox);
			this.dataTabPage.Controls.Add(this.refreshButton);
			this.dataTabPage.Controls.Add(this.usersLabel);
			this.dataTabPage.Controls.Add(this.usersFilterСomboBox);
			this.dataTabPage.Controls.Add(this.scheduleDataGridView);
			this.dataTabPage.Controls.Add(this.toggleMenuStrip);
			this.dataTabPage.Location = new System.Drawing.Point(4, 22);
			this.dataTabPage.Name = "dataTabPage";
			this.dataTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.dataTabPage.Size = new System.Drawing.Size(879, 548);
			this.dataTabPage.TabIndex = 0;
			this.dataTabPage.Text = "Schedule";
			this.dataTabPage.UseVisualStyleBackColor = true;
			// 
			// sprintLabel
			// 
			this.sprintLabel.AutoSize = true;
			this.sprintLabel.Enabled = false;
			this.sprintLabel.Location = new System.Drawing.Point(378, 33);
			this.sprintLabel.Name = "sprintLabel";
			this.sprintLabel.Size = new System.Drawing.Size(71, 13);
			this.sprintLabel.TabIndex = 19;
			this.sprintLabel.Text = "Filter by sprint";
			// 
			// sprintFilterComboBox
			// 
			this.sprintFilterComboBox.Enabled = false;
			this.sprintFilterComboBox.FormattingEnabled = true;
			this.sprintFilterComboBox.Location = new System.Drawing.Point(450, 30);
			this.sprintFilterComboBox.Name = "sprintFilterComboBox";
			this.sprintFilterComboBox.Size = new System.Drawing.Size(209, 21);
			this.sprintFilterComboBox.TabIndex = 18;
			this.sprintFilterComboBox.SelectionChangeCommitted += new System.EventHandler(this.SprintFilterComboBoxSelectionChangeCommitted);
			// 
			// refreshButton
			// 
			this.refreshButton.Enabled = false;
			this.refreshButton.Location = new System.Drawing.Point(8, 28);
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
			this.usersLabel.Location = new System.Drawing.Point(89, 33);
			this.usersLabel.Name = "usersLabel";
			this.usersLabel.Size = new System.Drawing.Size(66, 13);
			this.usersLabel.TabIndex = 5;
			this.usersLabel.Text = "Filter by user";
			// 
			// usersFilterСomboBox
			// 
			this.usersFilterСomboBox.Enabled = false;
			this.usersFilterСomboBox.FormattingEnabled = true;
			this.usersFilterСomboBox.Location = new System.Drawing.Point(161, 30);
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
            this.Iteration,
            this.Sprint,
            this.Id,
            this.Docs,
            this.Task,
            this.Blockers,
            this.AssignedTo,
            this.Past});
			this.scheduleDataGridView.EnableHeadersVisualStyles = false;
			this.scheduleDataGridView.Location = new System.Drawing.Point(0, 56);
			this.scheduleDataGridView.Name = "scheduleDataGridView";
			this.scheduleDataGridView.ReadOnly = true;
			this.scheduleDataGridView.RowHeadersVisible = false;
			this.scheduleDataGridView.Size = new System.Drawing.Size(880, 496);
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
			// Iteration
			// 
			this.Iteration.Frozen = true;
			this.Iteration.HeaderText = "Iteration";
			this.Iteration.Name = "Iteration";
			this.Iteration.ReadOnly = true;
			this.Iteration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Iteration.Visible = false;
			this.Iteration.Width = 180;
			// 
			// Sprint
			// 
			this.Sprint.Frozen = true;
			this.Sprint.HeaderText = "Sprint";
			this.Sprint.Name = "Sprint";
			this.Sprint.ReadOnly = true;
			this.Sprint.Visible = false;
			this.Sprint.Width = 60;
			// 
			// Id
			// 
			this.Id.Frozen = true;
			this.Id.HeaderText = "Id";
			this.Id.Name = "Id";
			this.Id.ReadOnly = true;
			this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Id.Width = 60;
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
			// toggleMenuStrip
			// 
			this.toggleMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.columnsToolStripMenuItem,
            this.rowsToolStripMenuItem});
			this.toggleMenuStrip.Location = new System.Drawing.Point(3, 3);
			this.toggleMenuStrip.Name = "toggleMenuStrip";
			this.toggleMenuStrip.Size = new System.Drawing.Size(873, 24);
			this.toggleMenuStrip.TabIndex = 17;
			this.toggleMenuStrip.Text = "menuStrip1";
			// 
			// columnsToolStripMenuItem
			// 
			this.columnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleIterationToolStripMenuItem,
            this.toggleSprintToolStripMenuItem});
			this.columnsToolStripMenuItem.Name = "columnsToolStripMenuItem";
			this.columnsToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
			this.columnsToolStripMenuItem.Text = "Columns";
			// 
			// toggleIterationToolStripMenuItem
			// 
			this.toggleIterationToolStripMenuItem.Name = "toggleIterationToolStripMenuItem";
			this.toggleIterationToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.toggleIterationToolStripMenuItem.Text = "Toggle Iteration";
			this.toggleIterationToolStripMenuItem.Click += new System.EventHandler(this.ToggleIterationToolStripMenuItemClick);
			// 
			// toggleSprintToolStripMenuItem
			// 
			this.toggleSprintToolStripMenuItem.Name = "toggleSprintToolStripMenuItem";
			this.toggleSprintToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.toggleSprintToolStripMenuItem.Text = "Toggle Sprint";
			this.toggleSprintToolStripMenuItem.Click += new System.EventHandler(this.ToggleSprintToolStripMenuItemClick);
			// 
			// rowsToolStripMenuItem
			// 
			this.rowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleDevCompletedToolStripMenuItem,
            this.toggleLTOnlyToolStripMenuItem,
            this.toggleBlockersToolStripMenuItem});
			this.rowsToolStripMenuItem.Name = "rowsToolStripMenuItem";
			this.rowsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.rowsToolStripMenuItem.Text = "Rows";
			// 
			// toggleDevCompletedToolStripMenuItem
			// 
			this.toggleDevCompletedToolStripMenuItem.Name = "toggleDevCompletedToolStripMenuItem";
			this.toggleDevCompletedToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.toggleDevCompletedToolStripMenuItem.Text = "Toggle Dev Completed";
			this.toggleDevCompletedToolStripMenuItem.Click += new System.EventHandler(this.ToggleDevCompletedToolStripMenuItem1Click);
			// 
			// toggleLTOnlyToolStripMenuItem
			// 
			this.toggleLTOnlyToolStripMenuItem.Name = "toggleLTOnlyToolStripMenuItem";
			this.toggleLTOnlyToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.toggleLTOnlyToolStripMenuItem.Text = "Toggle LT Only";
			this.toggleLTOnlyToolStripMenuItem.Click += new System.EventHandler(this.ToggleLtOnlyToolStripMenuItemClick);
			// 
			// toggleBlockersToolStripMenuItem
			// 
			this.toggleBlockersToolStripMenuItem.Name = "toggleBlockersToolStripMenuItem";
			this.toggleBlockersToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.toggleBlockersToolStripMenuItem.Text = "Toggle Blockers";
			this.toggleBlockersToolStripMenuItem.Click += new System.EventHandler(this.ToggleBlockersToolStripMenuItemClick);
			// 
			// planningTabPage
			// 
			this.planningTabPage.Controls.Add(this.autoplanButton);
			this.planningTabPage.Controls.Add(this.addUserButton);
			this.planningTabPage.Controls.Add(this.addUserTextBox);
			this.planningTabPage.Controls.Add(this.planButton);
			this.planningTabPage.Controls.Add(this.usersLabel2);
			this.planningTabPage.Controls.Add(this.usersFilterComboBox2);
			this.planningTabPage.Controls.Add(this.planningDataGridView);
			this.planningTabPage.Controls.Add(this.toggleMenuStrip2);
			this.planningTabPage.Location = new System.Drawing.Point(4, 22);
			this.planningTabPage.Name = "planningTabPage";
			this.planningTabPage.Size = new System.Drawing.Size(879, 548);
			this.planningTabPage.TabIndex = 3;
			this.planningTabPage.Text = "Planning";
			this.planningTabPage.UseVisualStyleBackColor = true;
			// 
			// addUserButton
			// 
			this.addUserButton.Enabled = false;
			this.addUserButton.Location = new System.Drawing.Point(568, 27);
			this.addUserButton.Name = "addUserButton";
			this.addUserButton.Size = new System.Drawing.Size(75, 23);
			this.addUserButton.TabIndex = 28;
			this.addUserButton.Text = "Add user";
			this.addUserButton.UseVisualStyleBackColor = true;
			this.addUserButton.Click += new System.EventHandler(this.AddUserButtonClick);
			// 
			// addUserTextBox
			// 
			this.addUserTextBox.Location = new System.Drawing.Point(376, 29);
			this.addUserTextBox.Name = "addUserTextBox";
			this.addUserTextBox.Size = new System.Drawing.Size(186, 20);
			this.addUserTextBox.TabIndex = 27;
			this.addUserTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AddUserTextBoxKeyUp);
			// 
			// planButton
			// 
			this.planButton.Enabled = false;
			this.planButton.Location = new System.Drawing.Point(8, 27);
			this.planButton.Name = "planButton";
			this.planButton.Size = new System.Drawing.Size(75, 23);
			this.planButton.TabIndex = 22;
			this.planButton.Text = "Plan";
			this.planButton.UseVisualStyleBackColor = true;
			this.planButton.Click += new System.EventHandler(this.PlanButtonClick);
			// 
			// usersLabel2
			// 
			this.usersLabel2.AutoSize = true;
			this.usersLabel2.Enabled = false;
			this.usersLabel2.Location = new System.Drawing.Point(89, 32);
			this.usersLabel2.Name = "usersLabel2";
			this.usersLabel2.Size = new System.Drawing.Size(66, 13);
			this.usersLabel2.TabIndex = 23;
			this.usersLabel2.Text = "Filter by user";
			// 
			// usersFilterComboBox2
			// 
			this.usersFilterComboBox2.Enabled = false;
			this.usersFilterComboBox2.FormattingEnabled = true;
			this.usersFilterComboBox2.Location = new System.Drawing.Point(161, 29);
			this.usersFilterComboBox2.Name = "usersFilterComboBox2";
			this.usersFilterComboBox2.Size = new System.Drawing.Size(209, 21);
			this.usersFilterComboBox2.TabIndex = 24;
			this.usersFilterComboBox2.SelectionChangeCommitted += new System.EventHandler(this.UsersFilterСomboBoxSelectionChangeCommitted);
			// 
			// planningDataGridView
			// 
			this.planningDataGridView.AllowUserToAddRows = false;
			this.planningDataGridView.AllowUserToDeleteRows = false;
			this.planningDataGridView.AllowUserToResizeRows = false;
			this.planningDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.planningDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.planningDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Priority2,
            this.Iteration2,
            this.Sprint2,
            this.Id2,
            this.Docs2,
            this.Task2,
            this.Blockers2,
            this.AssignedTo2,
            this.Past2});
			this.planningDataGridView.EnableHeadersVisualStyles = false;
			this.planningDataGridView.Location = new System.Drawing.Point(0, 56);
			this.planningDataGridView.Name = "planningDataGridView";
			this.planningDataGridView.RowHeadersVisible = false;
			this.planningDataGridView.Size = new System.Drawing.Size(880, 496);
			this.planningDataGridView.TabIndex = 25;
			// 
			// Priority2
			// 
			this.Priority2.Frozen = true;
			this.Priority2.HeaderText = "Pr";
			this.Priority2.Name = "Priority2";
			this.Priority2.ReadOnly = true;
			this.Priority2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Priority2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Priority2.Width = 30;
			// 
			// Iteration2
			// 
			this.Iteration2.Frozen = true;
			this.Iteration2.HeaderText = "Iteration";
			this.Iteration2.Name = "Iteration2";
			this.Iteration2.ReadOnly = true;
			this.Iteration2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Iteration2.Visible = false;
			this.Iteration2.Width = 180;
			// 
			// Sprint2
			// 
			this.Sprint2.Frozen = true;
			this.Sprint2.HeaderText = "Sprint";
			this.Sprint2.Name = "Sprint2";
			this.Sprint2.ReadOnly = true;
			this.Sprint2.Visible = false;
			this.Sprint2.Width = 60;
			// 
			// Id2
			// 
			this.Id2.Frozen = true;
			this.Id2.HeaderText = "Id";
			this.Id2.Name = "Id2";
			this.Id2.ReadOnly = true;
			this.Id2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Id2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Id2.Width = 60;
			// 
			// Docs2
			// 
			this.Docs2.Frozen = true;
			this.Docs2.HeaderText = "Docs";
			this.Docs2.Name = "Docs2";
			this.Docs2.ReadOnly = true;
			this.Docs2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Docs2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Docs2.Width = 50;
			// 
			// Task2
			// 
			this.Task2.Frozen = true;
			this.Task2.HeaderText = "Task";
			this.Task2.Name = "Task2";
			this.Task2.ReadOnly = true;
			this.Task2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Task2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Task2.Width = 200;
			// 
			// Blockers2
			// 
			this.Blockers2.Frozen = true;
			this.Blockers2.HeaderText = "Blockers";
			this.Blockers2.Name = "Blockers2";
			this.Blockers2.ReadOnly = true;
			this.Blockers2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Blockers2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Blockers2.Width = 60;
			// 
			// AssignedTo2
			// 
			this.AssignedTo2.DropDownWidth = 200;
			this.AssignedTo2.Frozen = true;
			this.AssignedTo2.HeaderText = "AssignedTo";
			this.AssignedTo2.MaxDropDownItems = 16;
			this.AssignedTo2.Name = "AssignedTo2";
			this.AssignedTo2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.AssignedTo2.Width = 120;
			// 
			// Past2
			// 
			this.Past2.HeaderText = "Past";
			this.Past2.Name = "Past2";
			this.Past2.ReadOnly = true;
			this.Past2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Past2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Past2.Width = 40;
			// 
			// toggleMenuStrip2
			// 
			this.toggleMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem4});
			this.toggleMenuStrip2.Location = new System.Drawing.Point(0, 0);
			this.toggleMenuStrip2.Name = "toggleMenuStrip2";
			this.toggleMenuStrip2.Size = new System.Drawing.Size(879, 24);
			this.toggleMenuStrip2.TabIndex = 26;
			this.toggleMenuStrip2.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleIterationToolStripMenuItem2,
            this.toggleSprintToolStripMenuItem2});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(67, 20);
			this.toolStripMenuItem1.Text = "Columns";
			// 
			// toggleIterationToolStripMenuItem2
			// 
			this.toggleIterationToolStripMenuItem2.Name = "toggleIterationToolStripMenuItem2";
			this.toggleIterationToolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
			this.toggleIterationToolStripMenuItem2.Text = "Toggle Iteration";
			this.toggleIterationToolStripMenuItem2.Click += new System.EventHandler(this.ToggleIterationToolStripMenuItemClick);
			// 
			// toggleSprintToolStripMenuItem2
			// 
			this.toggleSprintToolStripMenuItem2.Name = "toggleSprintToolStripMenuItem2";
			this.toggleSprintToolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
			this.toggleSprintToolStripMenuItem2.Text = "Toggle Sprint";
			this.toggleSprintToolStripMenuItem2.Click += new System.EventHandler(this.ToggleSprintToolStripMenuItemClick);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleDevCompletedToolStripMenuItem2,
            this.toggleLTOnlyToolStripMenuItem2,
            this.toggleBlockersToolStripMenuItem2});
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(47, 20);
			this.toolStripMenuItem4.Text = "Rows";
			// 
			// toggleDevCompletedToolStripMenuItem2
			// 
			this.toggleDevCompletedToolStripMenuItem2.Name = "toggleDevCompletedToolStripMenuItem2";
			this.toggleDevCompletedToolStripMenuItem2.Size = new System.Drawing.Size(196, 22);
			this.toggleDevCompletedToolStripMenuItem2.Text = "Toggle Dev Completed";
			this.toggleDevCompletedToolStripMenuItem2.Click += new System.EventHandler(this.ToggleDevCompletedToolStripMenuItem1Click);
			// 
			// toggleLTOnlyToolStripMenuItem2
			// 
			this.toggleLTOnlyToolStripMenuItem2.Name = "toggleLTOnlyToolStripMenuItem2";
			this.toggleLTOnlyToolStripMenuItem2.Size = new System.Drawing.Size(196, 22);
			this.toggleLTOnlyToolStripMenuItem2.Text = "Toggle LT Only";
			this.toggleLTOnlyToolStripMenuItem2.Click += new System.EventHandler(this.ToggleLtOnlyToolStripMenuItemClick);
			// 
			// toggleBlockersToolStripMenuItem2
			// 
			this.toggleBlockersToolStripMenuItem2.Name = "toggleBlockersToolStripMenuItem2";
			this.toggleBlockersToolStripMenuItem2.Size = new System.Drawing.Size(196, 22);
			this.toggleBlockersToolStripMenuItem2.Text = "Toggle Blockers";
			this.toggleBlockersToolStripMenuItem2.Click += new System.EventHandler(this.ToggleBlockersToolStripMenuItemClick);
			// 
			// settingsTabPage
			// 
			this.settingsTabPage.Controls.Add(this.vacationsButton);
			this.settingsTabPage.Controls.Add(this.usersVacationsComboBox);
			this.settingsTabPage.Controls.Add(this.holidaysButton);
			this.settingsTabPage.Controls.Add(this.tfsUrlTextBox);
			this.settingsTabPage.Controls.Add(this.tfsUrlLabel);
			this.settingsTabPage.Location = new System.Drawing.Point(4, 22);
			this.settingsTabPage.Name = "settingsTabPage";
			this.settingsTabPage.Size = new System.Drawing.Size(879, 548);
			this.settingsTabPage.TabIndex = 2;
			this.settingsTabPage.Text = "Settings";
			this.settingsTabPage.UseVisualStyleBackColor = true;
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
			// autoplanButton
			// 
			this.autoplanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.autoplanButton.Enabled = false;
			this.autoplanButton.Location = new System.Drawing.Point(796, 26);
			this.autoplanButton.Name = "autoplanButton";
			this.autoplanButton.Size = new System.Drawing.Size(75, 23);
			this.autoplanButton.TabIndex = 29;
			this.autoplanButton.Text = "AutoPlan";
			this.autoplanButton.UseVisualStyleBackColor = true;
			this.autoplanButton.Click += new System.EventHandler(this.AutoplanButtonClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(887, 574);
			this.Controls.Add(this.mainTabControl);
			this.MainMenuStrip = this.toggleMenuStrip;
			this.Name = "MainForm";
			this.Text = "Task Scheduler";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.mainTabControl.ResumeLayout(false);
			this.mainTabPage.ResumeLayout(false);
			this.mainTabPage.PerformLayout();
			this.ParamsGroupBox.ResumeLayout(false);
			this.ParamsGroupBox.PerformLayout();
			this.firstGroupBox.ResumeLayout(false);
			this.secondGroupBox.ResumeLayout(false);
			this.queryGroupBox.ResumeLayout(false);
			this.queryGroupBox.PerformLayout();
			this.dataTabPage.ResumeLayout(false);
			this.dataTabPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).EndInit();
			this.toggleMenuStrip.ResumeLayout(false);
			this.toggleMenuStrip.PerformLayout();
			this.planningTabPage.ResumeLayout(false);
			this.planningTabPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.planningDataGridView)).EndInit();
			this.toggleMenuStrip2.ResumeLayout(false);
			this.toggleMenuStrip2.PerformLayout();
			this.settingsTabPage.ResumeLayout(false);
			this.settingsTabPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage dataTabPage;
		private System.Windows.Forms.DataGridView scheduleDataGridView;
		private System.Windows.Forms.TabPage mainTabPage;
		private System.Windows.Forms.Button makeScheduleButton;
		private System.Windows.Forms.Label usersLabel;
		private System.Windows.Forms.ComboBox usersFilterСomboBox;
		private System.Windows.Forms.Button loadLeadTasksButton;
		private System.Windows.Forms.ComboBox secondComboBox;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.TabPage settingsTabPage;
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
		private System.Windows.Forms.CheckBox subTreesCheckBox;
		private System.Windows.Forms.Button vacationsButton;
		private System.Windows.Forms.ComboBox usersVacationsComboBox;
		private System.Windows.Forms.ToolTip secondToolTip;
		private System.Windows.Forms.Button exchangeButton;
		private System.Windows.Forms.GroupBox ParamsGroupBox;
		private System.Windows.Forms.Label orLabel;
		private System.Windows.Forms.Label queryLabel;
		private System.Windows.Forms.TextBox queryTextBox;
		private System.Windows.Forms.GroupBox queryGroupBox;
		private System.Windows.Forms.ComboBox firstComboBox;
		private System.Windows.Forms.CheckBox sprintCheckBox;
		private System.Windows.Forms.MenuStrip toggleMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem columnsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleIterationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleSprintToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleDevCompletedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleLTOnlyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleBlockersToolStripMenuItem;
		private System.Windows.Forms.Label sprintLabel;
		private System.Windows.Forms.ComboBox sprintFilterComboBox;
		private System.Windows.Forms.TabPage planningTabPage;
		private System.Windows.Forms.Button planButton;
		private System.Windows.Forms.Label usersLabel2;
		private System.Windows.Forms.ComboBox usersFilterComboBox2;
		private System.Windows.Forms.MenuStrip toggleMenuStrip2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toggleIterationToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toggleSprintToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem toggleDevCompletedToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toggleLTOnlyToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toggleBlockersToolStripMenuItem2;
		private System.Windows.Forms.DataGridView planningDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
		private System.Windows.Forms.DataGridViewTextBoxColumn Iteration;
		private System.Windows.Forms.DataGridViewTextBoxColumn Sprint;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id;
		private System.Windows.Forms.DataGridViewTextBoxColumn Docs;
		private System.Windows.Forms.DataGridViewTextBoxColumn Task;
		private System.Windows.Forms.DataGridViewTextBoxColumn Blockers;
		private System.Windows.Forms.DataGridViewTextBoxColumn AssignedTo;
		private System.Windows.Forms.DataGridViewTextBoxColumn Past;
		private System.Windows.Forms.DataGridViewTextBoxColumn Priority2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Iteration2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Sprint2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Id2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Docs2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Task2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Blockers2;
		private System.Windows.Forms.DataGridViewComboBoxColumn AssignedTo2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Past2;
		private System.Windows.Forms.TextBox addUserTextBox;
		private System.Windows.Forms.Button addUserButton;
		private System.Windows.Forms.Button autoplanButton;
	}
}

