using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal class RowsAdder
	{
		private const string m_proposedLtMark = "O";
		private const string m_activeLtMark = "X";

		internal static int AddLeadTaskRow(
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
				return ScheduleFiller.AddDatesProposed(
					viewColumnsIndexes,
					freeDaysCalculator,
					leadTask,
					leadTaskRow,
					viewColumnsIndexes.FirstDateColumnIndex,
					m_proposedLtMark,
					m_proposedLtMark,
					shouldCheckEstimate);
			return ScheduleFiller.AddDatesActive(
				viewColumnsIndexes,
				freeDaysCalculator,
				leadTask,
				leadTaskRow,
				viewColumnsIndexes.FirstDateColumnIndex,
				m_activeLtMark,
				m_activeLtMark);
		}

		internal static int AddTaskRow(
			DataGridView dgv,
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItemInfoFiller workItemInfoFiller,
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem task,
			List<WorkItem> siblings,
			int? leadTaskPriority,
			DataContainer data,
			Dictionary<int, int> alreadyAdded,
			Dictionary<string, int> tasksByUser,
			Dictionary<int, Tuple<int?, int>> tasksSchedule)
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
				siblings,
				leadTaskPriority,
				alreadyAdded,
				nextInds,
				tasksByUser,
				tasksSchedule);

			dgv.Rows.Add(new DataGridViewRow());
			var taskRow = dgv.Rows[dgv.Rows.Count - 1];

			workItemInfoFiller.FillTaskInfo(
				viewFiltersBuilder,
				task,
				siblings,
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

			string userMark = GetUserMark(assignedTo);

			Tuple<int?, int> taskSchedule = null;
			int nextInd;

			if (tasksSchedule != null && tasksSchedule.ContainsKey(task.Id))
				taskSchedule = tasksSchedule[task.Id];

			if (taskSchedule != null && taskSchedule.Item1 != null)
			{
				nextInd = ScheduleFiller.AddDates(
					viewColumnsIndexes,
					freeDaysCalculator,
					taskRow,
					taskSchedule.Item1.Value,
					taskSchedule.Item2,
					false,
					assignedTo,
					userMark);
			}
			else
			{
				nextInd = task.State == WorkItemState.Proposed || task.State == WorkItemState.ToDo
					? ScheduleFiller.AddDatesProposed(
						viewColumnsIndexes,
						freeDaysCalculator,
						task,
						taskRow,
						maxNextInd,
						assignedTo,
						userMark,
						true)
					: ScheduleFiller.AddDatesActive(
						viewColumnsIndexes,
						freeDaysCalculator,
						task,
						taskRow,
						maxNextInd,
						assignedTo,
						userMark);
			}
			alreadyAdded.Add(task.Id, nextInd);
			tasksByUser[assignedTo] = nextInd;
			return nextInd;
		}

		private static string GetUserMark(string user)
		{
			if (string.IsNullOrEmpty(user))
				return string.Empty;
			string[] words = user.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length == 0)
				return string.Empty;
			if (words.Length == 1)
				return words[0].Length > 3 ? words[0].Substring(0, 3) : words[0];
			string result = string.Empty;
			for (int i = 0; i < words.Length && i < 3; i++)
			{
				result += words[i].Substring(0, 1);
			}
			return result;
		}

		private static void AddBlockerRow(
			DataGridView dgv,
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItemInfoFiller workItemInfoFiller,
			DataContainer data,
			int blockerId)
		{
			dgv.Rows.Add(new DataGridViewRow());
			var blockerRow = dgv.Rows[dgv.Rows.Count - 1];
			WorkItem blocker = data.WiDict[blockerId];
			workItemInfoFiller.FillBlockerInfo(blockerRow, blocker);
			viewFiltersBuilder.MarkBlockerRow(blockerRow);
		}

		private static List<int> ProcessBlockers(
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
			Dictionary<string, int> tasksByUser,
			Dictionary<int, Tuple<int?, int>> tasksSchedule)
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
						tasksByUser,
						tasksSchedule);
					nextInds.Add(blockerNextInd);
				}
			}

			return blockerIds;
		}
	}
}
