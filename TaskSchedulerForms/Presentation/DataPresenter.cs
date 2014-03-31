using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TaskSchedulerForms.Properties;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal class DataPresenter
	{
		private const double m_focusFactor = 0.5f;
		private const string m_proposedLtMark = "O";
		private const string m_activeLtMark = "X";
		private readonly int m_maxInd = (int)DateTime.Now.AddMonths(1).Date.Subtract(DateTime.Now.Date).TotalDays;

		internal void ToggleIteration(
			DataGridView dgv,
			ViewColumnsIndexes viewColumnsIndexes,
			bool showIterationGlag)
		{
			dgv.Columns[viewColumnsIndexes.IterationColumnIndex].Visible = showIterationGlag;
		}

		internal ViewFiltersApplier PresentData(
			DataContainer data,
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridView dgv)
		{
			var alreadyAdded = new Dictionary<int, int>();
			var tasksByUser = new Dictionary<string, int>();

			DateTime today = DateTime.Now.Date;
			var resultBuilder = new ViewFiltersBuilder(dgv, viewColumnsIndexes);
			var workItemInfoFiller = new WorkItemInfoFiller(dgv, viewColumnsIndexes);

			foreach (var leadTaskChildren in data.LeadTaskChildrenDict)
			{
				var leadTask = data.WiDict[leadTaskChildren.Key];

				int nextLtInd = AddLeadTaskRow(
					dgv,
					resultBuilder,
					workItemInfoFiller,
					viewColumnsIndexes,
					freeDaysCalculator,
					leadTask,
					data);
				int ltRowInd = dgv.Rows.Count - 1;

				var childrenTasks = leadTaskChildren.Value
					.Where(i => data.WiDict.ContainsKey(i))
					.Select(i => data.WiDict[i])
					.OrderBy(i => i.Priority() ?? 999)
					.ToList();

				if (childrenTasks.Count > 0)
				{
					int lastTaskInd = childrenTasks
						.Select(task =>
							AddTaskRow(
								dgv,
								resultBuilder,
								workItemInfoFiller,
								viewColumnsIndexes,
								freeDaysCalculator,
								task,
								childrenTasks,
								leadTask.Priority(),
								data,
								alreadyAdded,
								tasksByUser))
						.Max();
					for (int i = nextLtInd; i < lastTaskInd; i++)
					{
						DateTime date = today.AddDays(i - viewColumnsIndexes.FirstDateColumnIndex);
						if (freeDaysCalculator.GetDayType(date) != DayType.WorkDay)
							continue;
						dgv.Rows[ltRowInd].Cells[i].SetErrorColor();
						dgv.Rows[ltRowInd].Cells[i].ToolTipText = Messages.ChildTaskHasLaterFd();
					}
					if (dgv.Rows[ltRowInd].Cells[viewColumnsIndexes.PriorityColumnIndex].IsColorForState(WorkItemState.Proposed)
						|| dgv.Rows[ltRowInd].Cells[viewColumnsIndexes.PriorityColumnIndex].IsColorForState(WorkItemState.ToDo))
					{
						int lasttChildRowInd = dgv.Rows.Count - 1;
						for (int i = ltRowInd + 1; i <= lasttChildRowInd; i++)
						{
							if (dgv.Rows[i].Cells[viewColumnsIndexes.PriorityColumnIndex].IsColorForState(WorkItemState.Active)
								|| dgv.Rows[i].Cells[viewColumnsIndexes.PriorityColumnIndex].IsColorForState(WorkItemState.Resolved)
								|| dgv.Rows[i].Cells[viewColumnsIndexes.PriorityColumnIndex].IsColorForState(WorkItemState.Done))
							{
								dgv.Rows[ltRowInd].Cells[viewColumnsIndexes.PriorityColumnIndex].SetErrorColor();
								dgv.Rows[ltRowInd].Cells[viewColumnsIndexes.PriorityColumnIndex].ToolTipText = Messages.ProposedLeadTaskHasNotProposedChild();
							}
						}
					}
				}

				var notAccessableChildren = leadTaskChildren.Value
					.Where(i => !data.WiDict.ContainsKey(i))
					.ToList();
				foreach (int notAccessableChildId in notAccessableChildren)
				{
					dgv.Rows.Add(new DataGridViewRow());
					var taskRow = dgv.Rows[dgv.Rows.Count - 1];
					workItemInfoFiller.FillNotAccessibleTaskInfo(viewColumnsIndexes, taskRow, notAccessableChildId);
					resultBuilder.MarkTaskRow(taskRow);
				}
			}

			return resultBuilder.Build();
		}

		private int AddLeadTaskRow(
			DataGridView dgv,
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItemInfoFiller workItemInfoFiller,
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem leadTask,
			DataContainer data)
		{
			dgv.Rows.Add(new DataGridViewRow());
			var leadTaskRow = dgv.Rows[dgv.Rows.Count - 1];

			List<int> blockersIds = data.BlockersDict.ContainsKey(leadTask.Id)
				? data.BlockersDict[leadTask.Id]
				: null;
			bool shouldCheckEstimate = workItemInfoFiller.FillLeadTaskInfo(
				viewFiltersBuilder,
				leadTask,
				leadTaskRow,
				data,
				blockersIds);

			viewFiltersBuilder.MarkLeadTaskRow(leadTaskRow);

			if (blockersIds != null)
				foreach (int blockerId in blockersIds)
				{
					AddBlockerRow(
						dgv,
						viewFiltersBuilder,
						workItemInfoFiller,
						data,
						blockerId);
				}

			if (leadTask.State == WorkItemState.Proposed || leadTask.State == WorkItemState.ToDo)
				return AddDatesProposed(
					viewColumnsIndexes,
					freeDaysCalculator,
					leadTask,
					leadTaskRow,
					viewColumnsIndexes.FirstDateColumnIndex,
					m_proposedLtMark,
					shouldCheckEstimate);
			return AddDatesActive(
				viewColumnsIndexes,
				freeDaysCalculator,
				leadTask,
				leadTaskRow,
				viewColumnsIndexes.FirstDateColumnIndex,
				m_activeLtMark);
		}

		private void AddBlockerRow(
			DataGridView dgv,
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItemInfoFiller workItemInfoFiller,
			DataContainer data,
			int blockerId)
		{
			dgv.Rows.Add(new DataGridViewRow());
			var blockerRow = dgv.Rows[dgv.Rows.Count - 1];
			WorkItem blocker = data.NonChildBlockers.ContainsKey(blockerId)
				? data.NonChildBlockers[blockerId]
				: data.WiDict[blockerId];
			workItemInfoFiller.FillBlockerInfo(blockerRow, blocker);
			viewFiltersBuilder.MarkBlockerRow(blockerRow);
		}

		private int AddTaskRow(
			DataGridView dgv,
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItemInfoFiller workItemInfoFiller,
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem task,
			List<WorkItem> childrenTasks,
			int? leadTaskPriority,
			DataContainer data,
			Dictionary<int, int> alreadyAdded,
			Dictionary<string, int> tasksByUser)
		{
			if (alreadyAdded.ContainsKey(task.Id))
				return viewColumnsIndexes.FirstDateColumnIndex;

			var nextInds = new List<int>();

			List<int> blockerIds = ProcessBlockers(
				dgv,
				viewFiltersBuilder,
				workItemInfoFiller,
				viewColumnsIndexes,
				freeDaysCalculator,
				data,
				task,
				childrenTasks,
				leadTaskPriority,
				alreadyAdded,
				nextInds,
				tasksByUser);

			dgv.Rows.Add(new DataGridViewRow());
			var taskRow = dgv.Rows[dgv.Rows.Count - 1];

			workItemInfoFiller.FillTaskInfo(
				viewFiltersBuilder,
				task,
				leadTaskPriority,
				taskRow,
				data,
				blockerIds);

			viewFiltersBuilder.MarkTaskRow(taskRow);

			if (blockerIds != null)
				foreach (int blockerId in blockerIds)
				{
					AddBlockerRow(
						dgv,
						viewFiltersBuilder,
						workItemInfoFiller,
						data,
						blockerId);
				}

			if (task.State == WorkItemState.Resolved || task.State == WorkItemState.Done)
			{
				alreadyAdded.Add(task.Id, viewColumnsIndexes.FirstDateColumnIndex);
				return viewColumnsIndexes.FirstDateColumnIndex;
			}

			string assignedTo = task.AssignedTo();
			if (!assignedTo.IsUnassigned() && tasksByUser.ContainsKey(assignedTo))
				nextInds.Add(tasksByUser[assignedTo]);

			int maxNextInd = viewColumnsIndexes.FirstDateColumnIndex;
			if (nextInds.Count > 0)
				maxNextInd = nextInds.Max();

			string userMark = assignedTo.Length > 0 ? assignedTo.Substring(0, 3) : Resources.Nobody;
			int nextInd = task.State == WorkItemState.Proposed || task.State == WorkItemState.ToDo
				? AddDatesProposed(
					viewColumnsIndexes,
					freeDaysCalculator,
					task,
					taskRow,
					maxNextInd,
					userMark,
					true)
				: AddDatesActive(
					viewColumnsIndexes,
					freeDaysCalculator,
					task,
					taskRow,
					maxNextInd,
					userMark);

			SetVacations(
				viewColumnsIndexes,
				freeDaysCalculator,
				taskRow,
				userMark);

			alreadyAdded.Add(task.Id, nextInd);
			tasksByUser[assignedTo] = nextInd;
			return nextInd;
		}

		private List<int> ProcessBlockers(
			DataGridView dgv,
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItemInfoFiller workItemInfoFiller,
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataContainer data,
			WorkItem task,
			List<WorkItem> childrenTasks,
			int? leadTaskPriority,
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
						viewFiltersBuilder,
						workItemInfoFiller,
						viewColumnsIndexes,
						freeDaysCalculator,
						blockerSiblingTask,
						childrenTasks,
						leadTaskPriority,
						data,
						alreadyAdded,
						tasksByUser);
					nextInds.Add(blockerNextInd);
				}
			}

			return blockerIds;
		}

		private int AddDatesActive(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem workItem,
			DataGridViewRow row,
			int startInd,
			string userMark)
		{
			var taskStart = workItem.StartDate();
			var taskFinish = workItem.FinishDate();
			if (taskFinish == null)
				return viewColumnsIndexes.FirstDateColumnIndex;

			if (taskFinish.Value.Date < DateTime.Now.Date)
			{
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskFinish.Value.ToString("dd.MM");

				var verificationResult = WorkItemVerifier.VerifyFinishDate(workItem);
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].SetColorByVerification(verificationResult.Result);
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].ToolTipText = verificationResult.AllMessagesString;

				double? remaining = workItem.Remaining();
				if (remaining.HasValue)
				{
					var length = (int)Math.Ceiling(remaining.Value / 8 / m_focusFactor);
					return AddDates(
						viewColumnsIndexes,
						freeDaysCalculator,
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
					row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskStart.Value.ToString("dd.MM");
				indStart = Math.Min(Math.Max(0, indStart), m_maxInd) + viewColumnsIndexes.FirstDateColumnIndex;

				var indFinish = (int)taskFinish.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				indFinish = Math.Min(Math.Max(0, indFinish), m_maxInd) + viewColumnsIndexes.FirstDateColumnIndex;
				DateTime today = DateTime.Now.Date;
				for (int i = indStart; i <= indFinish; i++)
				{
					DateTime date = today.AddDays(i - viewColumnsIndexes.FirstDateColumnIndex);
					if (ColorCellIfFreeDay(
						freeDaysCalculator,
						row.Cells[i],
						date,
						userMark))
						continue;
					row.Cells[i].Value = userMark;
				}
				return indFinish + 1;
			}
			return viewColumnsIndexes.FirstDateColumnIndex;
		}

		private int AddDatesProposed(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem task,
			DataGridViewRow taskRow,
			int startInd,
			string userMark,
			bool shouldCheckEstimate)
		{
			var verificationResult = WorkItemVerifier.VerifyEstimatePresence(task);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				if (shouldCheckEstimate)
				{
					var estimateCell = taskRow.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1];
					estimateCell.SetColorByVerification(verificationResult.Result);
					estimateCell.ToolTipText = verificationResult.AllMessagesString;
				}
				return viewColumnsIndexes.FirstDateColumnIndex;
			}

			double? estimate = task.Estimate();

			var length = (int)Math.Ceiling(estimate.Value / 8 / m_focusFactor);

			if (task.FinishDate() != null)
			{
				int finishShift = length - 1;
				DateTime startDate = task.FinishDate().Value.Date;
				DateTime today = DateTime.Now.Date;
				while (finishShift > 0 && startDate >= today)
				{
					startDate = startDate.AddDays(-1);
					if (freeDaysCalculator.GetDayType(startDate, userMark) == DayType.WorkDay)
						--finishShift;
				}
				var startShift = (int)startDate.Subtract(DateTime.Now.Date).TotalDays;
				startInd = Math.Max(startInd, startShift + viewColumnsIndexes.FirstDateColumnIndex);
			}

			return AddDates(
				viewColumnsIndexes,
				freeDaysCalculator,
				taskRow,
				startInd,
				length,
				userMark);
		}

		private int AddDates(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			int startInd,
			int length,
			string userMark)
		{
			if (startInd - viewColumnsIndexes.FirstDateColumnIndex > m_maxInd)
				return viewColumnsIndexes.FirstDateColumnIndex + m_maxInd + 1;
			int ind = 0;
			while (length > 0)
			{
				int dateIndexShift = startInd - viewColumnsIndexes.FirstDateColumnIndex + ind;
				if (dateIndexShift > m_maxInd)
					return viewColumnsIndexes.FirstDateColumnIndex + m_maxInd + 1;
				var date = DateTime.Now.AddDays(dateIndexShift);
				var cell = row.Cells[startInd + ind];
				if (ColorCellIfFreeDay(
					freeDaysCalculator,
					cell,
					date,
					userMark))
				{
					++ind;
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

		private void SetVacations(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			string user)
		{
			List<DateTime> vacations = freeDaysCalculator.GetVacations(user);
			if (vacations == null || vacations.Count == 0)
				return;

			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			if (finish < vacations[0])
				return;
			if (start > vacations[vacations.Count - 1])
				return;
			int ind = viewColumnsIndexes.FirstDateColumnIndex;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				if (vacations.Any(d => d == i))
					row.Cells[ind].SetColorByDayType(DayType.Vacations);
				++ind;
			}
		}

		private bool ColorCellIfFreeDay(
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewCell cell,
			DateTime dateTime,
			string user)
		{
			DayType dt = freeDaysCalculator.GetDayType(dateTime, user);
			cell.SetColorByDayType(dt);
			return dt != DayType.WorkDay;
		}
	}
}
