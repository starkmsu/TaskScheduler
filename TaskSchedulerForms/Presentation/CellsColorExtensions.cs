using System;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal static class CellsColorExtensions
	{
		internal static void SetColorByState(this DataGridViewCell cell, WorkItem workItem)
		{
			switch (workItem.State)
			{
				case WorkItemState.Proposed:
				case WorkItemState.ToDo:
					cell.Style.BackColor = CellsPalette.StateProposed;
					break;
				case WorkItemState.Active:
					cell.Style.BackColor = workItem.IsDevCompleted()
						? CellsPalette.StateDevCompleted
						: CellsPalette.StateActive;
					break;
				case WorkItemState.Resolved:
				case WorkItemState.Done:
					cell.Style.BackColor = CellsPalette.StateResolved;
					break;
			}
		}

		internal static void SetErrorColor(this DataGridViewCell cell)
		{
			cell.Style.BackColor = CellsPalette.Error;
		}

		internal static void SetWarningColor(this DataGridViewCell cell)
		{
			cell.Style.BackColor = CellsPalette.Warning;
		}

		internal static void SetWeekEndColor(this DataGridViewCell cell)
		{
			cell.Style.BackColor = CellsPalette.WeekEnd;
		}

		internal static void SetFreeDayColor(this DataGridViewCell cell)
		{
			cell.Style.BackColor = CellsPalette.FreeDay;
		}

		internal static bool IsColorForState(this DataGridViewCell cell, string state)
		{
			if (state == WorkItemState.Proposed || state == WorkItemState.ToDo)
				return cell.Style.BackColor == CellsPalette.StateProposed;
			if (state == WorkItemState.Active)
				return cell.Style.BackColor == CellsPalette.StateActive;
			if (state == WorkItemState.Resolved || state == WorkItemState.Done)
				return cell.Style.BackColor == CellsPalette.StateResolved;
			if (state == WorkItemState.DevCompleted)
				return cell.Style.BackColor == CellsPalette.StateDevCompleted;
			throw new NotImplementedException("Unkonown state " + state);
		}
	}
}
