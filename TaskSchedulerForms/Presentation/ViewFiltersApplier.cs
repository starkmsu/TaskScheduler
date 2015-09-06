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
		private readonly Dictionary<string, List<int>> m_sprintLeadtTasksIndexesDict = new Dictionary<string, List<int>>();
		private readonly Dictionary<int, List<int>> m_blockersIndexesDict = new Dictionary<int, List<int>>();

		private string m_filteringUser;
		private string m_filteringSprint;
		private bool m_filterByDevCompleted;
		private bool m_filterByLeadTasks;
		private bool m_expandBlockers;

		internal List<string> Users { get; private set; }

		internal List<string> Sprints { get; private set; }

		internal ViewFiltersApplier(
			DataGridView dataGridView,
			List<int> leadTasksRowsIndexes,
			List<int> leadTasksWithDevCompletedRowsIndexes,
			Dictionary<int, int> taskToLeadTasksIndexesDict,
			Dictionary<int, List<int>> leadTaskToTaskIndexesDict,
			Dictionary<string, List<int>> usersTasksIndexesDict,
			Dictionary<string, List<int>> sprintLeadtTasksIndexesDict,
			Dictionary<int, List<int>> blockersIndexesDict)
		{
			m_dataGridView = dataGridView;

			m_leadTasksRowsIndexes = leadTasksRowsIndexes;
			m_leadTasksWithDevCompletedRowsIndexes = leadTasksWithDevCompletedRowsIndexes;

			m_taskToLeadTaskIndexesDict = taskToLeadTasksIndexesDict;
			m_leadTaskToTaskIndexesDict = leadTaskToTaskIndexesDict;
			m_usersTasksIndexesDict = usersTasksIndexesDict;
			m_sprintLeadtTasksIndexesDict = sprintLeadtTasksIndexesDict;
			m_blockersIndexesDict = blockersIndexesDict;

			List<string> users = m_usersTasksIndexesDict.Keys.ToList();
			users.Sort();
			Users = users;

			List<string> sprints = m_sprintLeadtTasksIndexesDict.Keys.ToList();
			sprints.Sort();
			Sprints = sprints;
		}

		internal void FilterDataByUser(string user)
		{
			m_filteringUser = user;
			DoFiltering();
		}

		internal void FilterDataBySprint(string sprint)
		{
			m_filteringSprint = sprint;
			DoFiltering();
		}

		internal void ToggleLeadTaskMode()
		{
			m_filterByLeadTasks = !m_filterByLeadTasks;
			DoFiltering();
		}

		internal void ToggleDevCompletedMode()
		{
			m_filterByDevCompleted = !m_filterByDevCompleted;
			DoFiltering();
		}

		internal void ToggleBlockers()
		{
			m_expandBlockers = !m_expandBlockers;
			DoFiltering();
		}

		internal void DoFiltering()
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
					userTasksIndexes
						.Where(i => m_taskToLeadTaskIndexesDict.ContainsKey(i))
						.Select(i => m_taskToLeadTaskIndexesDict[i]));
			}

			if (!string.IsNullOrEmpty(m_filteringSprint))
				result.RemoveAll(lt => !m_sprintLeadtTasksIndexesDict[m_filteringSprint].Contains(lt));

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
