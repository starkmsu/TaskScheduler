using System.Collections.Generic;
using System.Windows.Forms;
using TaskSchedulerForms.Properties;
using TfsUtils.Const;

namespace TaskSchedulerForms.Presentation
{
	internal class ViewFiltersBuilder
	{
		private readonly DataGridView m_dataGridView;
		private readonly ViewColumnsIndexes m_viewColumnsIndexes;

		private readonly List<int> m_leadTasksRowsIndexes = new List<int>();
		private readonly List<int> m_leadTasksWithDevCompletedRowsIndexes = new List<int>();

		private readonly Dictionary<int, int> m_taskToLeadTaskIndexesDict = new Dictionary<int, int>();
		private readonly Dictionary<int, List<int>> m_leadTaskToTaskIndexesDict = new Dictionary<int, List<int>>();
		private readonly Dictionary<string, List<int>> m_usersTasksIndexesDict = new Dictionary<string, List<int>>();
		private readonly Dictionary<string, List<int>> m_sprintLeadtTasksIndexesDict = new Dictionary<string, List<int>>();
		private readonly Dictionary<int, List<int>> m_blockersIndexesDict = new Dictionary<int, List<int>>();

		private int m_currentLeadTaskRowIndex;
		private int m_currentRowIndex;

		internal ViewFiltersBuilder(DataGridView dataGridView, ViewColumnsIndexes viewColumnsIndexes)
		{
			m_dataGridView = dataGridView;
			m_viewColumnsIndexes = viewColumnsIndexes;
		}

		internal void MarkLeadTaskRow(DataGridViewRow row)
		{
			m_currentRowIndex = m_dataGridView.Rows.IndexOf(row);
			m_currentLeadTaskRowIndex = m_currentRowIndex;
			m_leadTasksRowsIndexes.Add(m_currentLeadTaskRowIndex);
			string sprint = row.Cells[m_viewColumnsIndexes.SprintColumnIndex].Value.ToString();
			if (m_sprintLeadtTasksIndexesDict.ContainsKey(sprint))
				m_sprintLeadtTasksIndexesDict[sprint].Add(m_currentLeadTaskRowIndex);
			else
				m_sprintLeadtTasksIndexesDict.Add(sprint, new List<int> {m_currentLeadTaskRowIndex});
			bool isDevCompleted = row.Cells[m_viewColumnsIndexes.PriorityColumnIndex].IsColorForState(WorkItemState.DevCompleted);
			if (isDevCompleted)
				m_leadTasksWithDevCompletedRowsIndexes.Add(m_currentLeadTaskRowIndex);
		}

		internal void MarkTaskRow(DataGridViewRow row)
		{
			m_currentRowIndex = m_dataGridView.Rows.IndexOf(row);
			List<int> leadTaskChildrenIndexes;
			if (m_leadTaskToTaskIndexesDict.ContainsKey(m_currentLeadTaskRowIndex))
			{
				leadTaskChildrenIndexes = m_leadTaskToTaskIndexesDict[m_currentLeadTaskRowIndex];
			}
			else
			{
				leadTaskChildrenIndexes = new List<int>();
				m_leadTaskToTaskIndexesDict.Add(m_currentLeadTaskRowIndex, leadTaskChildrenIndexes);
			}
			leadTaskChildrenIndexes.Add(m_currentRowIndex);
			m_taskToLeadTaskIndexesDict[m_currentRowIndex] = m_currentLeadTaskRowIndex;
			string user = row.Cells[m_viewColumnsIndexes.AssignedToColumnIndex].Value.ToString();
			if (user == Resources.AccessDenied)
				return;

			if (m_usersTasksIndexesDict.ContainsKey(user))
				m_usersTasksIndexesDict[user].Add(m_currentRowIndex);
			else
				m_usersTasksIndexesDict.Add(user, new List<int> {m_currentRowIndex});
		}

		internal void MarkBlockerRow(DataGridViewRow row)
		{
			int index = m_dataGridView.Rows.IndexOf(row);
			if (m_blockersIndexesDict.ContainsKey(m_currentRowIndex))
				m_blockersIndexesDict[m_currentRowIndex].Add(index);
			else
				m_blockersIndexesDict.Add(m_currentRowIndex, new List<int> {index});
		}

		internal ViewFiltersApplier Build()
		{
			return new ViewFiltersApplier(
				m_dataGridView,
				m_leadTasksRowsIndexes,
				m_leadTasksWithDevCompletedRowsIndexes,
				m_taskToLeadTaskIndexesDict,
				m_leadTaskToTaskIndexesDict,
				m_usersTasksIndexesDict,
				m_sprintLeadtTasksIndexesDict,
				m_blockersIndexesDict);
		}
	}
}
