using System;
using System.Windows.Forms;
using TaskSchedulerForms.Const;

namespace TaskSchedulerForms.Presentation
{
	internal class ScheduleColumnsPresenter
	{
		internal static void InitColumns(DataGridView dgv, int indShift)
		{
			while (dgv.Columns.Count > indShift)
			{
				dgv.Columns.RemoveAt(indShift);
			}
			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				string dateText = i.ToString("dd.MM");
				var column = new DataGridViewTextBoxColumn
				{
					Name = dateText,
					HeaderText = dateText,
					Width = 40,
					Resizable = DataGridViewTriState.False,
					SortMode = DataGridViewColumnSortMode.NotSortable
				};
				if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
					column.HeaderCell.Style.BackColor = column.DefaultCellStyle.BackColor = CellsPalette.WeekEnd;
				dgv.Columns.Add(column);
			}
		}
	}
}
