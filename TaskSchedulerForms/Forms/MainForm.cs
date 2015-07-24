using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Config;
using TaskSchedulerForms.Const;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TaskSchedulerForms.Presentation;
using TaskSchedulerForms.Properties;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Forms
{
	public partial class MainForm : Form
	{
		private readonly Config.Config m_config;

		private static readonly DataLoader s_dataLoader = new DataLoader();
		private static readonly DataFiller s_dataFiller = new DataFiller();
		private static readonly DataPresenter s_dataPresenter = new DataPresenter();
		private static readonly StateContainer s_stateContainer = new StateContainer();
		private static readonly FreeDaysCalculator s_freeDaysCalculator = new FreeDaysCalculator();
		private static readonly FocusFactorCalculator s_focusFactorCalculator = new FocusFactorCalculator();

		private static ViewColumnsIndexes s_viewColumnsIndexes;
		private static ViewColumnsIndexes s_planColumnsIndexes;

		private List<WorkItem> m_leadTasks;
		private List<DateTime> m_holidays;

		private ViewFiltersApplier m_viewFiltersApplier;
		private ViewFiltersApplier m_planFiltersApplier;

		private readonly List<ToolStripMenuItem> m_viewMenuItems;

		private DataContainer m_lastProcessedData;

		public MainForm()
		{
			InitializeComponent();

			m_config = ConfigManager.LoadConfig();

			s_viewColumnsIndexes = new ViewColumnsIndexes(scheduleDataGridView);
			s_planColumnsIndexes = new ViewColumnsIndexes(planningDataGridView);
			ScheduleColumnsPresenter.InitColumns(
				scheduleDataGridView,
				s_viewColumnsIndexes.FirstDateColumnIndex,
				1);
			ScheduleColumnsPresenter.InitColumns(
				planningDataGridView,
				s_planColumnsIndexes.FirstDateColumnIndex,
				3);

			m_holidays = m_config.Holidays;
			s_freeDaysCalculator.SetHolidays(m_holidays);

			UpdateHolidays();

			if (m_config.Vacations.Count > 0)
			{
				vacationsButton.Enabled = true;
				var vacationsUsers = m_config.Vacations.Select(v => v.User).ToList();
				vacationsUsers.Sort();
				usersVacationsComboBox.DataSource = vacationsUsers;
				s_freeDaysCalculator.SetVacations(m_config.Vacations);
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

			m_viewMenuItems = new List<ToolStripMenuItem>
			{
				toggleIterationToolStripMenuItem,
				toggleSprintToolStripMenuItem,
				toggleDevCompletedToolStripMenuItem,
				toggleLTOnlyToolStripMenuItem,
				toggleBlockersToolStripMenuItem,
			};
		}

		private void InitFirst()
		{
			subTreesCheckBox.Checked = m_config.WithSubAreaPaths;
			List<string> firstList = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.AreaPathsByArea : m_config.IterationPathsByIteration;
			if (firstList != null && firstList.Count > 0)
			{
				var list = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.AllAreaPaths : m_config.AllIterationPaths;
				if (!list.Contains(firstList[0]))
					list.Add(firstList[0]);
				firstComboBox.DataSource = list;
				firstComboBox.Text = firstList[0];
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
					? CellsPalette.FreeDay
					: scheduleDataGridView.Columns[1].HeaderCell.Style.BackColor;
				column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = color;
			}
		}

		private void LoadLeadTasksButtonClick(object sender, EventArgs e)
		{
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
					subTreesCheckBox.Checked,
					sprintCheckBox.Checked);
			}
			catch (Exception e)
			{
				HandleException(
					e,
					secondComboBox,
					Resources.LeadTasksFetchingError);
				secondComboBox.Invoke(
					new Action(() =>
					{
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
			var oldSecond = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.LastIterationPaths : m_config.LastAreaPaths;
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
			var configSecond = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.IterationPathsByArea : m_config.AreaPathsByIteration;
			if (configSecond != null && configSecond.Count > 0)
				validSecond = configSecond.Where(secondList.Contains).ToList();
			validSecond.Sort();

			var selectedSecond = secondListBox.Items
				.Cast<string>()
				.Where(i => secondList.Contains(i))
				.ToList();

			validSecond.ForEach(i =>
			{
				if (!selectedSecond.Contains(i))
					selectedSecond.Add(i);
			});

			secondList = secondList
				.Where(i => selectedSecond.All(j => j != i))
				.ToList();

			var sencondArray = selectedSecond
				.Select(i => (object) i)
				.ToArray();

			secondComboBox.Invoke(new Action(() =>
			{
				secondComboBox.DataSource = secondList;
				secondComboBox.Enabled = true;
				secondAddButton.Enabled = secondList.Count > 0;

				secondListBox.Items.Clear();
				secondListBox.Items.AddRange(sencondArray);

				subTreesCheckBox.Enabled = true;
				loadLeadTasksButton.Enabled = true;
				makeScheduleButton.Enabled = secondListBox.Items.Count > 0;
			}));
		}

		private void MakeScheduleButtonClick(object sender, EventArgs e)
		{
			makeScheduleButton.Enabled = false;
			loadLeadTasksButton.Enabled = false;
			queryTextBox.Enabled = false;
			holidaysButton.Enabled = false;

			usersLabel.Enabled = false;
			usersFilterСomboBox.Enabled = false;
			sprintLabel.Enabled = false;
			sprintFilterComboBox.Enabled = false;
			mainTabControl.SelectTab(mainTabPage);
			scheduleDataGridView.Rows.Clear();
			refreshButton.Enabled = false;
			refreshButton.BackColor = Color.Transparent;

			Text = GetFirstShortName();

			ThreadPool.QueueUserWorkItem(x => ProcessData());
		}

		private string GetFirstShortName()
		{
			string first = firstListBox.Items[0].ToString();
			var parts = first.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
			return parts[parts.Length - 1];
		}

		private List<WorkItem> GetLeadTasks()
		{
			List<WorkItem> result;
			if (s_stateContainer.WorkMode != WorkMode.Query)
			{
				s_stateContainer.LastWithSubTree = subTreesCheckBox.Checked;
				s_stateContainer.LastWithSprint = sprintCheckBox.Checked;

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
					string first = s_stateContainer.GetParamForFirst(leadTask);
					string second = s_stateContainer.GetParamForSecond(leadTask);
					if (firstList.All(f => first.IndexOf(first, StringComparison.Ordinal) == -1)
						|| secondList.All(s => second.IndexOf(s, StringComparison.Ordinal) == -1))
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

				m_lastProcessedData = s_dataFiller.ProcessLeadTasks(s_stateContainer.LastTfsUrl, leadTasks);
			}
			catch (Exception exc)
			{
				HandleException(
					exc,
					scheduleDataGridView,
					Resources.LeadTasksParsingError);
				scheduleDataGridView.Invoke(
					new Action(() =>
					{
						makeScheduleButton.Enabled = true;
						loadLeadTasksButton.Enabled = true;
						queryTextBox.Enabled = true;
					}));
				return;
			}

			scheduleDataGridView.Invoke(new Action(() =>
				{
					try
					{
						m_viewFiltersApplier = s_dataPresenter.PresentData(
							m_lastProcessedData,
							null,
							null,
							s_viewColumnsIndexes,
							s_freeDaysCalculator,
							s_focusFactorCalculator,
							scheduleDataGridView);
					}
					catch (Exception exc)
					{
						HandleException(
							exc,
							scheduleDataGridView,
							Resources.LeadTasksParsingError);
						scheduleDataGridView.Invoke(new Action(() =>
						{
							makeScheduleButton.Enabled = true;
							loadLeadTasksButton.Enabled = true;
							queryTextBox.Enabled = true;
						}));
						return;
					}
					usersVacationsComboBox.DataSource = m_viewFiltersApplier.Users.Where(u => !u.IsUnassigned()).ToList();
					vacationsButton.Enabled = m_viewFiltersApplier.Users.Count > 0;
					var users = new List<string>(m_viewFiltersApplier.Users);
					users.Insert(0, string.Empty);
					usersFilterСomboBox.DataSource = users;
					usersFilterСomboBox.Enabled = true;
					usersLabel.Enabled = true;
					var sprints = new List<string>(m_viewFiltersApplier.Sprints);
					sprints.Insert(0, string.Empty);
					sprintFilterComboBox.DataSource = sprints;
					sprintFilterComboBox.Enabled = true;
					sprintLabel.Enabled = true;
					makeScheduleButton.Enabled = true;
					loadLeadTasksButton.Enabled = true;
					holidaysButton.Enabled = true;
					queryTextBox.Enabled = true;
					refreshButton.Enabled = true;
					planButton.Enabled = true;
					mainTabControl.SelectTab(dataTabPage);
				}));
		}

		private void UsersFilterСomboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			if (sender == usersFilterСomboBox)
			{
				string user = usersFilterСomboBox.SelectedItem.ToString();
				m_viewFiltersApplier.FilterDataByUser(user);
			}
			else
			{
				string user = usersFilterComboBox2.SelectedItem.ToString();
				m_planFiltersApplier.FilterDataByUser(user);
			}
		}

		private void SprintFilterComboBoxSelectionChangeCommitted(object sender, EventArgs e)
		{
			string sprint = sprintFilterComboBox.SelectedItem.ToString();
			m_viewFiltersApplier.FilterDataBySprint(sprint);
		}

		private void RefreshButtonClick(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(x => RefreshData());
		}

		private List<WorkItem> GetLastLeadTasks()
		{

			List<WorkItem> items;
			if (s_stateContainer.WorkMode != WorkMode.Query)
				items = s_dataLoader.GetLeadTasks(
					s_stateContainer.LastTfsUrl,
					s_stateContainer.WorkMode == WorkMode.AreaFirst,
					s_stateContainer.GetFirstList(),
					s_stateContainer.LastWithSubTree,
					s_stateContainer.LastWithSprint);
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
			string currentSprint = null;
			refreshButton.Invoke(new Action(() =>
				{
					loadLeadTasksButton.Enabled = false;
					makeScheduleButton.Enabled = false;
					refreshButton.Enabled = false;
					if (usersFilterСomboBox.SelectedItem != null)
						currentUser = usersFilterСomboBox.SelectedItem.ToString();
					if (sprintFilterComboBox.SelectedItem != null)
						currentSprint = sprintFilterComboBox.SelectedItem.ToString();
					usersLabel.Enabled = false;
					usersFilterСomboBox.Enabled = false;
					scheduleDataGridView.Rows.Clear();

					Text = GetFirstShortName();
				}));
			try
			{
				var leadTasksCollection = GetLastLeadTasks();

				var leadTasks = new List<WorkItem>(leadTasksCollection.Count);
				var oldSecond = s_stateContainer.WorkMode == WorkMode.AreaFirst ? m_config.LastIterationPaths : m_config.LastAreaPaths;
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
				m_lastProcessedData = s_dataFiller.ProcessLeadTasks(tfsUrlTextBox.Text, leadTasks);

				scheduleDataGridView.Invoke(new Action(() =>
				{
					bool isDateChanged = scheduleDataGridView.Columns[s_viewColumnsIndexes.FirstDateColumnIndex].HeaderText != DateTime.Now.ToString("dd.MM");
					if (isDateChanged)
						ScheduleColumnsPresenter.InitColumns(
							scheduleDataGridView,
							s_viewColumnsIndexes.FirstDateColumnIndex,
							1);
					m_viewFiltersApplier = s_dataPresenter.PresentData(
						m_lastProcessedData,
						null,
						null,
						s_viewColumnsIndexes,
						s_freeDaysCalculator,
						s_focusFactorCalculator,
						scheduleDataGridView);
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

					if (!string.IsNullOrEmpty(currentSprint) && m_viewFiltersApplier.Sprints.Contains(currentSprint))
					{
						sprintFilterComboBox.SelectedItem = currentSprint;
						m_viewFiltersApplier.FilterDataBySprint(currentSprint);
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
				}));
			}
			catch (Exception exc)
			{
				HandleException(
					exc,
					scheduleDataGridView,
					Resources.LeadTasksParsingError);
				scheduleDataGridView.Invoke(
					new Action(() =>
					{
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
			s_freeDaysCalculator.SetHolidays(m_holidays);
			UpdateHolidays();
		}

		private void FirstAddButtonClick(object sender, EventArgs e)
		{
			string first = firstComboBox.Text;
			List<string> list = s_stateContainer.WorkMode == WorkMode.AreaFirst
				? m_config.AllAreaPaths
				: m_config.AllIterationPaths;
			if (!list.Contains(first))
			{
				list.Add(first);
				firstComboBox.DataSource = list;
			}
			if (firstListBox.Items.Contains(first))
				return;
			firstListBox.Items.Add(first);
			loadLeadTasksButton.Enabled = true;
			makeScheduleButton.Enabled = false;
		}

		private void FirstRemoveButtonClick(object sender, EventArgs e)
		{
			firstListBox.Items.Remove(firstListBox.SelectedItem);
			firstRemoveButton.Enabled = false;
			if (firstListBox.Items.Count > 0)
				return;
			loadLeadTasksButton.Enabled = false;
			makeScheduleButton.Enabled = false;
		}

		private void SecondAddButtonClick(object sender, EventArgs e)
		{
			string second = secondComboBox.Text;
			if (string.IsNullOrEmpty(second)
				|| secondListBox.Items.Contains(second))
				return;
			var newList = secondComboBox.Items
				.Cast<string>()
				.Where(secondOption => second != secondOption)
				.ToList();
			secondComboBox.DataSource = newList;
			if (newList.Count == 0)
			{
				secondComboBox.SelectedItem = null;
				secondComboBox.Text = string.Empty;
				secondAddButton.Enabled = false;
			}
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
		}

		private void SecondRemoveButtonClick(object sender, EventArgs e)
		{
			var toRemove = secondListBox.SelectedItem;
			if (toRemove == null)
			{
				secondRemoveButton.Enabled = false;
				return;
			}
			secondListBox.Items.Remove(toRemove);
			var newList = secondComboBox.Items
				.Cast<string>()
				.ToList();
			newList.Add(toRemove.ToString());
			newList.Sort();
			secondComboBox.DataSource = newList;
			if (newList.Count == 1)
				secondAddButton.Enabled = true;
			secondRemoveButton.Enabled = false;
			if (secondListBox.Items.Count > 0)
				return;
			makeScheduleButton.Enabled = false;
		}

		private void ToggleDevCompletedToolStripMenuItem1Click(object sender, EventArgs e)
		{
			if (m_viewMenuItems.Contains(sender))
				m_viewFiltersApplier.ToggleDevCompletedMode();
			else
				m_planFiltersApplier.ToggleDevCompletedMode();
		}

		private void ToggleLtOnlyToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (m_viewMenuItems.Contains(sender))
				m_viewFiltersApplier.ToggleLeadTaskMode();
			else
				m_planFiltersApplier.ToggleLeadTaskMode();
		}

		private void ToggleBlockersToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (m_viewMenuItems.Contains(sender))
				m_viewFiltersApplier.ToggleBlockers();
			else
				m_planFiltersApplier.ToggleBlockers();
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
			s_freeDaysCalculator.SetVacations(m_config.Vacations);
		}

		private void ExchangeButtonClick(object sender, EventArgs e)
		{
			s_stateContainer.WorkMode = s_stateContainer.WorkMode == WorkMode.AreaFirst ? WorkMode.IterationFirst : WorkMode.AreaFirst;
			m_config.WorkMode = s_stateContainer.WorkMode;
			m_config.ByArea = s_stateContainer.ByArea;

			ExchangeNames();
			var itemsCopy = new object[firstListBox.Items.Count];
			firstComboBox.Text = secondListBox.Items.Count > 0
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

		private void ToggleIterationToolStripMenuItemClick(object sender, EventArgs e)
		{
			s_dataPresenter.ToggleIteration(
				m_viewMenuItems.Contains(sender) ? scheduleDataGridView : planningDataGridView,
				s_viewColumnsIndexes);
		}

		private void ToggleSprintToolStripMenuItemClick(object sender, EventArgs e)
		{
			s_dataPresenter.ToggleSprint(
				m_viewMenuItems.Contains(sender) ? scheduleDataGridView : planningDataGridView,
				s_viewColumnsIndexes);
		}

		private void AppendExceptionString(Exception exc, StringBuilder stringBuilder)
		{
			if (exc.InnerException != null)
				AppendExceptionString(exc.InnerException, stringBuilder);
			stringBuilder.AppendLine(exc.Message);
			stringBuilder.AppendLine(exc.StackTrace);
		}

		private void HandleException(
			Exception exc,
			Control control,
			string caption)
		{
			var strBuilder = new StringBuilder();
			AppendExceptionString(exc, strBuilder);
			string text = strBuilder.ToString();
			using (var fileWriter = new StreamWriter(DateTime.Now.ToString("yyyy-mm-dd HH-mm-ss") + ".txt", false))
			{
				fileWriter.WriteLine(text);
			}
			control.Invoke(new Action(() => MessageBox.Show(text, caption)));
		}

		private void PlanButtonClick(object sender, EventArgs e)
		{
			ThreadPool.QueueUserWorkItem(x => MakePlan());
		}

		private void MakePlan()
		{
			var planningAssignments = GetPlanningAssignments();

			var allUsers = usersFilterСomboBox.Items.Cast<string>().Where(i => !string.IsNullOrEmpty(i)).ToList();
			if (planningAssignments != null)
			{
				allUsers.AddRange(
					planningAssignments.Values
						.Distinct()
						.Where(i => !allUsers.Contains(i)));
				allUsers.Sort();
			}

			string currentUser = null;
			planButton.Invoke(new Action(() =>
			{
				planButton.Enabled = false;
				if (usersFilterComboBox2.SelectedItem != null)
					currentUser = usersFilterComboBox2.SelectedItem.ToString();
				usersLabel2.Enabled = false;
				usersFilterComboBox2.Enabled = false;
				planningDataGridView.Rows.Clear();
			}));
			try
			{
				planningDataGridView.Invoke(new Action(() =>
				{
					bool isDateChanged = planningDataGridView.Columns[s_viewColumnsIndexes.FirstDateColumnIndex].HeaderText != DateTime.Now.ToString("dd.MM");
					if (isDateChanged)
						ScheduleColumnsPresenter.InitColumns(
							planningDataGridView,
							s_planColumnsIndexes.FirstDateColumnIndex,
							3);

					var comboBoxColumn = planningDataGridView.Columns[s_planColumnsIndexes.AssignedToColumnIndex] as DataGridViewComboBoxColumn;
					comboBoxColumn.DataSource = allUsers;
					m_planFiltersApplier = s_dataPresenter.PresentData(
						m_lastProcessedData,
						planningAssignments,
						allUsers,
						s_viewColumnsIndexes,
						s_freeDaysCalculator,
						s_focusFactorCalculator,
						planningDataGridView);

					usersVacationsComboBox.DataSource = m_planFiltersApplier.Users;
					vacationsButton.Enabled = m_planFiltersApplier.Users.Count > 0;

					var users2 = new List<string>(m_planFiltersApplier.Users);
					users2.Insert(0, string.Empty);
					usersFilterComboBox2.DataSource = users2;

					if (!string.IsNullOrEmpty(currentUser) && m_planFiltersApplier.Users.Contains(currentUser))
					{
						usersFilterComboBox2.SelectedItem = currentUser;
						m_planFiltersApplier.FilterDataByUser(currentUser);
					}

					usersFilterComboBox2.Enabled = true;
					usersLabel2.Enabled = true;
					planButton.Enabled = true;
					autoplanButton.Enabled = true;
				}));
			}
			catch (Exception exc)
			{
				HandleException(
					exc,
					planningDataGridView,
					Resources.LeadTasksParsingError);
				planningDataGridView.Invoke(new Action(() => { planButton.Enabled = true; }));
			}
		}

		private Dictionary<int, string> GetPlanningAssignments()
		{
			if (planningDataGridView.Rows.Count == 0)
				return null;
			var result = new Dictionary<int, string>(planningDataGridView.Rows.Count);
			int idInd = s_planColumnsIndexes.IdColumnIndex;
			int assignInd = s_planColumnsIndexes.AssignedToColumnIndex;
			for (int i = 0; i < planningDataGridView.Rows.Count; i++)
			{
				var row = planningDataGridView.Rows[i];
				string assignee = row.Cells[assignInd].Value.ToString();
				if (!row.Cells[idInd].IsUncolored())
					continue;
				int id;
				bool isInt = int.TryParse(row.Cells[idInd].Value.ToString(), out id);
				if (!isInt)
					continue;
				result.Add(id, assignee);
			}
			return result;
		}

		private void AddUserButtonClick(object sender, EventArgs e)
		{
			string newUser = addUserTextBox.Text;

			AddUsers(new []{newUser});
		}

		private void AddUsers(IEnumerable<string> newUsers)
		{
			var planningUsers = usersFilterComboBox2.Items
				.Cast<string>()
				.Where(i => !string.IsNullOrEmpty(i))
				.ToList();

			foreach (string newUser in newUsers)
			{
				if (planningUsers.Contains(newUser))
					continue;
				planningUsers.Add(newUser);
				planningUsers.Sort();
			}
			
			var comboBoxColumn = planningDataGridView.Columns[s_planColumnsIndexes.AssignedToColumnIndex] as DataGridViewComboBoxColumn;
			if (comboBoxColumn != null)
				comboBoxColumn.DataSource = planningUsers;
			var usersToFilter = new List<string>(planningUsers);
			usersToFilter.Insert(0, string.Empty);
			usersFilterComboBox2.DataSource = usersToFilter;
		}

		private void AddUserTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			addUserButton.Enabled = addUserTextBox.Text.Length > 0;
		}

		private void AutoPlanButtonClick(object sender, EventArgs e)
		{
			var planningAssignments = new List<Tuple<int, string>>(planningDataGridView.Rows.Count);
			var disciplineUsers = new Dictionary<string, HashSet<string>>();
			var rowsToPlanDict = new Dictionary<int, DataGridViewRow>(planningDataGridView.Rows.Count);
			int idInd = s_planColumnsIndexes.IdColumnIndex;
			int assignedIndex = s_planColumnsIndexes.AssignedToColumnIndex;
			for (int i = 0; i < planningDataGridView.Rows.Count; i++)
			{
				var planRow = planningDataGridView.Rows[i];
				int taskId;
				Boolean isInt = int.TryParse(planRow.Cells[idInd].Value.ToString(), out taskId);
				if (!isInt)
					continue;
				var task = m_lastProcessedData.WiDict[taskId];
				if (task.Type.Name != TfsUtils.Const.WorkItemType.Task)
					continue;
				rowsToPlanDict.Add(taskId, planRow);
				string discipline = task.Discipline();
				string user = planRow.Cells[assignedIndex].Value.ToString();
				planningAssignments.Add(new Tuple<int, string>(taskId, user));
				if (user.IsUnassigned())
					continue;
				if (!disciplineUsers.ContainsKey(discipline))
					disciplineUsers.Add(discipline, new HashSet<string>());
				disciplineUsers[discipline].Add(user);
			}

			var autoPlanForm = new AutoPlanForm(disciplineUsers);
			var dialogResult = autoPlanForm.ShowDialog();
			if (dialogResult != DialogResult.OK)
				return;

			var userToPlanByDiscipline = autoPlanForm.UsersByDiscipline;

			var usersToPlan = userToPlanByDiscipline.Values.SelectMany(i => i).Distinct();
			AddUsers(usersToPlan);

			var autoPlan = AutoPlanMaker.Make(
				userToPlanByDiscipline,
				planningAssignments,
				m_lastProcessedData);

			foreach (var planPair in autoPlan)
			{
				rowsToPlanDict[planPair.Key].Cells[assignedIndex].Value = planPair.Value;
			}
		}

		private void SecondListBoxSelectedValueChanged(object sender, EventArgs e)
		{
			if (secondListBox.SelectedItem != null
				&& !secondRemoveButton.Enabled)
				secondRemoveButton.Enabled = true;
		}

		private void FirstListBoxSelectedValueChanged(object sender, EventArgs e)
		{
			if (firstListBox.SelectedItem != null
				&& !firstRemoveButton.Enabled)
				firstRemoveButton.Enabled = true;
		}
	}
}
