using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskPlanningForms.Properties;

namespace TaskPlanningForms
{
	public partial class MainForm : Form
	{
		private const string s_iterationsChangedText = "Iterations changed.";

		private readonly Config m_config;

		private static readonly DataLoader s_dataLoader = new DataLoader();
		private static readonly DataPresenter s_dataPresenter = new DataPresenter();
		private static readonly ScheduleColumnsPresenter s_columnsPresenter = new ScheduleColumnsPresenter(s_dataPresenter.FirstDataColumnIndex);

		private WorkItemCollection m_leadTasks;
		private List<DateTime> m_holidays;

		private string m_lastTfsUrl;
		private List<string> m_lastAreaPaths;
		private List<string> m_lastIterationPaths;
		private bool m_lastWithSubAreas;

		public MainForm()
		{
			InitializeComponent();

			m_config = ConfigManager.LoadConfig();

			s_columnsPresenter.InitColumns(scheduleDataGridView);

			m_holidays = m_config.Holidays;
			s_dataPresenter.SetHolidays(m_holidays);

			tfsUrlTextBox.Text = m_config.TfsUrl;
			if (m_config.AreaPaths != null && m_config.AreaPaths.Count > 0)
			{
				areaPathTextBox.Text = m_config.AreaPaths[0];
				m_config.AreaPaths.ForEach(i => areaPathListBox.Items.Add(i));
			}
			subAreaPathsCheckBox.Checked = m_config.WithSubAreaPaths;

			UpdateHolidays();

			if (m_config.Vacations.Count > 0)
			{
				vacationsButton.Enabled = true;
				var vacationsUsers = m_config.Vacations.Select(v => v.User).ToList();
				vacationsUsers.Sort();
				usersVacationsComboBox.DataSource = vacationsUsers;
				s_dataPresenter.SetVacations(m_config.Vacations);
			}
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
				var column = scheduleDataGridView.Columns[s_dataPresenter.FirstDataColumnIndex + ind];
				var color = m_holidays.Contains(i)
					? CellsPalette.Holiday
					: scheduleDataGridView.Columns[1].HeaderCell.Style.BackColor;
				column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = color;
			}
		}

		private void LoadLeadTasksButtonClick(object sender, EventArgs e)
		{
			holidaysButton.Enabled = false;
			subAreaPathsCheckBox.Enabled = false;
			loadLeadTasksButton.Enabled = false;

			m_config.TfsUrl = tfsUrlTextBox.Text;
			m_config.AreaPaths = areaPathListBox.Items.Cast<object>().Cast<string>().ToList();
			m_config.WithSubAreaPaths = subAreaPathsCheckBox.Checked;

			ThreadPool.QueueUserWorkItem(LoadLeadTasks);
		}

		private void LoadLeadTasks(object state)
		{
			try
			{
				m_lastAreaPaths = areaPathListBox.Items.Cast<object>().Cast<string>().ToList();
				m_leadTasks = s_dataLoader.GetLeadTasks(tfsUrlTextBox.Text, m_lastAreaPaths, subAreaPathsCheckBox.Checked);
			}
			catch (Exception e)
			{
				iterationsComboBox.Invoke(new Action(() =>
					{
						MessageBox.Show(e.Message, Resources.LeadTasksFetchingError);
						holidaysButton.Enabled = true;
						subAreaPathsCheckBox.Enabled = true;
						loadLeadTasksButton.Enabled = true;
					}));
				return;
			}

			var iterations = new List<string>();
			for (int i = 0; i < m_leadTasks.Count; i++)
			{
				string iteration = m_leadTasks[i].IterationPath;
				if (iterations.Contains(iteration))
					continue;
				iterations.Add(iteration);
			}
			iterations.Sort();

			var iterationsColor = Color.White;
			if (m_config.AllIterationPaths != null
				&& m_config.AllIterationPaths.Count > 0
				&& (iterations.Count != m_config.AllIterationPaths.Count
					|| m_config.AllIterationPaths.Any(i => !iterations.Contains(i))))
			{
				iterationsColor = Color.Yellow;
			}
			iterationsComboBox.Invoke(new Action(() =>
				{
					iterationsComboBox.BackColor = iterationsColor;
					if (iterationsColor == Color.Yellow)
						iterationsToolTip.SetToolTip(iterationsComboBox, s_iterationsChangedText);
					else
						iterationsToolTip.RemoveAll();
				}));
			m_config.AllIterationPaths = iterations;

			var validIterations = new List<string>();
			if (m_config.IterationPaths != null && m_config.IterationPaths.Count > 0)
			{
				validIterations = m_config.IterationPaths.Where(iterations.Contains).ToList();
				validIterations.Sort();
			}

			iterationsComboBox.Invoke(new Action(() =>
				{
					iterationsComboBox.DataSource = iterations;
					iterationsComboBox.Enabled = true;
					iterationPathAddButton.Enabled = iterations.Count > 0;

					for (int i = 0; i < iterationPathListBox.Items.Count; i++)
					{
						var iteration = iterationPathListBox.Items[i];
						if (iterations.Contains(iteration.ToString()))
							continue;
						iterationPathListBox.Items.RemoveAt(i);
						--i;
					}

					validIterations.ForEach(i =>
						{
							if (!iterationPathListBox.Items.Contains(i))
								iterationPathListBox.Items.Add(i);
						});

					subAreaPathsCheckBox.Enabled = true;
					loadLeadTasksButton.Enabled = true;
					loadDataButton.Enabled = iterationPathListBox.Items.Count > 0;
					if (iterationPathListBox.Items.Count > 0)
					{
						iterationPathAddButton.Enabled = true;
						iterationPathRemoveButton.Enabled = true;
					}
				}));
		}

		private void LoadDataButtonClick(object sender, EventArgs e)
		{
			loadLeadTasksButton.Enabled = false;
			loadDataButton.Enabled = false;
			usersLabel.Enabled = false;
			usersFilterСomboBox.Enabled = false;
			mainTabControl.SelectTab(mainTabPage);
			scheduleDataGridView.Rows.Clear();

			ThreadPool.QueueUserWorkItem(x => LoadAndPresentData());
		}

		private void LoadAndPresentData()
		{
			List<string> iterationPaths = null;
			iterationsComboBox.Invoke(
				new Action(() => iterationPaths = iterationPathListBox.Items.Cast<object>().Cast<string>().ToList()));
			m_config.IterationPaths = iterationPaths;

			var leadTasks = new List<WorkItem>(m_leadTasks.Count);
			for (int i = 0; i < m_leadTasks.Count; i++)
			{
				var leadTask = m_leadTasks[i];
				if (!iterationPaths.Contains(leadTask.IterationPath))
					continue;
				leadTasks.Add(leadTask);
			}
			m_lastTfsUrl = tfsUrlTextBox.Text;
			m_lastIterationPaths = iterationPaths;
			m_lastWithSubAreas = subAreaPathsCheckBox.Checked;
			try
			{
				var data = s_dataLoader.ProcessLeadTasks(m_lastTfsUrl, leadTasks);

				scheduleDataGridView.Invoke(new Action(() =>
					{
						var users = s_dataPresenter.PresentData(data, scheduleDataGridView);
						usersVacationsComboBox.DataSource = users;
						vacationsButton.Enabled = users.Count > 0;
						var users2 = new List<string>(users);
						users2.Insert(0, string.Empty);
						usersFilterСomboBox.DataSource = users2;
						usersFilterСomboBox.Enabled = true;
						usersLabel.Enabled = true;
						loadDataButton.Enabled = true;
						loadLeadTasksButton.Enabled = true;
						refreshButton.Enabled = true;
						mainTabControl.SelectTab(dataTabPage);
					}));
			}
			catch (Exception e)
			{
				scheduleDataGridView.Invoke(new Action(() =>
					{
						MessageBox.Show(e.Message, Resources.LeadTasksParsinigError);
						loadDataButton.Enabled = true;
						loadLeadTasksButton.Enabled = true;
					}));
			}
		}

		private void UsersFilterСomboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			string user = usersFilterСomboBox.SelectedItem.ToString();
			s_dataPresenter.FilterDataByUser(scheduleDataGridView, user);
		}

		private void RefreshButtonClick(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(x => RefreshData());
		}

		private void RefreshData()
		{
			string currentUser = null;
			tfsUrlTextBox.Invoke(new Action(() =>
				{
					currentUser = usersFilterСomboBox.SelectedItem.ToString();
					refreshButton.Enabled = false;
					loadLeadTasksButton.Enabled = false;
					loadDataButton.Enabled = false;
					usersLabel.Enabled = false;
					usersFilterСomboBox.Enabled = false;
					scheduleDataGridView.Rows.Clear();
				}));
			try
			{
				var leadTasksCollection = s_dataLoader.GetLeadTasks(
					m_lastTfsUrl,
					m_lastAreaPaths.Cast<object>().ToList(),
					m_lastWithSubAreas,
					m_lastIterationPaths.Cast<object>().ToList());
				var leadTasks = new List<WorkItem>(leadTasksCollection.Count);
				for (int i = 0; i < leadTasksCollection.Count; i++)
				{
					leadTasks.Add(leadTasksCollection[i]);
				}
				var data = s_dataLoader.ProcessLeadTasks(tfsUrlTextBox.Text, leadTasks);

				scheduleDataGridView.Invoke(new Action(() =>
				{
					bool isDateChanged = scheduleDataGridView.Columns[s_dataPresenter.FirstDataColumnIndex].HeaderText != DateTime.Now.ToString("dd.MM");
					if (isDateChanged)
						s_columnsPresenter.InitColumns(scheduleDataGridView);
					var users = s_dataPresenter.PresentData(data, scheduleDataGridView);
					usersVacationsComboBox.DataSource = users;
					vacationsButton.Enabled = users.Count > 0;
					var users2 = new List<string>(users);
					users2.Insert(0, string.Empty);
					usersFilterСomboBox.DataSource = users2;

					if (!string.IsNullOrEmpty(currentUser) && users.Contains(currentUser))
					{
						usersFilterСomboBox.SelectedItem = currentUser;
						s_dataPresenter.FilterDataByUser(scheduleDataGridView, currentUser);
					}

					usersFilterСomboBox.Enabled = true;
					usersLabel.Enabled = true;
					loadDataButton.Enabled = true;
					loadLeadTasksButton.Enabled = true;
					refreshButton.Enabled = true;
				}));
			}
			catch (Exception exc)
			{
				scheduleDataGridView.Invoke(new Action(() =>
				{
					MessageBox.Show(exc.Message + Environment.NewLine + exc.StackTrace, Resources.LeadTasksParsinigError);
					loadDataButton.Enabled = true;
					loadLeadTasksButton.Enabled = true;
					refreshButton.Enabled = true;
				}));
			}
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			ConfigManager.SaveConfig(m_config);
		}

		private void HolidaysButtonClick(object sender, EventArgs e)
		{
			var holidaysForm = new HolidaysForm(m_holidays, null);
			holidaysForm.ShowDialog();
			m_holidays = holidaysForm.Holidays;

			m_config.Holidays = m_holidays;
			s_dataPresenter.SetHolidays(m_holidays);
			UpdateHolidays();
		}

		private void AreaPathAddButtonClick(object sender, EventArgs e)
		{
			string areaPath = areaPathTextBox.Text;
			if (areaPathListBox.Items.Contains(areaPath))
				return;
			areaPathListBox.Items.Add(areaPath);
			loadLeadTasksButton.Enabled = true;
			areaPathRemoveButton.Enabled = true;
		}

		private void AreaPathRemoveButtonClick(object sender, EventArgs e)
		{
			areaPathListBox.Items.Remove(areaPathListBox.SelectedItem);
			if (areaPathListBox.Items.Count > 0)
				return;
			loadLeadTasksButton.Enabled = false;
			areaPathRemoveButton.Enabled = false;
		}

		private void IterationPathAddButtonClick(object sender, EventArgs e)
		{
			string iterationPath = iterationsComboBox.Text;
			if (iterationPathListBox.Items.Contains(iterationPath))
				return;
			int ind = 0;
			for (; ind < iterationPathListBox.Items.Count; ind++)
			{
				if (string.Compare(iterationPathListBox.Items[ind].ToString(), iterationPath, StringComparison.Ordinal) <= 0)
					continue;
				iterationPathListBox.Items.Insert(ind, iterationPath);
				break;
			}
			if (ind == iterationPathListBox.Items.Count)
				iterationPathListBox.Items.Add(iterationPath);
			
			loadDataButton.Enabled = true;
			iterationPathRemoveButton.Enabled = true;
		}

		private void IterationPathRemoveButtonClick(object sender, EventArgs e)
		{
			iterationPathListBox.Items.Remove(iterationPathListBox.SelectedItem);
			if (iterationPathListBox.Items.Count > 0)
				return;
			loadDataButton.Enabled = false;
			iterationPathRemoveButton.Enabled = false;
		}

		private void DevCmpletedCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool withDevCompleted = devCmpletedCheckBox.Checked;
			s_dataPresenter.FilterDataByDevCompleted(scheduleDataGridView, withDevCompleted);
		}

		private void LtOnlyCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool ltOnly = ltOnlyCheckBox.Checked;
			s_dataPresenter.FilterDataByLTMode(scheduleDataGridView, ltOnly);
		}

		private void VacationsButtonClick(object sender, EventArgs e)
		{
			string user = usersVacationsComboBox.Text;
			var userVacations = m_config.Vacations.FirstOrDefault(v => v.User == user);
			var holidaysForm = new HolidaysForm(userVacations != null ? userVacations.VacationDays : new List<DateTime>(), user);
			holidaysForm.ShowDialog();
			if (userVacations != null)
				m_config.Vacations.Remove(userVacations);
			if (holidaysForm.Holidays.Count > 0)
				m_config.Vacations.Add(new VacationData { User = user, VacationDays = holidaysForm.Holidays });
			s_dataPresenter.SetVacations(m_config.Vacations);
		}

		private void ShowBlockersCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool expandBlockers = expandBlockersCheckBox.Checked;
			s_dataPresenter.ExpandBlockers(scheduleDataGridView, expandBlockers);
		}
	}
}
