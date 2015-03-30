using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskSchedulerForms.Presentation
{
	internal class ViewColumnsIndexes
	{
		private readonly int? m_priorityInd;
		private readonly int? m_iterationInd;
		private readonly int? m_sprintInd;
		private readonly int? m_leadTaskIdInd;
		private readonly int? m_docsInd;
		private readonly int? m_titleInd;
		private readonly int? m_blockersInd;
		private readonly int? m_assignedToInd;
		private readonly int? m_workInd;
		private readonly int? m_pastInd;
		private readonly int? m_indShift;

		internal int PriorityColumnIndex { get { return m_priorityInd.Value; } }

		internal int IterationColumnIndex { get { return m_iterationInd.Value; } }

		internal int SprintColumnIndex { get { return m_sprintInd.Value; } }

		internal int IdColumnIndex { get { return m_leadTaskIdInd.Value; } }

		internal int DocsColumnIndex { get { return m_docsInd.Value; } }

		internal int TitleColumnIndex { get { return m_titleInd.Value; } }

		internal int BlockersColumnIndex { get { return m_blockersInd.Value; } }

		internal int AssignedToColumnIndex { get { return m_assignedToInd.Value; } }

		internal int WorkColumnIndex { get { return m_workInd.Value; } }

		internal int PastColumnIndex { get { return m_pastInd.Value; } }

		internal int FirstDateColumnIndex { get { return m_indShift.Value; } }

		internal ViewColumnsIndexes(DataGridView dataGridView)
		{
			for (int i = 0; i < dataGridView.Columns.Count; i++)
			{
				var column = dataGridView.Columns[i];
				if (column.Name.StartsWith("Priority"))
					m_priorityInd = i;
				else if (column.Name.StartsWith("Iteration"))
					m_iterationInd = i;
				else if (column.Name.StartsWith("Sprint"))
					m_sprintInd = i;
				else if (column.Name.StartsWith("Id"))
					m_leadTaskIdInd = i;
				else if (column.Name.StartsWith("Docs"))
					m_docsInd = i;
				else if (column.Name.StartsWith("Task"))
					m_titleInd = i;
				else if (column.Name.StartsWith("Blockers"))
					m_blockersInd = i;
				else if (column.Name.StartsWith("AssignedTo"))
					m_assignedToInd = i;
				else if (column.Name.StartsWith("Work"))
					m_workInd = i;
				else if (column.Name.StartsWith("Past"))
					m_pastInd = i;
			}
			var listOfIndexes = new List<int?>
			{
				m_priorityInd,
				m_iterationInd,
				m_sprintInd,
				m_leadTaskIdInd,
				m_docsInd,
				m_titleInd,
				m_blockersInd,
				m_assignedToInd,
				m_workInd,
				m_pastInd,
			};
			if (listOfIndexes.Any(i => i == null))
				throw new InvalidOperationException("DatagridView does not have necessary columns");
			m_indShift = listOfIndexes.Max().Value + 1;
		}
	}
}
