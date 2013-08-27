using System;
using System.Windows.Forms;

namespace TaskPlanningForms
{
	internal class ScheduleColumnsPresenter
	{
		private const int m_indShift = 6;

		internal int FirstDataColumnIndex { get { return m_indShift; } }

		internal void InitColumns(DataGridView dgv)
		{
			while (dgv.Columns.Count > m_indShift)
			{
				dgv.Columns.RemoveAt(m_indShift);
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
