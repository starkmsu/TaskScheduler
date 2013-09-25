using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Const;
using TfsUtils.Parsers;
using WorkItemType = TfsUtils.Const.WorkItemType;

namespace TaskPlanningForms
{
	internal class DataPresenter
	{
		private const double m_focusFactor = 0.5f;
		
		private readonly int m_maxInd = (int)DateTime.Now.AddMonths(1).Date.Subtract(DateTime.Now.Date).TotalDays;
		private const int m_leadTaskIdInd = 1;
		private const int m_assignedToInd = 4;
		private const string m_groupPrefix = "g ";
		private readonly int m_indShift;

		private List<DateTime> m_holidays;

		private Dictionary<string, List<DateTime>> m_vacations = new Dictionary<string, List<DateTime>>(0);

		internal DataPresenter(int indShift)
		{
			m_indShift = indShift;
		}

		internal void SetHolidays(List<DateTime> holidays)
		{
			m_holidays = holidays;
		}

		internal void SetVacations(List<VacationData> vacations)
		{
			m_vacations = new Dictionary<string, List<DateTime>>();
			foreach (VacationData vacation in vacations)
			{
				m_vacations.Add(vacation.User.Substring(0, 3), vacation.VacationDays);
			}
		}

		internal List<string> PresentData(DataContainer data, DataGridView dgv)
		{
			var alreadyAdded = new Dictionary<int, int>();
			var tasksByUser = new Dictionary<string, int>();

			DateTime today = DateTime.Now.Date;

			foreach (var leadTaskChildren in data.LeadTaskChildrenDict)
			{
				var leadTask = data.WiDict[leadTaskChildren.Key];

				int nextLtInd = AddLeadTaskRow(dgv, leadTask, data);
				int ltRowInd = dgv.Rows.Count - 1;

				var childrenTasks = leadTaskChildren.Value
					.Select(i => data.WiDict[i])
					.OrderBy(i => i.Priority())
					.ToList();

				if (childrenTasks.Count == 0)
					continue;

				int lastTaskInd = childrenTasks
					.Select(task =>
						AddTaskRow(
							dgv,
							task,
							childrenTasks,
							data,
							alreadyAdded,
							tasksByUser))
					.Max();
				for (int i = nextLtInd; i < lastTaskInd; i++)
				{
					DateTime date = today.AddDays(i - m_indShift);
					if (IsHoliday(date))
						continue;
					dgv.Rows[ltRowInd].Cells[i].SetErrorColor();
					dgv.Rows[ltRowInd].Cells[i].ToolTipText = Messages.ChildTaskHasLaterFd();
				}
				if (dgv.Rows[ltRowInd].Cells[0].IsColorForState(WorkItemState.Proposed))
				{
					int lasttChildRowInd = dgv.Rows.Count - 1;
					for (int i = ltRowInd + 1; i <= lasttChildRowInd; i++)
					{
						if (!dgv.Rows[i].Cells[0].IsColorForState(WorkItemState.Proposed))
						{
							dgv.Rows[ltRowInd].Cells[0].SetErrorColor();
							dgv.Rows[ltRowInd].Cells[0].ToolTipText = Messages.ProposedLeadTaskHasNotProposedChild();
						}
					}
				}
			}

			List<string> users = tasksByUser.Keys.ToList();
			users.Sort();

			return users;
		}

		internal void FilterDataByUser(DataGridView dgv, string user)
		{
			int prevLeadTaskRow = -1;
			bool hasUserTasks = false;
			for (int i = 0; i < dgv.Rows.Count; i++)
			{
				var row = dgv.Rows[i];
				if (row.Cells[m_leadTaskIdInd].Value != null)
				{
					if (prevLeadTaskRow >= 0)
						dgv.Rows[prevLeadTaskRow].Visible = user == string.Empty || hasUserTasks;
					hasUserTasks = false;
					prevLeadTaskRow = i;
					continue;
				}
				bool visible = user == string.Empty || row.Cells[m_assignedToInd].Value.ToString() == user;
				if (visible)
					hasUserTasks = true;
				row.Visible = visible;
			}
			if (prevLeadTaskRow >= 0)
				dgv.Rows[prevLeadTaskRow].Visible = hasUserTasks;
		}

		internal void FilterDataByDevCompleted(DataGridView dgv, bool withDevCompleted)
		{
			bool visible = true;
			for (int i = 0; i < dgv.Rows.Count; i++)
			{
				var row = dgv.Rows[i];
				if (row.Cells[m_leadTaskIdInd].Value != null)
					visible = withDevCompleted || !row.Cells[0].IsColorForState(WorkItemState.DevCompleted);
				row.Visible = visible;
			}
		}

		internal void FilterDataByLTMode(DataGridView dgv, bool ltOnly)
		{
			bool visible = true;
			for (int i = 0; i < dgv.Rows.Count; i++)
			{
				var row = dgv.Rows[i];
				if (row.Cells[m_leadTaskIdInd].Value != null)
					visible = row.Visible;
				else
					row.Visible = visible && !ltOnly;
			}
		}

		private int AddLeadTaskRow(
			DataGridView dgv,
			WorkItem leadTask,
			DataContainer data)
		{
			dgv.Rows.Add(new DataGridViewRow());
			var leadTaskRow = dgv.Rows[dgv.Rows.Count - 1];

			bool shouldCheckEstimate = FillLeadTaskStarttingCells(
				leadTask,
				leadTaskRow,
				data);

			if (leadTask.State == WorkItemState.Proposed)
				return AddDatesProposed(
					leadTask,
					leadTaskRow,
					m_indShift,
					"O",
					shouldCheckEstimate);
			return AddDatesActive(
				leadTask,
				leadTaskRow,
				m_indShift,
				"X");
		}

		private bool FillLeadTaskStarttingCells(
			WorkItem leadTask,
			DataGridViewRow leadTaskRow,
			DataContainer data)
		{
			leadTaskRow.Cells[0].Value = leadTask.Priority();
			leadTaskRow.Cells[0].SetColorByState(leadTask);
			leadTaskRow.Cells[0].ToolTipText = leadTask.IsDevCompleted() ? WorkItemState.DevCompleted : leadTask.State;

			leadTaskRow.Cells[1].Value = leadTask.Id;

			bool result = true;
			string visionAgreementState = leadTask.VisionAgreementState();
			string hlaAgeementState = leadTask.HlaAgreementState();
			leadTaskRow.Cells[1].ToolTipText = leadTask.IterationPath;
			if (visionAgreementState == DocumentAgreementState.No || visionAgreementState == DocumentAgreementState.Waiting)
			{
				leadTaskRow.Cells[1].SetErrorColor();
				leadTaskRow.Cells[1].ToolTipText += Environment.NewLine + Messages.BadVisionAgreemntState(visionAgreementState);
				result = false;
			}
			else if (hlaAgeementState == DocumentAgreementState.No || hlaAgeementState == DocumentAgreementState.Waiting)
			{
				leadTaskRow.Cells[1].SetErrorColor();
				leadTaskRow.Cells[1].ToolTipText += Environment.NewLine + Messages.BadHlaAgreemntState(hlaAgeementState);
				result = false;
			}
			else if (!data.LeadTaskChildrenDict.ContainsKey(leadTask.Id) || data.LeadTaskChildrenDict[leadTask.Id].Count == 0)
			{
				leadTaskRow.Cells[1].SetWarningColor();
				leadTaskRow.Cells[1].ToolTipText += Environment.NewLine + Messages.LTHasNoChildren();
			}
			else
			{
				leadTaskRow.Cells[1].SetColorByState(leadTask);
			}

			leadTaskRow.Cells[2].Value = leadTask.Title;
			leadTaskRow.Cells[2].Style.BackColor = leadTaskRow.Cells[0].Style.BackColor;

			if (data.BlockersDict.ContainsKey(leadTask.Id))
			{
				List<int> blockerIds = data.BlockersDict[leadTask.Id];
				string blockerIdsStr = string.Join(",", blockerIds);
				leadTaskRow.Cells[3].Value = blockerIdsStr;
				int nonChildBlockerId = blockerIds.FirstOrDefault(data.NonChildBlockers.Contains);
				if (nonChildBlockerId > 0)
				{
					leadTaskRow.Cells[3].SetErrorColor();
					leadTaskRow.Cells[3].ToolTipText = Messages.NonChildBlocker(nonChildBlockerId);
				}
				else
				{
					blockerIdsStr = string.Join(Environment.NewLine, blockerIds.Select(b => data.WiDict[b].Title));
					leadTaskRow.Cells[3].ToolTipText = blockerIdsStr;
				}
			}
			if (!string.IsNullOrEmpty(leadTask.BlockingReason()))
			{
				if (!string.IsNullOrEmpty(leadTaskRow.Cells[3].ToolTipText))
					leadTaskRow.Cells[3].ToolTipText += Environment.NewLine;
				else
					leadTaskRow.Cells[3].Value = leadTask.BlockingReason();
				leadTaskRow.Cells[3].ToolTipText += "Blocking Reason: " + leadTask.BlockingReason();
			}

			leadTaskRow.Cells[4].Value = leadTask.AssignedTo();

			return result;
		}

		private int AddTaskRow(
			DataGridView dgv,
			WorkItem task,
			List<WorkItem> childrenTasks,
			DataContainer data,
			Dictionary<int, int> alreadyAdded,
			Dictionary<string, int> tasksByUser)
		{
			if (alreadyAdded.ContainsKey(task.Id))
				return m_indShift;

			var nextInds = new List<int>();

			List<int> blockerIds = ProcessBlockers(
				dgv,
				data,
				task,
				childrenTasks,
				alreadyAdded,
				nextInds,
				tasksByUser);

			dgv.Rows.Add(new DataGridViewRow());
			
			var taskRow = dgv.Rows[dgv.Rows.Count - 1];

			string assignedTo = FillTaskStartingCells(
				task,
				taskRow,
				data,
				blockerIds);

			if (task.State == WorkItemState.Resolved)
			{
				alreadyAdded.Add(task.Id, m_indShift);
				return m_indShift;
			}

			if (!assignedTo.StartsWith(m_groupPrefix) && tasksByUser.ContainsKey(task.AssignedTo()))
				nextInds.Add(tasksByUser[assignedTo]);

			int maxNextInd = m_indShift;
			if (nextInds.Count > 0)
				maxNextInd = nextInds.Max();

			string userMark = assignedTo.Substring(0, 3);
			int nextInd = task.State == WorkItemState.Proposed
				? AddDatesProposed(
					task,
					taskRow,
					maxNextInd,
					userMark,
					true)
				: AddDatesActive(
					task,
					taskRow,
					maxNextInd,
					userMark);

			SetVacations(taskRow, userMark);

			alreadyAdded.Add(task.Id, nextInd);
			tasksByUser[assignedTo] = nextInd;
			return nextInd;
		}

		private List<int> ProcessBlockers(
			DataGridView dgv,
			DataContainer data,
			WorkItem task,
			List<WorkItem> childrenTasks,
			Dictionary<int, int> alreadyAdded,
			List<int> nextInds,
			Dictionary<string, int> tasksByUser)
		{
			if (!data.BlockersDict.ContainsKey(task.Id))
				return null;

			var blockerIds = data.BlockersDict[task.Id];
			var blokers = blockerIds
				.Where(b => data.WiDict.ContainsKey(b))
				.Select(b => data.WiDict[b])
				.OrderBy(b => b.Priority())
				.ToList();
			foreach (var blocker in blokers)
			{
				if (alreadyAdded.ContainsKey(blocker.Id))
				{
					nextInds.Add(alreadyAdded[blocker.Id]);
					continue;
				}

				var blockerSiblingTask = childrenTasks.FirstOrDefault(t => t.Id == blocker.Id);
				if (blockerSiblingTask != null)
				{
					int blockerNextInd = AddTaskRow(
						dgv,
						blockerSiblingTask,
						childrenTasks,
						data,
						alreadyAdded,
						tasksByUser);
					nextInds.Add(blockerNextInd);
				}
			}

			return blockerIds;
		}

		private string FillTaskStartingCells(
			WorkItem task,
			DataGridViewRow taskRow,
			DataContainer data,
			List<int> blockerIds)
		{
			taskRow.Cells[0].Value = task.Priority();
			taskRow.Cells[0].SetColorByState(task);
			taskRow.Cells[0].ToolTipText = task.State;

			taskRow.Cells[2].Value = task.Id + " " + task.Title;
			taskRow.Cells[2].ToolTipText =
				task.Discipline() + " "
				+ task.Title + " "
				+ (task.State == WorkItemState.Active
					? "Remaining " + task.Remaining().ToString()
					: "Estimate " + task.Estimate().ToString());
			taskRow.Cells[2].Style.BackColor = taskRow.Cells[0].Style.BackColor;

			if (blockerIds != null)
			{
				string blockerIdsStr = string.Join(",", blockerIds);
				taskRow.Cells[3].Value = blockerIdsStr;
				int nonChildBlockerId = blockerIds.FirstOrDefault(data.NonChildBlockers.Contains);
				if (nonChildBlockerId > 0)
				{
					taskRow.Cells[3].SetErrorColor();
					taskRow.Cells[3].ToolTipText = Messages.NonChildBlocker(nonChildBlockerId);
				}
				else if (task.State == WorkItemState.Active)
				{
					taskRow.Cells[3].SetErrorColor();
					taskRow.Cells[3].ToolTipText = Messages.ActiveIsBlocked(blockerIdsStr);
				}
				else
				{
					blockerIdsStr = string.Join(Environment.NewLine, blockerIds.Select(b => data.WiDict[b].Title));
					taskRow.Cells[3].ToolTipText = blockerIdsStr;
				}
			}
			if (!string.IsNullOrEmpty(task.BlockingReason()))
			{
				if (!string.IsNullOrEmpty(taskRow.Cells[3].ToolTipText))
					taskRow.Cells[3].ToolTipText += Environment.NewLine;
				else
					taskRow.Cells[3].Value = task.BlockingReason();
				taskRow.Cells[3].ToolTipText += "Blocking Reason: " + task.BlockingReason();
			}

			string assignedTo = task.AssignedTo();
			taskRow.Cells[4].Value = assignedTo;
			if (assignedTo.StartsWith(m_groupPrefix))
			{
				taskRow.Cells[4].SetWarningColor();
				taskRow.Cells[4].ToolTipText = Messages.TaskIsNotAssigned();
			}

			return assignedTo;
		}

		private int AddDatesActive(
			WorkItem workItem,
			DataGridViewRow row,
			int startInd,
			string userMark)
		{
			var taskStart = workItem.StartDate();
			var taskFinish = workItem.FinishDate();
			if (taskFinish == null)
				return m_indShift;

			if (taskFinish.Value.Date < DateTime.Now.Date)
			{
				row.Cells[m_indShift - 1].Value = taskFinish.Value.ToString("dd.MM");
				if (workItem.Type.Name == WorkItemType.LeadTask)
					row.Cells[m_indShift - 1].SetErrorColor();
				else
					row.Cells[m_indShift - 1].SetWarningColor();
				row.Cells[m_indShift - 1].ToolTipText = Messages.ExpiredFd();

				double? remaining = workItem.Remaining();
				if (remaining.HasValue)
				{
					var length = (int)Math.Ceiling(remaining.Value / 8 / m_focusFactor);
					return AddDates(
						row,
						startInd,
						length,
						userMark);
				}
			}
			else if (taskStart.HasValue)
			{
				var indStart = (int)taskStart.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				if (indStart < 0)
					row.Cells[m_indShift - 1].Value = taskStart.Value.ToString("dd.MM");
				indStart = Math.Min(Math.Max(0, indStart), m_maxInd) + m_indShift;

				var indFinish = (int)taskFinish.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				indFinish = Math.Min(Math.Max(0, indFinish), m_maxInd) + m_indShift;
				DateTime today = DateTime.Now.Date;
				for (int i = indStart; i <= indFinish; i++)
				{
					DateTime date = today.AddDays(i - m_indShift);
					if (IsHoliday(date))
						continue;
					if (IsVacation(date, userMark))
					{
						row.Cells[i].Style.BackColor = CellsPalette.WeekEnd;
						continue;
					}
					row.Cells[i].Value = userMark;
				}
				return indFinish + 1;
			}
			return m_indShift;
		}

		private int AddDatesProposed(
			WorkItem task,
			DataGridViewRow taskRow,
			int startInd,
			string userMark,
			bool shouldCheckEstimate)
		{
			double? estimate = task.Estimate();
			if (estimate == null)
			{
				if (shouldCheckEstimate)
				{
					taskRow.Cells[m_indShift - 1].SetErrorColor();
					taskRow.Cells[m_indShift - 1].ToolTipText = Messages.NoEstimate();
				}
				return m_indShift;
			}

			var length = (int)Math.Ceiling(estimate.Value / 8 / m_focusFactor);

			if (task.FinishDate() != null)
			{
				int finishShift = length - 1;
				DateTime startDate = task.FinishDate().Value.Date;
				DateTime today = DateTime.Now.Date;
				while (finishShift > 0 && startDate >= today)
				{
					startDate = startDate.AddDays(-1);
					if (!IsHoliday(startDate))
						--finishShift;
				}
				var startShift = (int)startDate.Subtract(DateTime.Now.Date).TotalDays;
				startInd = Math.Max(startInd, startShift + m_indShift);
			}

			return AddDates(
				taskRow,
				startInd,
				length,
				userMark);
		}

		private int AddDates(
			DataGridViewRow row,
			int startInd,
			int length,
			string userMark)
		{
			if (startInd - m_indShift > m_maxInd)
				return m_indShift + m_maxInd + 1;
			int ind = 0;
			while (length > 0)
			{
				var date = DateTime.Now.AddDays(startInd - m_indShift + ind);
				if (IsHoliday(date))
				{
					++ind;
					continue;
				}
				if (startInd - m_indShift + ind > m_maxInd)
					return m_indShift + m_maxInd + 1;
				var cell = row.Cells[startInd + ind];
				if (IsVacation(date, userMark))
				{
					++ind;
					cell.Style.BackColor = CellsPalette.WeekEnd;
					continue;
				}
				if (cell.Value == null)
					cell.Value = userMark;
				else
					cell.Value = cell.Value + userMark;
				++ind;
				--length;
			}
			return startInd + ind;
		}

		private void SetVacations(DataGridViewRow row, string user)
		{
			if (!m_vacations.ContainsKey(user) || m_vacations[user].Count == 0)
				return;
			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			var vacationsDays = m_vacations[user];
			if (finish < vacationsDays[0])
				return;
			if (start > vacationsDays[vacationsDays.Count-1])
				return;
			int ind = m_indShift;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				if (vacationsDays.Any(d => d == i))
					row.Cells[ind].Style.BackColor = CellsPalette.WeekEnd;
				++ind;
			}
		}

		private bool IsHoliday(DateTime dateTime)
		{
			var date = dateTime.Date;
			var dayOfWeek = date.DayOfWeek;
			if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
				return true;
			if (m_holidays == null)
				return false;
			return m_holidays.Contains(date);
		}

		private bool IsVacation(DateTime dateTime, string user)
		{
			if (!m_vacations.ContainsKey(user) || m_vacations[user].Count == 0)
				return false;
			var date = dateTime.Date;
			return m_vacations[user].Any(v => v == date);
		}
	}
}
