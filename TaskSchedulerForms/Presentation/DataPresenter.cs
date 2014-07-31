﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaskSchedulerForms.Data;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal class DataPresenter
	{
		internal void ToggleIteration(
			DataGridView dgv,
			ViewColumnsIndexes viewColumnsIndexes,
			bool showIterationFlag)
		{
			dgv.Columns[viewColumnsIndexes.IterationColumnIndex].Visible = showIterationFlag;
		}

		internal void ToggleSprint(
			DataGridView dgv,
			ViewColumnsIndexes viewColumnsIndexes,
			bool showSprintFlag)
		{
			dgv.Columns[viewColumnsIndexes.SprintColumnIndex].Visible = showSprintFlag;
		}

		internal ViewFiltersApplier PresentData(
			DataContainer data,
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			FocusFactorCalculator focusFactorCalculator,
			DataGridView dgv)
		{
			var tasksSchedule = WorkItemsScheduler.MakeSchedule(
				data,
				freeDaysCalculator,
				focusFactorCalculator);

			var alreadyAdded = new Dictionary<int, int>();
			var tasksByUser = new Dictionary<string, int>();

			var resultBuilder = new ViewFiltersBuilder(dgv, viewColumnsIndexes);
			var workItemInfoFiller = new WorkItemInfoFiller(dgv, viewColumnsIndexes);

			foreach (var leadTaskChildren in data.LeadTaskChildrenDict)
			{
				var leadTask = data.WiDict[leadTaskChildren.Key];

				int nextLtInd = RowsAdder.AddLeadTaskRow(
					dgv,
					resultBuilder,
					workItemInfoFiller,
					viewColumnsIndexes,
					freeDaysCalculator,
					focusFactorCalculator,
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
							RowsAdder.AddTaskRow(
								dgv,
								resultBuilder,
								workItemInfoFiller,
								viewColumnsIndexes,
								freeDaysCalculator,
								focusFactorCalculator,
								task,
								childrenTasks,
								leadTask.Priority(),
								data,
								alreadyAdded,
								tasksByUser,
								tasksSchedule))
						.Max();
					ScheduleFiller.ColorFdOutDays(
						viewColumnsIndexes,
						freeDaysCalculator,
						dgv.Rows[ltRowInd],
						nextLtInd,
						lastTaskInd);
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
	}
}
