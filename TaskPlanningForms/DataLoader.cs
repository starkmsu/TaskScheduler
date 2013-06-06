using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Accessors;
using TfsUtils.Const;
using TfsUtils.Parsers;
using WorkItemLinkType = TfsUtils.Const.WorkItemLinkType;
using WorkItemType = TfsUtils.Const.WorkItemType;

namespace TaskPlanningForms
{
	internal class DataLoader
	{
		internal WorkItemCollection GetLeadTasks(string tfsUrl, List<string> areaPaths)
		{
			const string queryStr = "SELECT *" +
				" FROM WorkItems " +
				" WHERE [System.TeamProject] = @project" +
				" AND [System.AreaPath] IN (@areaPath)" +
				" AND [System.WorkItemType] IN (@wiType)" +
				" AND [System.State] IN (@wiState)" +
				" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)" +
				" ORDER BY [Priority]";

			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
				{"areaPath", areaPaths.Cast<object>().ToList()},
				{"discipline", new List<object>{"Development"}},
				{"wiType", new List<object>{"LeadTask"}},
				{"wiState", new List<object>{"Proposed", "Active"}},
			};

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				return wiqlAccessor.QueryWorkItems(
					queryStr,
					paramValues,
					complexParamValues);
			}
		}

		internal WorkItemCollection GetLeadTasks(
			string tfsUrl,
			List<object> areaPaths,
			List<object> iterationPaths)
		{
			const string queryStr = "SELECT *" +
				" FROM WorkItems " +
				" WHERE [System.TeamProject] = @project" +
				" AND [System.AreaPath] IN (@areaPath)" +
				" AND [System.IterationPath] IN (@iterationPath)" +
				" AND [System.WorkItemType] IN (@wiType)" +
				" AND [System.State] IN (@wiState)" +
				" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)" +
				" ORDER BY [Priority]";

			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
				{"iterationPath", iterationPaths},
				{"areaPath", areaPaths},
				{"discipline", new List<object>{"Development"}},
				{"wiType", new List<object>{"LeadTask"}},
				{"wiState", new List<object>{"Proposed", "Active"}},
			};

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				return wiqlAccessor.QueryWorkItems(
					queryStr,
					paramValues,
					complexParamValues);
			}
		}

		internal DataContainer ProcessLeadTasks(string tfsUrl, List<WorkItem> leadTasks)
		{
			var result = new DataContainer
			{
				WiDict = new Dictionary<int, WorkItem>(),
				BlockersDict = new Dictionary<int, List<int>>(),
				LeadTaskChildrenDict = new Dictionary<int, List<int>>(),
				NonChildBlockers = new HashSet<int>()
			};

			AppendLeadTasks(result, leadTasks);

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				AppendTasks(result, wiqlAccessor);
				CleanDict(result.LeadTaskChildrenDict, result.WiDict, false, wiqlAccessor);
				CleanDict(result.BlockersDict, result.WiDict, true, wiqlAccessor);
			}

			InitExternalBlockers(result);

			return result;
		}

		private void AppendLeadTasks(DataContainer dataContainer, IEnumerable<WorkItem> leadTasks)
		{
			foreach (WorkItem leadTask in leadTasks)
			{
				var leadTaskLinkDict = WorkItemParser.ParseLinks(leadTask);

				dataContainer.LeadTaskChildrenDict.Add(
					leadTask.Id,
					leadTaskLinkDict.ContainsKey(WorkItemLinkType.Child)
						? leadTaskLinkDict[WorkItemLinkType.Child]
						: new List<int>());

				dataContainer.WiDict.Add(leadTask.Id, leadTask);

				if (leadTaskLinkDict.ContainsKey(WorkItemLinkType.BlockedBy))
					dataContainer.BlockersDict.Add(leadTask.Id, leadTaskLinkDict[WorkItemLinkType.BlockedBy]);
			}
		}

		private void AppendTasks(DataContainer dataContainer, TfsWiqlAccessor wiqlAccessor)
		{
			List<int> ltChildrenIds = dataContainer.LeadTaskChildrenDict.Values.SelectMany(i => i).ToList();
			if (ltChildrenIds.Count == 0)
				return;

			var childrenTasks = wiqlAccessor.QueryWorkItemsByIds(ltChildrenIds);
			for (int i = 0; i < childrenTasks.Count; i++)
			{
				WorkItem task = childrenTasks[i];
				dataContainer.WiDict.Add(task.Id, task);
				if (task.State == WorkItemState.Closed)
					continue;
				if (task.Type.Name != WorkItemType.Task)
					continue;
				List<int> taskBlockersIds = WorkItemParser.GetRelationsByType(task, WorkItemLinkType.BlockedBy);
				if (taskBlockersIds.Count > 0)
					dataContainer.BlockersDict.Add(task.Id, taskBlockersIds);
			}
		}

		private void CleanDict(
			Dictionary<int, List<int>> dict,
			Dictionary<int, WorkItem> wiDict,
			bool deleteEmpty,
			TfsWiqlAccessor wiqlAccessor)
		{
			var notFoundIdsDict = new Dictionary<int, List<List<int>>>(dict.Keys.Count);
			foreach (var pair in dict)
			{
				var idsToRemove = new HashSet<int>();
				foreach (int id in pair.Value)
				{
					if (wiDict.ContainsKey(id))
					{
						if (wiDict[id].State == WorkItemState.Closed)
							idsToRemove.Add(id);
					}
					else
					{
						if (notFoundIdsDict.ContainsKey(id))
							notFoundIdsDict[id].Add(pair.Value);
						else
							notFoundIdsDict.Add(id, new List<List<int>>(1){pair.Value});
					}
				}
				foreach (int id in idsToRemove)
				{
					pair.Value.Remove(id);
				}
			}
			if (notFoundIdsDict.Keys.Count > 0)
			{
				var workItems = wiqlAccessor.QueryWorkItemsByIds(notFoundIdsDict.Keys.ToList());
				for (int i = 0; i < workItems.Count; i++)
				{
					WorkItem workItem = workItems[i];
					if (workItem.State != WorkItemState.Closed)
						continue;
					foreach (List<int> ids in notFoundIdsDict[workItem.Id])
					{
						ids.Remove(workItem.Id);
					}
				}
			}
			if (deleteEmpty)
			{
				var emptyKeys = dict.Keys.Where(k => dict[k].Count == 0).ToList();
				emptyKeys.ForEach(k => dict.Remove(k));
			}
		}

		private void InitExternalBlockers(DataContainer dataContainer)
		{
			foreach (var pair in dataContainer.BlockersDict)
			{
				foreach (int blockerId in pair.Value.Where(i => !dataContainer.WiDict.ContainsKey(i)))
				{
					dataContainer.NonChildBlockers.Add(blockerId);
				}
			}
		}
	}
}
