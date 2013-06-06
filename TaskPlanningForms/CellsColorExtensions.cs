using System;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskPlanningForms
{
	internal static class CellsColorExtensions
	{
		internal static void SetColorByState(this DataGridViewCell cell, WorkItem workItem)
		{
			switch (workItem.State)
			{
				case WorkItemState.Proposed:
					cell.Style.BackColor = CellsPalette.StateProposed;
					break;
				case WorkItemState.Active:
					cell.Style.BackColor = CellsPalette.StateActive;
					break;
				case WorkItemState.Resolved:
					cell.Style.BackColor = CellsPalette.StateResolved;
					break;
			}
		}

		internal static void SetColorByDiscipline(this DataGridViewCell cell, WorkItem workItem)
		{
			switch (workItem.Discipline())
			{
				case WorlItemDiscipline.Development:
					cell.Style.BackColor = CellsPalette.DisciplineDevelopment;
					break;
				case WorlItemDiscipline.AutoTesting:
					cell.Style.BackColor = CellsPalette.DisciplineAutoTesting;
					break;
				case WorlItemDiscipline.ProductTesting:
					cell.Style.BackColor = CellsPalette.DisciplineProductTesting;
					break;
				case WorlItemDiscipline.Architecture:
					cell.Style.BackColor = CellsPalette.DisciplineArchitecture;
					break;
			}
		}

		internal static void SetErrorColor(this DataGridViewCell cell)
		{
			cell.Style.BackColor = CellsPalette.Error;
		}

		internal static bool IsColorForState(this DataGridViewCell cell, string state)
		{
			if (state == WorkItemState.Proposed)
				return cell.Style.BackColor == CellsPalette.StateProposed;
			if (state == WorkItemState.Active)
				return cell.Style.BackColor == CellsPalette.StateActive;
			if (state == WorkItemState.Resolved)
				return cell.Style.BackColor == CellsPalette.StateResolved;
			throw new NotImplementedException("Unkonown state " + state);
		}
	}
}
