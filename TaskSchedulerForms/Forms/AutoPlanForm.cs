using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskSchedulerForms.Forms
{
	public partial class AutoPlanForm : Form
	{
		internal Dictionary<string, List<string>> UsersByDiscipline
		{
			get
			{
				var result = new Dictionary<string, List<string>>(autoPlanTabControl.TabPages.Count);
				for (int i = 0; i < autoPlanTabControl.TabPages.Count; i++)
				{
					var tabPage = autoPlanTabControl.TabPages[i];
					var usersBox = tabPage.Controls[0] as ListBox;
					result.Add(tabPage.Text, usersBox.Items.Cast<string>().ToList());
				}
				return result;
			}
		}

		public AutoPlanForm(Dictionary<string, HashSet<string>> disciplineUsers)
		{
			InitializeComponent();

			if (disciplineUsers == null)
				return;

			foreach (string discipline in disciplineUsers.Keys.OrderBy(i => i))
			{
				var tabPage = new TabPage(discipline);
				var users = new ListBox {Dock = DockStyle.Fill};
				users.Items.AddRange(disciplineUsers[discipline].Cast<object>().ToArray());
				users.SelectedIndexChanged += UsersOnSelectedIndexChanged;

				tabPage.Controls.Add(users);
				autoPlanTabControl.TabPages.Add(tabPage);
			}
		}

		private void UsersOnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			autoPlanDeleteUserButton.Enabled = true;
		}

		private void AutoPlanAddUserButtonClick(object sender, EventArgs e)
		{
			string userToAdd = autoPlanAddUserTextBox.Text;
			var usersBox = autoPlanTabControl.SelectedTab.Controls[0] as ListBox;
			if (usersBox.Items.Cast<string>().Any(i => i == userToAdd))
				return;
			usersBox.Items.Add(userToAdd);
			if (usersBox.Items.Count == 1)
				autoPlanDeleteUserButton.Enabled = true;
		}

		private void AutoPlanDeleteUserButtonClick(object sender, EventArgs e)
		{
			var usersBox = autoPlanTabControl.SelectedTab.Controls[0] as ListBox;
			if (usersBox.SelectedItem != null)
				usersBox.Items.Remove(usersBox.SelectedItem);
			autoPlanDeleteUserButton.Enabled = false;
		}

		private void AutoPlanAddUserTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			autoPlanAddUserButton.Enabled = autoPlanAddUserTextBox.Text.Length > 0;
		}

		private void AutoPlanTabControlSelected(object sender, TabControlEventArgs e)
		{
			autoPlanDeleteUserButton.Enabled = false;
		}
	}
}
