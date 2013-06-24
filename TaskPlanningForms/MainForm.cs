using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskPlanningForms.Properties;

namespace TaskPlanningForms
{
	public partial class MainForm : Form
	{
		private const int m_indShift = 6;

		private readonly Config m_config;

		private readonly DataLoader m_dataLoader = new DataLoader();
		private readonly DataPresenter m_dataPresenter = new DataPresenter(m_indShift);

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

			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				string dateText = i.Date.ToString("dd.MM");
				var column = new DataGridViewTextBoxColumn
				{
					Name = dateText,
					HeaderText = dateText,
					Width = 40,
					Resizable = DataGridViewTriState.False
				};
				if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
					column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = CellsPalette.WeekEnd;
				scheduleDataGridView.Columns.Add(column);
			}

			m_holidays = m_config.Holidays;
			m_dataPresenter.Holidays = m_holidays;

			tfsUrlTextBox.Text = m_config.TfsUrl;
			if (m_config.AreaPaths != null && m_config.AreaPaths.Count > 0)
			{
				areaPathTextBox.Text = m_config.AreaPaths[0];
				m_config.AreaPaths.ForEach(i => areaPathListBox.Items.Add(i));
			}
			subAreaPathsCheckBox.Checked = m_config.WithSubAreaPaths;

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
				m_leadTasks = m_dataLoader.GetLeadTasks(tfsUrlTextBox.Text, m_lastAreaPaths, subAreaPathsCheckBox.Checked);
			}
			catch (Exception e)
			{
				iterationsComboBox.Invoke(new Action(() =>
					{
						MessageBox.Show(e.Message, Resources.LeadTasksFetchingError);
						setHolidaysButton.Enabled = true;
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
			var validIterations = new List<string>();
			if (m_config.IterationPaths != null && m_config.IterationPaths.Count > 0)
				validIterations = m_config.IterationPaths.Where(iterations.Contains).ToList();

			iterationsComboBox.Invoke(new Action(() =>
				{
					iterationsComboBox.DataSource = iterations;
					iterationsComboBox.Enabled = true;
					iterationPathAddButton.Enabled = iterations.Count > 0;
					validIterations.ForEach(i =>
						{
							if (!iterationPathListBox.Items.Contains(i))
								iterationPathListBox.Items.Add(i);
						});

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
			usersСomboBox.Enabled = false;
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
				var data = m_dataLoader.ProcessLeadTasks(m_lastTfsUrl, leadTasks);

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

		private void UsersСomboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			string user = usersСomboBox.SelectedItem.ToString();
			m_dataPresenter.FilterDataByUser(scheduleDataGridView, user);
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
					currentUser = usersСomboBox.SelectedItem.ToString();
					refreshButton.Enabled = false;
					loadLeadTasksButton.Enabled = false;
					loadDataButton.Enabled = false;
					usersLabel.Enabled = false;
					usersСomboBox.Enabled = false;
					scheduleDataGridView.Rows.Clear();
				}));
			try
			{
				var leadTasksCollection = m_dataLoader.GetLeadTasks(
					m_lastTfsUrl,
					m_lastAreaPaths.Cast<object>().ToList(),
					m_lastWithSubAreas,
					m_lastIterationPaths.Cast<object>().ToList());
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
						m_dataPresenter.FilterDataByUser(scheduleDataGridView, currentUser);
					}

					usersСomboBox.Enabled = true;
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
					MessageBox.Show(exc.Message, Resources.LeadTasksParsinigError);
					loadDataButton.Enabled = true;
					loadLeadTasksButton.Enabled = true;
				}));
			}
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			ConfigManager.SaveConfig(m_config);
		}

		private void SetHolidaysButtonClick(object sender, EventArgs e)
		{
			var holidaysForm = new HolidaysForm(m_holidays);
			holidaysForm.ShowDialog();
			m_holidays = holidaysForm.Holidays;

			m_config.Holidays = m_holidays;
			m_dataPresenter.Holidays = m_holidays;
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
			m_dataPresenter.FilterDataByDevCompleted(scheduleDataGridView, withDevCompleted);
		}

		private void LtOnlyCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool ltOnly = ltOnlyCheckBox.Checked;
			m_dataPresenter.FilterDataByLTMode(scheduleDataGridView, ltOnly);
		}
	}
}
