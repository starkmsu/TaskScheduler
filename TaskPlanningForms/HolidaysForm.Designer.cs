namespace TaskPlanningForms
{
	partial class HolidaysForm
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
			this.holidaysListBox = new System.Windows.Forms.ListBox();
			this.holidaysCalendar = new System.Windows.Forms.MonthCalendar();
			this.addHolidaysButton = new System.Windows.Forms.Button();
			this.deleteHolidaysButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// holidaysListBox
			// 
			this.holidaysListBox.FormattingEnabled = true;
			this.holidaysListBox.Location = new System.Drawing.Point(12, 12);
			this.holidaysListBox.Name = "holidaysListBox";
			this.holidaysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.holidaysListBox.Size = new System.Drawing.Size(93, 225);
			this.holidaysListBox.TabIndex = 10;
			// 
			// holidaysCalendar
			// 
			this.holidaysCalendar.Location = new System.Drawing.Point(117, 12);
			this.holidaysCalendar.Name = "holidaysCalendar";
			this.holidaysCalendar.TabIndex = 9;
			// 
			// addHolidaysButton
			// 
			this.addHolidaysButton.Location = new System.Drawing.Point(117, 186);
			this.addHolidaysButton.Name = "addHolidaysButton";
			this.addHolidaysButton.Size = new System.Drawing.Size(164, 23);
			this.addHolidaysButton.TabIndex = 11;
			this.addHolidaysButton.Text = "Add holidays";
			this.addHolidaysButton.UseVisualStyleBackColor = true;
			this.addHolidaysButton.Click += new System.EventHandler(this.AddHolidayButtonClick);
			// 
			// deleteHolidaysButton
			// 
			this.deleteHolidaysButton.Location = new System.Drawing.Point(12, 243);
			this.deleteHolidaysButton.Name = "deleteHolidaysButton";
			this.deleteHolidaysButton.Size = new System.Drawing.Size(93, 23);
			this.deleteHolidaysButton.TabIndex = 12;
			this.deleteHolidaysButton.Text = "Delete holidays";
			this.deleteHolidaysButton.UseVisualStyleBackColor = true;
			this.deleteHolidaysButton.Click += new System.EventHandler(this.DeleteHolidaysButtonClick);
			// 
			// HolidaysForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(290, 274);
			this.Controls.Add(this.deleteHolidaysButton);
			this.Controls.Add(this.addHolidaysButton);
			this.Controls.Add(this.holidaysListBox);
			this.Controls.Add(this.holidaysCalendar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HolidaysForm";
			this.Text = "Holidays";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox holidaysListBox;
		private System.Windows.Forms.MonthCalendar holidaysCalendar;
		private System.Windows.Forms.Button addHolidaysButton;
		private System.Windows.Forms.Button deleteHolidaysButton;
	}
}