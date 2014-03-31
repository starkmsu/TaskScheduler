using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskSchedulerForms.Presentation
{
	internal class ViewFiltersApplier
	{
		private readonly DataGridView m_dataGridView;

		private readonly List<int> m_leadTasksRowsIndexes = new List<int>();
		private readonly List<int> m_leadTasksWithDevCompletedRowsIndexes = new List<int>();

		private readonly Dictionary<int, int> m_taskToLeadTaskIndexesDict = new Dictionary<int, int>();
		private readonly Dictionary<int, List<int>> m_leadTaskToTaskIndexesDict = new Dictionary<int, List<int>>();
		private readonly Dictionary<string, List<int>> m_usersTasksIndexesDict = new Dictionary<string, List<int>>();
		private readonly Dictionary<int, List<int>> m_blockersIndexesDict = new Dictionary<int, List<int>>();

		private string m_filteringUser;
		private bool m_filterByDevCompleted;
		private bool m_filterByLeadTasks;
		private bool m_expandBlockers;

		internal List<string> Users { get; private set; }

		internal ViewFiltersApplier(
			DataGridView dataGridView,
			List<int> leadTasksRowsIndexes,
			List<int> leadTasksWithDevCompletedRowsIndexes,
			Dictionary<int, int> taskToLeadTasksIndexesDict,
			Dictionary<int, List<int>> leadTaskToTaskIndexesDict,
			Dictionary<string, List<int>> usersTasksIndexesDict,
			Dictionary<int, List<int>> blockersIndexesDict)
		{
			m_dataGridView = dataGridView;

			m_leadTasksRowsIndexes = leadTasksRowsIndexes;
			m_leadTasksWithDevCompletedRowsIndexes = leadTasksWithDevCompletedRowsIndexes;

			m_taskToLeadTaskIndexesDict = taskToLeadTasksIndexesDict;
			m_leadTaskToTaskIndexesDict = leadTaskToTaskIndexesDict;
			m_usersTasksIndexesDict = usersTasksIndexesDict;
			m_blockersIndexesDict = blockersIndexesDict;

			List<string> users = m_usersTasksIndexesDict.Keys.ToList();
			users.Sort();
			Users = users;
		}

		internal void FilterDataByUser(string user)
		{
			m_filteringUser = user;
			DoFiltering();
		}

		internal void FilterDataByLeadTaskMode(bool ltOnly)
		{
			m_filterByLeadTasks = ltOnly;
			DoFiltering();
		}

		internal void FilterDataByDevCompleted(bool withDevCompleted)
		{
			m_filterByDevCompleted = !withDevCompleted;
			DoFiltering();
		}

		internal void ExpandBlockers(bool expandBlockers)
		{
			m_expandBlockers = expandBlockers;
			DoFiltering();
		}

		private void DoFiltering()
		{
			List<int> visibleIndexes = GetVisibleIndexes();
			for (int i = 0; i < m_dataGridView.Rows.Count; i++)
			{
				var row = m_dataGridView.Rows[i];
				row.Visible = visibleIndexes.Contains(i);
			}
		}

		private List<int> GetVisibleIndexes()
		{
			var result = new List<int>();

			if (string.IsNullOrEmpty(m_filteringUser))
			{
				result.AddRange(m_leadTasksRowsIndexes);
			}
			else
			{
				List<int> userTasksIndexes = m_usersTasksIndexesDict[m_filteringUser];
				result.AddRange(
					userTasksIndexes.Select(userTasksIndex =>
						m_taskToLeadTaskIndexesDict[userTasksIndex]));
			}

			if (m_filterByDevCompleted)
				result.RemoveAll(i => m_leadTasksWithDevCompletedRowsIndexes.Contains(i));

			if (!m_filterByLeadTasks)
			{
				if (string.IsNullOrEmpty(m_filteringUser))
				{
					var childrenIndexes = result
						.Where(i => m_leadTaskToTaskIndexesDict.ContainsKey(i))
						.SelectMany(i => m_leadTaskToTaskIndexesDict[i])
						.ToList();
					result.AddRange(childrenIndexes);
				}
				else
				{
					result.AddRange(m_usersTasksIndexesDict[m_filteringUser]);
				}
			}

			if (m_expandBlockers)
			{
				var blockersIndexes = result
					.Where(i => m_blockersIndexesDict.ContainsKey(i))
					.SelectMany(i => m_blockersIndexesDict[i])
					.ToList();
				result.AddRange(blockersIndexes);
			}

			return result;
		}
	}
}
