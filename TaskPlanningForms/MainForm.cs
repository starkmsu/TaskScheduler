using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Properties;

namespace TaskSchedulerForms
{
	public partial class MainForm : Form
	{
		private readonly Config m_config;

		private static readonly DataLoader s_dataLoader = new DataLoader();
		private static readonly DataProcessor s_dataProcessor = new DataProcessor();
		private static readonly DataPresenter s_dataPresenter = new DataPresenter();

		private static ViewColumnsIndexes s_viewColumnsIndexes;
		private static ScheduleColumnsPresenter s_columnsPresenter;

		private WorkItemCollection m_leadTasks;
		private List<DateTime> m_holidays;

		private string m_lastTfsUrl;
		private List<string> m_lastAreaPaths;
		private List<string> m_lastIterationPaths;
		private bool m_lastWithSubAreas;

		private bool m_isAreaFirstMode = true;

		private ViewFiltersApplier _viewFiltersApplier;

		public MainForm()
		{
			InitializeComponent();

			m_config = ConfigManager.LoadConfig();

			s_viewColumnsIndexes = new ViewColumnsIndexes(scheduleDataGridView);
			s_columnsPresenter = new ScheduleColumnsPresenter(s_viewColumnsIndexes.FirstDateColumnIndex);
			s_columnsPresenter.InitColumns(scheduleDataGridView);

			m_holidays = m_config.Holidays;
			s_dataPresenter.SetHolidays(m_holidays);

			tfsUrlTextBox.Text = m_config.TfsUrl;
			if (m_config.AreaPaths != null && m_config.AreaPaths.Count > 0)
			{
				firstTextBox.Text = m_config.AreaPaths[0];
				m_config.AreaPaths.ForEach(i => firstListBox.Items.Add(i));
			}
			subTreesCheckBox.Checked = m_config.WithSubAreaPaths;

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
				var column = scheduleDataGridView.Columns[s_viewColumnsIndexes.FirstDateColumnIndex + ind];
				var color = m_holidays.Contains(i)
					? CellsPalette.Holiday
					: scheduleDataGridView.Columns[1].HeaderCell.Style.BackColor;
				column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = color;
			}
		}

		private void LoadLeadTasksButtonClick(object sender, EventArgs e)
		{
			holidaysButton.Enabled = false;
			subTreesCheckBox.Enabled = false;
			loadLeadTasksButton.Enabled = false;
			refreshButton.Enabled = false;
			refreshButton.BackColor = Color.Transparent;

			m_config.TfsUrl = tfsUrlTextBox.Text;
			m_config.WithSubAreaPaths = subTreesCheckBox.Checked;
			if (m_isAreaFirstMode)
				m_config.AreaPaths = firstListBox.Items.Cast<string>().ToList();
			else
				m_config.IterationPaths = firstListBox.Items.Cast<string>().ToList();

			ThreadPool.QueueUserWorkItem(LoadLeadTasks);
		}

		private void LoadLeadTasks(object state)
		{
			try
			{
				if (m_isAreaFirstMode)
				{
					m_lastAreaPaths = firstListBox.Items.Cast<string>().ToList();
					m_leadTasks = s_dataLoader.GetLeadTasksByAreas(
						tfsUrlTextBox.Text,
						m_lastAreaPaths,
						subTreesCheckBox.Checked);
				}
				else
				{
					m_lastIterationPaths = firstListBox.Items.Cast<string>().ToList();
					m_leadTasks = s_dataLoader.GetLeadTasksByIterations(
						tfsUrlTextBox.Text,
						m_lastIterationPaths,
						subTreesCheckBox.Checked);
				}
			}
			catch (Exception e)
			{
				secondComboBox.Invoke(new Action(() =>
					{
						MessageBox.Show(e.Message, Resources.LeadTasksFetchingError);
						holidaysButton.Enabled = true;
						subTreesCheckBox.Enabled = true;
						loadLeadTasksButton.Enabled = true;
					}));
				return;
			}

			FillSecondList();
		}

		private void FillSecondList()
		{
			var secondList = new List<string>();
			for (int i = 0; i < m_leadTasks.Count; i++)
			{
				string second = m_isAreaFirstMode ? m_leadTasks[i].IterationPath : m_leadTasks[i].AreaPath;
				if (secondList.Contains(second))
					continue;
				secondList.Add(second);
			}
			secondList.Sort();

			var newSecond = new List<string>(secondList.Count);
			if (m_isAreaFirstMode)
			{
				if (m_config.AllIterationPaths != null && m_config.AllIterationPaths.Count > 0)
					newSecond.AddRange(secondList.Where(i => !m_config.AllIterationPaths.Contains(i)));
			}
			else
			{
				if (m_config.AllAreaPaths != null && m_config.AllAreaPaths.Count > 0)
					newSecond.AddRange(secondList.Where(i => !m_config.AllAreaPaths.Contains(i)));
			}

			secondComboBox.Invoke(new Action(() =>
			{
				secondComboBox.BackColor = newSecond.Count > 0 ? Color.Yellow : Color.White;
				if (newSecond.Count > 0)
					secondToolTip.SetToolTip(
						secondComboBox,
						Resources.NewItems + Environment.NewLine + string.Join(Environment.NewLine, newSecond));
				else
					secondToolTip.RemoveAll();
			}));

			if (m_isAreaFirstMode)
				m_config.AllIterationPaths = secondList;
			else
				m_config.AllAreaPaths = secondList;

			var validSecond = new List<string>();
			if (m_isAreaFirstMode
				&& m_config.IterationPaths != null
				&& m_config.IterationPaths.Count > 0)
				validSecond = m_config.IterationPaths.Where(secondList.Contains).ToList();
			else if (!m_isAreaFirstMode
				&& m_config.AreaPaths != null
				&& m_config.AreaPaths.Count > 0)
				validSecond = m_config.AreaPaths.Where(secondList.Contains).ToList();
			validSecond.Sort();

			secondComboBox.Invoke(new Action(() =>
			{
				secondComboBox.DataSource = secondList;
				secondComboBox.Enabled = true;
				secondAddButton.Enabled = secondList.Count > 0;

				for (int i = 0; i < secondListBox.Items.Count; i++)
				{
					var second = secondListBox.Items[i];
					if (secondList.Contains(second.ToString()))
						continue;
					secondListBox.Items.RemoveAt(i);
					--i;
				}

				validSecond.ForEach(i =>
				{
					if (!secondListBox.Items.Contains(i))
						secondListBox.Items.Add(i);
				});

				subTreesCheckBox.Enabled = true;
				loadLeadTasksButton.Enabled = true;
				makeScheduleButton.Enabled = secondListBox.Items.Count > 0;
				if (secondListBox.Items.Count > 0)
				{
					secondAddButton.Enabled = true;
					secondRemoveButton.Enabled = true;
				}
			}));
		}

		private void MakeScheduleButtonClick(object sender, EventArgs e)
		{
			loadLeadTasksButton.Enabled = false;
			makeScheduleButton.Enabled = false;
			usersLabel.Enabled = false;
			usersFilterСomboBox.Enabled = false;
			mainTabControl.SelectTab(mainTabPage);
			scheduleDataGridView.Rows.Clear();
			refreshButton.Enabled = false;
			refreshButton.BackColor = Color.Transparent;

			ThreadPool.QueueUserWorkItem(x => ProcessData());
		}

		private void ProcessData()
		{
			List<string> firstList = null;
			List<string> secondList = null;
			secondComboBox.Invoke(new Action(() =>
				{
					firstList = firstListBox.Items.Cast<string>().ToList();
					secondList = secondListBox.Items.Cast<string>().ToList();
				}));
			if (m_isAreaFirstMode)
				m_config.IterationPaths = secondList;
			else
				m_config.AreaPaths = secondList;

			var leadTasks = new List<WorkItem>(m_leadTasks.Count);
			for (int i = 0; i < m_leadTasks.Count; i++)
			{
				var leadTask = m_leadTasks[i];
				if (!firstList.Contains(m_isAreaFirstMode ? leadTask.AreaPath : leadTask.IterationPath)
					||!secondList.Contains(m_isAreaFirstMode ? leadTask.IterationPath : leadTask.AreaPath))
					continue;
				leadTasks.Add(leadTask);
			}

			m_lastTfsUrl = tfsUrlTextBox.Text;
			m_lastWithSubAreas = subTreesCheckBox.Checked;
			if (m_isAreaFirstMode)
				m_lastIterationPaths = secondList;
			else
				m_lastAreaPaths = secondList;

			try
			{
				var data = s_dataProcessor.ProcessLeadTasks(m_lastTfsUrl, leadTasks);

				scheduleDataGridView.Invoke(new Action(() =>
					{
						_viewFiltersApplier = s_dataPresenter.PresentData(data, s_viewColumnsIndexes, scheduleDataGridView);
						usersVacationsComboBox.DataSource = _viewFiltersApplier.Users;
						vacationsButton.Enabled = _viewFiltersApplier.Users.Count > 0;
						var users2 = new List<string>(_viewFiltersApplier.Users);
						users2.Insert(0, string.Empty);
						usersFilterСomboBox.DataSource = users2;
						usersFilterСomboBox.Enabled = true;
						usersLabel.Enabled = true;
						makeScheduleButton.Enabled = true;
						loadLeadTasksButton.Enabled = true;
						refreshButton.Enabled = true;
						mainTabControl.SelectTab(dataTabPage);
					}));
			}
			catch (Exception exc)
			{
				scheduleDataGridView.Invoke(new Action(() =>
					{
						MessageBox.Show(exc.Message + Environment.NewLine + exc.StackTrace, Resources.LeadTasksParsingError);
						makeScheduleButton.Enabled = true;
						loadLeadTasksButton.Enabled = true;
					}));
			}
		}

		private void UsersFilterСomboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			string user = usersFilterСomboBox.SelectedItem.ToString();
			_viewFiltersApplier.FilterDataByUser(user);
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
					makeScheduleButton.Enabled = false;
					usersLabel.Enabled = false;
					usersFilterСomboBox.Enabled = false;
					scheduleDataGridView.Rows.Clear();
				}));
			try
			{
				WorkItemCollection leadTasksCollection;
				if (m_isAreaFirstMode)
					leadTasksCollection = s_dataLoader.GetLeadTasksByAreas(
						m_lastTfsUrl,
						m_lastAreaPaths,
						m_lastWithSubAreas);
				else
					leadTasksCollection = s_dataLoader.GetLeadTasksByIterations(
						m_lastTfsUrl,
						m_lastIterationPaths,
						m_lastWithSubAreas);

				var leadTasks = new List<WorkItem>(leadTasksCollection.Count);
				var newSecond = new List<string>(m_isAreaFirstMode ? m_lastIterationPaths.Count : m_lastAreaPaths.Count);
				for (int i = 0; i < leadTasksCollection.Count; i++)
				{
					WorkItem leadTask = leadTasksCollection[i];
					if (m_isAreaFirstMode)
					{
						if (!m_config.AllIterationPaths.Contains(leadTask.IterationPath)
							&& !newSecond.Contains(leadTask.IterationPath))
								newSecond.Add(leadTask.IterationPath);
						if (!m_lastIterationPaths.Contains(leadTask.IterationPath))
							continue;
					}
					else
					{
						if (!m_config.AllAreaPaths.Contains(leadTask.AreaPath)
							&& !newSecond.Contains(leadTask.AreaPath))
							newSecond.Add(leadTask.AreaPath);
						if (!m_lastAreaPaths.Contains(leadTask.AreaPath))
							continue;
					}
					leadTasks.Add(leadTask);
				}
				var data = s_dataProcessor.ProcessLeadTasks(tfsUrlTextBox.Text, leadTasks);

				scheduleDataGridView.Invoke(new Action(() =>
				{
					bool isDateChanged = scheduleDataGridView.Columns[s_viewColumnsIndexes.FirstDateColumnIndex].HeaderText != DateTime.Now.ToString("dd.MM");
					if (isDateChanged)
						s_columnsPresenter.InitColumns(scheduleDataGridView);
					_viewFiltersApplier = s_dataPresenter.PresentData(data, s_viewColumnsIndexes, scheduleDataGridView);
					usersVacationsComboBox.DataSource = _viewFiltersApplier.Users;
					vacationsButton.Enabled = _viewFiltersApplier.Users.Count > 0;
					var users2 = new List<string>(_viewFiltersApplier.Users);
					users2.Insert(0, string.Empty);
					usersFilterСomboBox.DataSource = users2;

					if (!string.IsNullOrEmpty(currentUser) && _viewFiltersApplier.Users.Contains(currentUser))
					{
						usersFilterСomboBox.SelectedItem = currentUser;
						_viewFiltersApplier.FilterDataByUser(currentUser);
					}

					usersFilterСomboBox.Enabled = true;
					usersLabel.Enabled = true;
					makeScheduleButton.Enabled = true;
					loadLeadTasksButton.Enabled = true;
					refreshButton.Enabled = true;
					if (newSecond.Count > 0)
					{
						refreshButton.BackColor = Color.Yellow;
						secondToolTip.SetToolTip(refreshButton,
							Resources.NewItems + Environment.NewLine + string.Join(Environment.NewLine, newSecond));
					}

					if (ltOnlyCheckBox.Checked)
						_viewFiltersApplier.FilterDataByLeadTaskMode(ltOnlyCheckBox.Checked);
					if (expandBlockersCheckBox.Checked)
						_viewFiltersApplier.ExpandBlockers(expandBlockersCheckBox.Checked);
				}));
			}
			catch (Exception exc)
			{
				scheduleDataGridView.Invoke(new Action(() =>
				{
					MessageBox.Show(exc.Message + Environment.NewLine + exc.StackTrace, Resources.LeadTasksParsingError);
					makeScheduleButton.Enabled = true;
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

		private void FirstAddButtonClick(object sender, EventArgs e)
		{
			string first = firstTextBox.Text;
			if (firstListBox.Items.Contains(first))
				return;
			firstListBox.Items.Add(first);
			loadLeadTasksButton.Enabled = true;
			firstRemoveButton.Enabled = true;
			makeScheduleButton.Enabled = false;
		}

		private void FirstRemoveButtonClick(object sender, EventArgs e)
		{
			firstListBox.Items.Remove(firstListBox.SelectedItem);
			if (firstListBox.Items.Count > 0)
				return;
			loadLeadTasksButton.Enabled = false;
			firstRemoveButton.Enabled = false;
			makeScheduleButton.Enabled = false;
		}

		private void SecondAddButtonClick(object sender, EventArgs e)
		{
			string second = secondComboBox.Text;
			if (secondListBox.Items.Contains(second))
				return;
			int ind = 0;
			for (; ind < secondListBox.Items.Count; ind++)
			{
				if (string.Compare(secondListBox.Items[ind].ToString(), second, StringComparison.Ordinal) <= 0)
					continue;
				secondListBox.Items.Insert(ind, second);
				break;
			}
			if (ind == secondListBox.Items.Count)
				secondListBox.Items.Add(second);
			
			makeScheduleButton.Enabled = true;
			secondRemoveButton.Enabled = true;
		}

		private void SecondRemoveButtonClick(object sender, EventArgs e)
		{
			secondListBox.Items.Remove(secondListBox.SelectedItem);
			if (secondListBox.Items.Count > 0)
				return;
			makeScheduleButton.Enabled = false;
			secondRemoveButton.Enabled = false;
		}

		private void DevCmpletedCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool withDevCompleted = devCmpletedCheckBox.Checked;
			_viewFiltersApplier.FilterDataByDevCompleted(withDevCompleted);
		}

		private void LtOnlyCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool ltOnly = ltOnlyCheckBox.Checked;
			_viewFiltersApplier.FilterDataByLeadTaskMode(ltOnly);
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
			_viewFiltersApplier.ExpandBlockers(expandBlockers);
		}

		private void ExchangeButtonClick(object sender, EventArgs e)
		{
			m_isAreaFirstMode = !m_isAreaFirstMode;
			string tmp = firstGroupBox.Text;
			firstGroupBox.Text = secondGroupBox.Text;
			secondGroupBox.Text = tmp;
			var itemsCopy = new object[firstListBox.Items.Count];
			firstTextBox.Text = secondListBox.Items.Count > 0
				? secondListBox.Items[0].ToString()
				: string.Empty;
			firstListBox.Items.CopyTo(itemsCopy, 0);
			firstListBox.Items.Clear();
			firstListBox.Items.AddRange(secondListBox.Items);
			secondListBox.Items.Clear();
			secondListBox.Items.AddRange(itemsCopy);
			secondComboBox.Text = string.Empty;
			secondComboBox.DataSource = itemsCopy;
		}
	}
}
