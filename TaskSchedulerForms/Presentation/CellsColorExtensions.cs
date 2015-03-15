using System;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;
using TaskSchedulerForms.Data;
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

		internal static void SetColorByVerification(this DataGridViewCell cell, VerificationResult result)
		{
			switch (result)
			{
				case VerificationResult.Error:
					cell.Style.BackColor = CellsPalette.Error;
					break;
				case VerificationResult.Warning:
					cell.Style.BackColor = CellsPalette.Warning;
					break;
			}
		}

		internal static void SetErrorColor(this DataGridViewCell cell)
		{
			cell.Style.BackColor = CellsPalette.Error;
		}

		internal static void SetColorByDayType(this DataGridViewCell cell, DayType dayType)
		{
			switch (dayType)
			{
				case DayType.WeekEnd:
					cell.Style.BackColor = CellsPalette.WeekEnd;
					break;
				case DayType.Holiday:
				case DayType.Vacations:
					cell.Style.BackColor = CellsPalette.FreeDay;
					break;
			}
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

		internal static bool IsUncolored(this DataGridViewCell cell)
		{
			var style = cell.OwningColumn.DefaultCellStyle ?? cell.OwningColumn.InheritedStyle;
			return cell.Style.BackColor == style.BackColor;
		}
	}
}
