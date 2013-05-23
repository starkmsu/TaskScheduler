using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TaskPlanningForms
{
	public partial class MainForm : Form
	{
		private const int m_indShift = 6;

		private readonly Config m_config;

		private readonly DataLoader m_dataLoader = new DataLoader();
		private readonly HolidaysStorage m_holidaysStorage = new HolidaysStorage();
		private readonly DataPresenter m_dataPresenter = new DataPresenter(m_indShift);

		private WorkItemCollection m_leadTasks;
		private List<DateTime> m_holidays;

		public MainForm()
		{
			InitializeComponent();

			m_config = ConfigManager.LoadConfig();

			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				string dateText = i.Date.ToString("dd.MM");
				var column = new DataGridViewTextBoxColumn
				{
					Name = dateText,
					HeaderText = dateText,
					Width = 40
				};
				if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
					column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = CellsPalette.WeekEnd;
				scheduleDataGridView.Columns.Add(column);
			}

			m_holidays = m_holidaysStorage.LoadHolidays();
			m_dataPresenter.Holidays = m_holidays;

			tfsUrlTextBox.Text = m_config.TfsUrl;

			UpdateHolidays();
		}

		private void UpdateHolidays()
		{
			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			int ind = -1;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				++ind;
				if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
					continue;
				var column = scheduleDataGridView.Columns[m_indShift + ind];
				var color = m_holidays.Contains(i)
					? CellsPalette.Holiday
					: scheduleDataGridView.Columns[1].HeaderCell.Style.BackColor;
				column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = color;
			}
		}

		private void LoadLeadTasksButtonClick(object sender, EventArgs e)
		{
			setHolidaysButton.Enabled = false;
			loadLeadTasksButton.Enabled = false;

			m_config.TfsUrl = tfsUrlTextBox.Text;

			ThreadPool.QueueUserWorkItem(LoadLeadTasks);
		}

		private void LoadLeadTasks(object state)
		{
			m_leadTasks = m_dataLoader.GetLeadTasks(tfsUrlTextBox.Text, areaPathTextBox.Text);

			var iterations = new List<string>();
			for (int i = 0; i < m_leadTasks.Count; i++)
			{
				string iteration = m_leadTasks[i].IterationPath;
				if (iterations.Contains(iteration))
					continue;
				iterations.Add(iteration);
			}
			iterations.Sort();

			iterationsComboBox.Invoke(new Action(() =>
			{
				iterationsComboBox.DataSource = iterations;
				iterationsComboBox.SelectedIndex = 0;
				iterationsComboBox.Enabled = true;

				loadLeadTasksButton.Enabled = true;
				loadDataButton.Enabled = true;
			}));
		}

		private void LoadDataButtonClick(object sender, EventArgs e)
		{
			loadLeadTasksButton.Enabled = false;
			loadDataButton.Enabled = false;
			usersLabel.Enabled = false;
			usersСomboBox.Enabled = false;
			mainTabControl.SelectTab(settingsTabPage);
			scheduleDataGridView.Rows.Clear();

			ThreadPool.QueueUserWorkItem(x => LoadAndPresentData());
		}

		private void LoadAndPresentData()
		{
			string iterationPath = null;
			iterationsComboBox.Invoke(new Action(() => iterationPath = iterationsComboBox.Text));

			var leadTasks = new List<WorkItem>(m_leadTasks.Count);
			for (int i = 0; i < m_leadTasks.Count; i++)
			{
				var leadTask = m_leadTasks[i];
				if (leadTask.IterationPath != iterationPath)
					continue;
				leadTasks.Add(leadTask);
			}
			var data = m_dataLoader.ProcessLeadTasks(tfsUrlTextBox.Text, leadTasks);

			scheduleDataGridView.Invoke(new Action(() =>
			{
				usersСomboBox.DataSource = m_dataPresenter.PresentData(data, scheduleDataGridView);

				usersСomboBox.Enabled = true;
				usersLabel.Enabled = true;
				loadDataButton.Enabled = true;
				loadLeadTasksButton.Enabled = true;
				mainTabControl.SelectTab(dataTabPage);
			}));
		}

		private void UsersСomboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			string user = usersСomboBox.SelectedItem.ToString();
			m_dataPresenter.FilterDataByUser(user, scheduleDataGridView);
		}

		private void SetHolidaysButtonClick(object sender, EventArgs e)
		{
			var oldHolidays = new List<DateTime>(m_holidays);
			var holidaysForm = new HolidaysForm(m_holidays);
			holidaysForm.ShowDialog();
			m_holidays = holidaysForm.Holidays;

			m_holidaysStorage.SaveHolidays(m_holidays);
			m_dataPresenter.Holidays = m_holidays;
			if (oldHolidays.Count != m_holidays.Count
				|| oldHolidays.Any(h => !m_holidays.Contains(h)))
			{
				
			}
			UpdateHolidays();
		}

		private void RefreshButtonClick(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(x => RefreshData());
		}

		private void RefreshData()
		{
			string tfsUrl = null, areaPath = null, iterationPath = null, currentUser = null;
			tfsUrlTextBox.Invoke(new Action(() =>
				{
					tfsUrl = tfsUrlTextBox.Text;
					areaPath = areaPathTextBox.Text;
					iterationPath = iterationsComboBox.Text;
					currentUser = usersСomboBox.SelectedItem.ToString();
					refreshButton.Enabled = false;
					loadLeadTasksButton.Enabled = false;
					loadDataButton.Enabled = false;
					usersLabel.Enabled = false;
					usersСomboBox.Enabled = false;
					scheduleDataGridView.Rows.Clear();
				}));

			var leadTasksCollection = m_dataLoader.GetLeadTasks(tfsUrl, areaPath, iterationPath);
			var leadTasks = new List<WorkItem>(leadTasksCollection.Count);
			for (int i = 0; i < leadTasksCollection.Count; i++)
			{
				leadTasks.Add(leadTasksCollection[i]);
			}
			var data = m_dataLoader.ProcessLeadTasks(tfsUrlTextBox.Text, leadTasks);

			scheduleDataGridView.Invoke(new Action(() =>
			{
				var users = m_dataPresenter.PresentData(data, scheduleDataGridView);
				usersСomboBox.DataSource = users;

				if (!string.IsNullOrEmpty(currentUser) && users.Contains(currentUser))
				{
					usersСomboBox.SelectedItem = currentUser;
					m_dataPresenter.FilterDataByUser(currentUser, scheduleDataGridView);
				}

				usersСomboBox.Enabled = true;
				usersLabel.Enabled = true;
				loadDataButton.Enabled = true;
				loadLeadTasksButton.Enabled = true;
				refreshButton.Enabled = true;
			}));
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			ConfigManager.SaveConfig(m_config);
		}
	}
}
