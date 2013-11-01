using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskPlanningForms
{
	internal class ViewColumnsIndexes
	{
		private readonly int? m_priorityInd;
		private readonly int? m_leadTaskIdInd;
		private readonly int? m_docsInd;
		private readonly int? m_titleInd;
		private readonly int? m_blockersInd;
		private readonly int? m_assignedToInd;
		private readonly int? m_pastInd;
		private readonly int? m_indShift;

		internal int PriorityColumnIndex { get { return m_priorityInd.Value; } }

		internal int LeadTaskColumnIndex { get { return m_leadTaskIdInd.Value; } }

		internal int DocsColumnIndex { get { return m_docsInd.Value; } }

		internal int TitleColumnIndex { get { return m_titleInd.Value; } }

		internal int BlockersColumnIndex { get { return m_blockersInd.Value; } }

		internal int AssignedToColumnIndex { get { return m_assignedToInd.Value; } }

		internal int PastColumnIndex { get { return m_pastInd.Value; } }

		internal int FirstDateColumnIndex { get { return m_indShift.Value; } }

		internal ViewColumnsIndexes(DataGridView dataGridView)
		{
			for (int i = 0; i < dataGridView.Columns.Count; i++)
			{
				var column = dataGridView.Columns[i];
				if (column.Name == "Priority")
					m_priorityInd = i;
				else if (column.Name == "LeadTask")
					m_leadTaskIdInd = i;
				else if (column.Name == "Docs")
					m_docsInd = i;
				else if (column.Name == "Task")
					m_titleInd = i;
				else if (column.Name == "Blockers")
					m_blockersInd = i;
				else if (column.Name == "AssignedTo")
					m_assignedToInd = i;
				else if (column.Name == "Past")
					m_pastInd = i;
			}
			var listOfIndexes = new List<int?>
			{
				m_priorityInd,
				m_leadTaskIdInd,
				m_docsInd,
				m_titleInd,
				m_blockersInd,
				m_assignedToInd,
				m_pastInd
			};
			if (listOfIndexes.Any(i => i == null))
				throw new InvalidOperationException("DatagridView does not have necessary columns");
			m_indShift = listOfIndexes.Max().Value + 1;
		}
	}
}
