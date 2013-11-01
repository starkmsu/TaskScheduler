﻿using System.Collections.Generic;
using System.Windows.Forms;
using TaskPlanningForms.Properties;
using TfsUtils.Const;

namespace TaskPlanningForms
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
		private readonly Dictionary<int, int> m_blockersIndexesDict = new Dictionary<int, int>(); 

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

			List<int> indexes;
			if (m_usersTasksIndexesDict.ContainsKey(user))
			{
				indexes = m_usersTasksIndexesDict[user];
			}
			else
			{
				indexes = new List<int>();
				m_usersTasksIndexesDict.Add(user, indexes);
			}
			indexes.Add(m_currentRowIndex);
		}

		internal void MarkBlockerRow(DataGridViewRow row)
		{
			int index = m_dataGridView.Rows.IndexOf(row);
			m_blockersIndexesDict[m_currentRowIndex] = index;
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
				m_blockersIndexesDict);
		}
	}
}
