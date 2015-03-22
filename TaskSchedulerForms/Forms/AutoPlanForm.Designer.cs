namespace TaskSchedulerForms.Forms
{
	partial class AutoPlanForm
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
			this.autoPlanTabControl = new System.Windows.Forms.TabControl();
			this.autoPlanAddUserButton = new System.Windows.Forms.Button();
			this.autoPlanDeleteUserButton = new System.Windows.Forms.Button();
			this.autoPlanAddUserTextBox = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// autoPlanTabControl
			// 
			this.autoPlanTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.autoPlanTabControl.Location = new System.Drawing.Point(1, 2);
			this.autoPlanTabControl.Name = "autoPlanTabControl";
			this.autoPlanTabControl.SelectedIndex = 0;
			this.autoPlanTabControl.Size = new System.Drawing.Size(494, 313);
			this.autoPlanTabControl.TabIndex = 0;
			this.autoPlanTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.AutoPlanTabControlSelected);
			// 
			// autoPlanAddUserButton
			// 
			this.autoPlanAddUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.autoPlanAddUserButton.Location = new System.Drawing.Point(325, 321);
			this.autoPlanAddUserButton.Name = "autoPlanAddUserButton";
			this.autoPlanAddUserButton.Size = new System.Drawing.Size(75, 23);
			this.autoPlanAddUserButton.TabIndex = 1;
			this.autoPlanAddUserButton.Text = "Add user";
			this.autoPlanAddUserButton.UseVisualStyleBackColor = true;
			this.autoPlanAddUserButton.Click += new System.EventHandler(this.AutoPlanAddUserButtonClick);
			// 
			// autoPlanDeleteUserButton
			// 
			this.autoPlanDeleteUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.autoPlanDeleteUserButton.Enabled = false;
			this.autoPlanDeleteUserButton.Location = new System.Drawing.Point(406, 321);
			this.autoPlanDeleteUserButton.Name = "autoPlanDeleteUserButton";
			this.autoPlanDeleteUserButton.Size = new System.Drawing.Size(75, 23);
			this.autoPlanDeleteUserButton.TabIndex = 2;
			this.autoPlanDeleteUserButton.Text = "Delete User";
			this.autoPlanDeleteUserButton.UseVisualStyleBackColor = true;
			this.autoPlanDeleteUserButton.Click += new System.EventHandler(this.AutoPlanDeleteUserButtonClick);
			// 
			// autoPlanAddUserTextBox
			// 
			this.autoPlanAddUserTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.autoPlanAddUserTextBox.Location = new System.Drawing.Point(1, 323);
			this.autoPlanAddUserTextBox.Name = "autoPlanAddUserTextBox";
			this.autoPlanAddUserTextBox.Size = new System.Drawing.Size(318, 20);
			this.autoPlanAddUserTextBox.TabIndex = 3;
			this.autoPlanAddUserTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AutoPlanAddUserTextBoxKeyUp);
			// 
			// button1
			// 
			this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(218, 351);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Auto Plan";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// AutoPlanForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(493, 386);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.autoPlanAddUserTextBox);
			this.Controls.Add(this.autoPlanDeleteUserButton);
			this.Controls.Add(this.autoPlanAddUserButton);
			this.Controls.Add(this.autoPlanTabControl);
			this.Name = "AutoPlanForm";
			this.Text = "AutoPlanForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl autoPlanTabControl;
		private System.Windows.Forms.Button autoPlanAddUserButton;
		private System.Windows.Forms.Button autoPlanDeleteUserButton;
		private System.Windows.Forms.TextBox autoPlanAddUserTextBox;
		private System.Windows.Forms.Button button1;
	}
}