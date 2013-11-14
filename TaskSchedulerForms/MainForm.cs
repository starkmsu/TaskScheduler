using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Properties;
using TfsUtils.Parsers;

namespace TaskSchedulerForms
{
	public partial class MainForm : Form
	{
		private readonly Config m_config;

		private static readonly DataLoader s_dataLoader = new DataLoader();
		private static readonly DataProcessor s_dataProcessor = new DataProcessor();
		private static readonly DataPresenter s_dataPresenter = new DataPresenter();
		private static readonly StateContainer s_stateContainer = new StateContainer();

		private static ViewColumnsIndexes s_viewColumnsIndexes;
		private static ScheduleColumnsPresenter s_columnsPresenter;

		private WorkItemCollection m_leadTasks;
		private List<DateTime> m_holidays;

		private ViewFiltersApplier m_viewFiltersApplier;

		public MainForm()
		{
			InitializeComponent();

			m_config = ConfigManager.LoadConfig();

			s_viewColumnsIndexes = new ViewColumnsIndexes(scheduleDataGridView);
			s_columnsPresenter = new ScheduleColumnsPresenter(s_viewColumnsIndexes.FirstDateColumnIndex);
			s_columnsPresenter.InitColumns(scheduleDataGridView);

			m_holidays = m_config.Holidays;
			s_dataPresenter.SetHolidays(m_holidays);

			UpdateHolidays();

			if (m_config.Vacations.Count > 0)
			{
				vacationsButton.Enabled = true;
				var vacationsUsers = m_config.Vacations.Select(v => v.User).ToList();
				vacationsUsers.Sort();
				usersVacationsComboBox.DataSource = vacationsUsers;
				s_dataPresenter.SetVacations(m_config.Vacations);
			}

			tfsUrlTextBox.Text = m_config.TfsUrl;
			s_stateContainer.ByArea = m_config.ByArea;
			if (m_config.WorkMode != WorkMode.Query)
			{
				s_stateContainer.WorkMode = m_config.WorkMode;
				InitFirst();
				queryTextBox.Text = Resources.QueryExample;
			}
			else
			{
				queryTextBox.Text = m_config.QueryPath;
				queryTextBox.ForeColor = Color.Black;
				ParamsGroupBox.Enabled = false;
				makeScheduleButton.Enabled = true;
			}
		}

		private void InitFirst()
		{
			subTreesCheckBox.Checked = m_config.WithSubAreaPaths;
			List<string> firstList = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.AreaPaths : m_config.IterationPaths;
			if (firstList != null && firstList.Count > 0)
			{
				firstTextBox.Text = firstList[0];
				firstList.ForEach(i => firstListBox.Items.Add(i));
			}
			if (s_stateContainer.WorkMode == WorkMode.IterationFirst)
				ExchangeNames();
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
			s_stateContainer.SaveChosenFirstToConfig(m_config, firstListBox.Items.Cast<string>().ToList());

			ThreadPool.QueueUserWorkItem(LoadLeadTasks);
		}

		private void LoadLeadTasks(object state)
		{
			try
			{
				s_stateContainer.SaveChosenFirstToState(firstListBox.Items.Cast<string>().ToList());
				m_leadTasks = s_dataLoader.GetLeadTasks(
					tfsUrlTextBox.Text,
					s_stateContainer.WorkMode == WorkMode.AreaFirst,
					s_stateContainer.GetFirstList(),
					subTreesCheckBox.Checked);
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
				string second = s_stateContainer.GetParamForSecond(m_leadTasks[i]);
				if (secondList.Contains(second))
					continue;
				secondList.Add(second);
			}
			secondList.Sort();

			var newSecond = new List<string>(secondList.Count);
			var oldSecond = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.AllIterationPaths : m_config.AllAreaPaths;
			if (oldSecond != null && oldSecond.Count > 0)
				newSecond.AddRange(secondList.Where(i => !oldSecond.Contains(i)));

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

			s_stateContainer.SaveAllSecondToConfig(m_config, secondList);

			var validSecond = new List<string>();
			var configSecond = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.IterationPaths : m_config.AreaPaths;
			if (configSecond != null && configSecond.Count > 0)
				validSecond = configSecond.Where(secondList.Contains).ToList();
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
			makeScheduleButton.Enabled = false;
			loadLeadTasksButton.Enabled = false;
			queryTextBox.Enabled = false;

			usersLabel.Enabled = false;
			usersFilterСomboBox.Enabled = false;
			mainTabControl.SelectTab(mainTabPage);
			scheduleDataGridView.Rows.Clear();
			refreshButton.Enabled = false;
			refreshButton.BackColor = Color.Transparent;

			ThreadPool.QueueUserWorkItem(x => ProcessData());
		}

		private List<WorkItem> GetLeadTasks()
		{
			List<WorkItem> result;
			if (s_stateContainer.WorkMode != WorkMode.Query)
			{
				s_stateContainer.LastWithSubTree = subTreesCheckBox.Checked;

				List<string> firstList = null;
				List<string> secondList = null;
				secondComboBox.Invoke(new Action(() =>
					{
						firstList = firstListBox.Items.Cast<string>().ToList();
						secondList = secondListBox.Items.Cast<string>().ToList();
					}));
				s_stateContainer.SaveChosenSecondToConfig(m_config, secondList);
				s_stateContainer.SaveChosenSecondToState(secondList);

				result = new List<WorkItem>(m_leadTasks.Count);
				for (int i = 0; i < m_leadTasks.Count; i++)
				{
					var leadTask = m_leadTasks[i];
					if (!firstList.Contains(s_stateContainer.GetParamForFirst(leadTask))
						|| !secondList.Contains(s_stateContainer.GetParamForSecond(leadTask)))
						continue;
					result.Add(leadTask);
				}
			}
			else
			{
				var items = s_dataLoader.GetLeadTasks(tfsUrlTextBox.Text, queryTextBox.Text);
				m_config.TfsUrl = tfsUrlTextBox.Text;
				m_config.WorkMode = WorkMode.Query;
				m_config.QueryPath = queryTextBox.Text;
				result = new List<WorkItem>(items.Count);
				for (int i = 0; i < items.Count; i++)
				{
					WorkItem item = items[i];
					if (item.Type.Name == TfsUtils.Const.WorkItemType.LeadTask)
						result.Add(item);
				}
				result = result.OrderBy(i => i.Priority()).ToList();
			}

			return result;
		}

		private void ProcessData()
		{
			s_stateContainer.LastTfsUrl = tfsUrlTextBox.Text;

			try
			{
				List<WorkItem> leadTasks = GetLeadTasks();

				var data = s_dataProcessor.ProcessLeadTasks(s_stateContainer.LastTfsUrl, leadTasks);

				scheduleDataGridView.Invoke(new Action(() =>
					{
						m_viewFiltersApplier = s_dataPresenter.PresentData(data, s_viewColumnsIndexes, scheduleDataGridView);
						usersVacationsComboBox.DataSource = m_viewFiltersApplier.Users;
						vacationsButton.Enabled = m_viewFiltersApplier.Users.Count > 0;
						var users2 = new List<string>(m_viewFiltersApplier.Users);
						users2.Insert(0, string.Empty);
						usersFilterСomboBox.DataSource = users2;
						usersFilterСomboBox.Enabled = true;
						usersLabel.Enabled = true;
						makeScheduleButton.Enabled = true;
						loadLeadTasksButton.Enabled = true;
						queryTextBox.Enabled = true;
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
						queryTextBox.Enabled = true;
					}));
			}
		}

		private void UsersFilterСomboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			string user = usersFilterСomboBox.SelectedItem.ToString();
			m_viewFiltersApplier.FilterDataByUser(user);
		}

		private void RefreshButtonClick(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(x => RefreshData());
		}

		private List<WorkItem> GetLastLeadTasks()
		{

			WorkItemCollection items;
			if (s_stateContainer.WorkMode != WorkMode.Query)
				items = s_dataLoader.GetLeadTasks(
					s_stateContainer.LastTfsUrl,
					s_stateContainer.WorkMode == WorkMode.AreaFirst,
					s_stateContainer.GetFirstList(),
					s_stateContainer.LastWithSubTree);
			else
				items = s_dataLoader.GetLeadTasks(tfsUrlTextBox.Text, queryTextBox.Text);

			var result = new List<WorkItem>(items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				WorkItem item = items[i];
				if (item.Type.Name == TfsUtils.Const.WorkItemType.LeadTask)
					result.Add(item);
			}

			return result;
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
				var leadTasksCollection = GetLastLeadTasks();

				var leadTasks = new List<WorkItem>(leadTasksCollection.Count);
				var oldSecond = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.AllIterationPaths : m_config.AllAreaPaths;
				var newSecond = new List<string>();
				foreach (WorkItem leadTask in leadTasksCollection)
				{
					if (s_stateContainer.WorkMode != WorkMode.Query)
					{
						string second = s_stateContainer.GetParamForSecond(leadTask);
						if (!oldSecond.Contains(second) && !newSecond.Contains(second))
							newSecond.Add(second);
						if (!s_stateContainer.IsSecondFromStateContains(second))
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
					m_viewFiltersApplier = s_dataPresenter.PresentData(data, s_viewColumnsIndexes, scheduleDataGridView);
					usersVacationsComboBox.DataSource = m_viewFiltersApplier.Users;
					vacationsButton.Enabled = m_viewFiltersApplier.Users.Count > 0;
					var users2 = new List<string>(m_viewFiltersApplier.Users);
					users2.Insert(0, string.Empty);
					usersFilterСomboBox.DataSource = users2;

					if (!string.IsNullOrEmpty(currentUser) && m_viewFiltersApplier.Users.Contains(currentUser))
					{
						usersFilterСomboBox.SelectedItem = currentUser;
						m_viewFiltersApplier.FilterDataByUser(currentUser);
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
						m_viewFiltersApplier.FilterDataByLeadTaskMode(ltOnlyCheckBox.Checked);
					if (expandBlockersCheckBox.Checked)
						m_viewFiltersApplier.ExpandBlockers(expandBlockersCheckBox.Checked);
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
			m_viewFiltersApplier.FilterDataByDevCompleted(withDevCompleted);
		}

		private void LtOnlyCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			bool ltOnly = ltOnlyCheckBox.Checked;
			m_viewFiltersApplier.FilterDataByLeadTaskMode(ltOnly);
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
			m_viewFiltersApplier.ExpandBlockers(expandBlockers);
		}

		private void ExchangeButtonClick(object sender, EventArgs e)
		{
			s_stateContainer.WorkMode = s_stateContainer.WorkMode == WorkMode.AreaFirst ? WorkMode.IterationFirst : WorkMode.AreaFirst;
			m_config.WorkMode = s_stateContainer.WorkMode;
			m_config.ByArea = s_stateContainer.ByArea;

			ExchangeNames();
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

		private void ExchangeNames()
		{
			string tmp = firstGroupBox.Text;
			firstGroupBox.Text = secondGroupBox.Text;
			secondGroupBox.Text = tmp;
		}

		private void QueryTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			bool isQueryMode = queryTextBox.Text.Length > 0;
			ParamsGroupBox.Enabled = !isQueryMode;
			makeScheduleButton.Enabled = isQueryMode;
		}

		private void QueryTextBoxEnter(object sender, EventArgs e)
		{
			if (queryTextBox.ForeColor != Color.Gray)
				return;

			queryTextBox.ForeColor = Color.Black;
			queryTextBox.Text = string.Empty;
		}

		private void QueryTextBoxLeave(object sender, EventArgs e)
		{
			bool isQueryMode = queryTextBox.Text.Length > 0;
			s_stateContainer.WorkMode = isQueryMode
				? WorkMode.Query
				: (s_stateContainer.ByArea ? WorkMode.AreaFirst : WorkMode.IterationFirst);
			if (isQueryMode)
				return;

			queryTextBox.ForeColor = Color.Gray;
			queryTextBox.Text = Resources.QueryExample;
			InitFirst();
		}
	}
}
