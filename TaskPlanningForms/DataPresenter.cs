using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Const;
using TfsUtils.Parsers;

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

		internal List<DateTime> Holidays { get; set; }

		internal DataPresenter(int indShift)
		{
			m_indShift = indShift;
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
			}

			List<string> users = tasksByUser.Keys.ToList();
			users.Sort();
			users.Insert(0, string.Empty);

			return users;
		}

		internal void FilterDataByUser(string user, DataGridView dgv)
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

		private int AddLeadTaskRow(
			DataGridView dgv,
			WorkItem leadTask,
			DataContainer data)
		{
			dgv.Rows.Add(new DataGridViewRow());
			var leadTaskRow = dgv.Rows[dgv.Rows.Count - 1];

			leadTaskRow.Cells[0].Value = leadTask.Priority();
			leadTaskRow.Cells[0].SetColorByState(leadTask);
			leadTaskRow.Cells[1].Value = leadTask.Id;
			leadTaskRow.Cells[1].ToolTipText = leadTask.IterationPath;
			leadTaskRow.Cells[1].SetColorByState(leadTask);
			leadTaskRow.Cells[2].Value = leadTask.Title;
			leadTaskRow.Cells[2].SetColorByState(leadTask);
			if (data.BlockersDict.ContainsKey(leadTask.Id))
			{
				List<int> blockerIds = data.BlockersDict[leadTask.Id];
				string blockerIdsStr = string.Join(",", blockerIds);
				leadTaskRow.Cells[3].Value = blockerIdsStr;
				int nonChildBlockerId = blockerIds.FirstOrDefault(data.NonChildBlockers.Contains);
				if (nonChildBlockerId > 0)
				{
					leadTaskRow.Cells[3].SetErrorColor();
					leadTaskRow.Cells[3].ToolTipText = nonChildBlockerId + " - " + Messages.NonChildBlocker();
				}
			}
			leadTaskRow.Cells[4].Value = leadTask.AssignedTo();

			if (leadTask.State == WorkItemState.Proposed)
				return AddDatesProposed(leadTask, leadTaskRow, m_indShift, "O");
			return AddDatesActive(leadTask, leadTaskRow, m_indShift, "X");
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
				blockerIds,
				data.NonChildBlockers);

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
				? AddDatesProposed(task, taskRow, maxNextInd, userMark)
				: AddDatesActive(task, taskRow, maxNextInd, userMark);

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
			List<int> blockerIds,
			HashSet<int> nonChildBlockerIds)
		{
			taskRow.Cells[0].Value = task.Priority();
			taskRow.Cells[0].SetColorByState(task);
			taskRow.Cells[2].Value = task.Id;
			taskRow.Cells[2].ToolTipText =
				task.Discipline() + " "
				+ task.Title + " "
				+ (task.State == WorkItemState.Active
					? "Remaining " + task.Remaining().ToString()
					: "Estimate " + task.Estimate().ToString());
			taskRow.Cells[2].SetColorByDiscipline(task);
			if (blockerIds != null)
			{
				string blockerIdsStr = string.Join(",", blockerIds);
				taskRow.Cells[3].Value = blockerIdsStr;
				int nonChildBlockerId = blockerIds.FirstOrDefault(nonChildBlockerIds.Contains);
				if (nonChildBlockerId > 0)
				{
					taskRow.Cells[3].SetErrorColor();
					taskRow.Cells[3].ToolTipText = nonChildBlockerId + " - " + Messages.NonChildBlocker();
				}
				else if (task.State == WorkItemState.Active)
				{
					taskRow.Cells[3].SetErrorColor();
					taskRow.Cells[3].ToolTipText = blockerIdsStr + " - " + Messages.ActiveIsBlocked();
				}
			}
			string assignedTo = task.AssignedTo();
			taskRow.Cells[4].Value = assignedTo;
			if (assignedTo.StartsWith(m_groupPrefix))
			{
				taskRow.Cells[4].SetErrorColor();
				taskRow.Cells[4].ToolTipText = Messages.TaskIsNotAssigned();
			}
			return assignedTo;
		}

		private int AddDatesActive(
			WorkItem task,
			DataGridViewRow taskRow,
			int startInd,
			string userMark)
		{
			var taskStart = task.StartDate();
			var taskFinish = task.FinishDate();
			if (taskFinish == null)
				return m_indShift;

			if (taskFinish.Value.Date < DateTime.Now.Date)
			{
				taskRow.Cells[m_indShift - 1].Value = taskFinish.Value.ToString("dd.MM");
				taskRow.Cells[m_indShift - 1].SetErrorColor();
				taskRow.Cells[m_indShift - 1].ToolTipText = Messages.ExpiredFd();

				double? remaining = task.Remaining();
				if (remaining.HasValue)
					return AddDates(
						taskRow,
						remaining.Value,
						startInd,
						userMark);
			}
			else if (taskStart.HasValue)
			{
				var indStart = (int)taskStart.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				if (indStart < 0)
					taskRow.Cells[m_indShift - 1].Value = taskStart.Value.ToString("dd.MM");
				indStart = Math.Min(Math.Max(0, indStart) + m_indShift, m_maxInd);

				var indFinish = (int)taskFinish.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				indFinish = Math.Min(Math.Max(0, indFinish) + m_indShift, m_maxInd);
				DateTime today = DateTime.Now.Date;
				for (int i = indStart; i <= indFinish; i++)
				{
					DateTime date = today.AddDays(i - m_indShift);
					if (IsHoliday(date))
						continue;
					taskRow.Cells[i].Value = userMark;
				}
				return indFinish + 1;
			}
			return m_indShift;
		}

		private int AddDatesProposed(
			WorkItem task,
			DataGridViewRow taskRow,
			int startInd,
			string userMark)
		{
			double? estimate = task.Estimate();
			if (estimate == null)
			{
				taskRow.Cells[m_indShift - 1].SetErrorColor();
				taskRow.Cells[m_indShift - 1].ToolTipText = Messages.NoEstimate();
				return m_indShift;
			}

			return AddDates(
				taskRow,
				estimate.Value,
				startInd,
				userMark);
		}

		private int AddDates(
			DataGridViewRow row,
			double duration,
			int startInd,
			string userMark)
		{
			if (startInd - m_indShift > m_maxInd)
				return m_indShift + m_maxInd + 1;
			var length = (int)Math.Ceiling(duration / 8 / m_focusFactor);
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
				if (cell.Value == null)
					cell.Value = userMark;
				else
					cell.Value = cell.Value + userMark;
				++ind;
				--length;
			}
			return startInd + ind;
		}

		private bool IsHoliday(DateTime dateTime)
		{
			var date = dateTime.Date;
			var dayOfWeek = date.DayOfWeek;
			if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
				return true;
			if (Holidays == null)
				return false;
			return Holidays.Contains(date);
		}
	}
}
