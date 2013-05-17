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
		internal WorkItemCollection GetLeadTasks(string tfsUrl, string areaPath)
		{
			const string queryStr = "SELECT *" +
				" FROM WorkItems " +
				" WHERE [System.TeamProject] = @project" +
				" AND [System.AreaPath] Under @areaPath" +
				" AND [System.WorkItemType] IN (@wiType)" +
				" AND [System.State] IN (@wiState)" +
				" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)" +
				" ORDER BY [Priority]";

			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
				{"areaPath", areaPath},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
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

		internal WorkItemCollection GetLeadTasks(string tfsUrl, string areaPath, string iterationPath)
		{
			const string queryStr = "SELECT *" +
				" FROM WorkItems " +
				" WHERE [System.TeamProject] = @project" +
				" AND [System.AreaPath] Under @areaPath" +
				" AND [System.IterationPath] Under @iterationPath" +
				" AND [System.WorkItemType] IN (@wiType)" +
				" AND [System.State] IN (@wiState)" +
				" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)" +
				" ORDER BY [Priority]";

			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
				{"areaPath", areaPath},
				{"iterationPath", iterationPath},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
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
				LeadTaskChildrenDict = new Dictionary<int, List<int>>()
			};

			AppendLeadTasks(result, leadTasks);

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				AppendTasks(result, wiqlAccessor);
				AppendBlockers(result, wiqlAccessor);
			}

			CleanDict(result.LeadTaskChildrenDict, result.WiDict);

			return result;
		}

		private void AppendLeadTasks(DataContainer dataContainer, IEnumerable<WorkItem> leadTasks)
		{
			foreach (var leadTask in leadTasks)
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
			var ltChildrenIds = dataContainer.LeadTaskChildrenDict.Values.SelectMany(i => i).ToList();
			if (ltChildrenIds.Count == 0)
				return;

			var childrenTasks = wiqlAccessor.QueryWorkItemsByIds(ltChildrenIds);
			for (int i = 0; i < childrenTasks.Count; i++)
			{
				WorkItem task = childrenTasks[i];
				if (task.State == WorkItemState.Closed)
					continue;
				if (task.Type.Name != WorkItemType.Task)
					continue;
				dataContainer.WiDict.Add(task.Id, task);
				var taskBlockersIds = WorkItemParser.GetRelationsByType(task, WorkItemLinkType.BlockedBy);
				if (taskBlockersIds.Count > 0)
					dataContainer.BlockersDict.Add(task.Id, taskBlockersIds);
			}
		}

		private void AppendBlockers(DataContainer dataContainer, TfsWiqlAccessor wiqlAccessor)
		{
			if (dataContainer.BlockersDict.Count == 0)
				return;

			var blockerIds = dataContainer.BlockersDict.Values.SelectMany(i => i).ToList();
			var blockersWi = wiqlAccessor.QueryWorkItemsByIds(blockerIds);
			for (int i = 0; i < blockersWi.Count; i++)
			{
				WorkItem blocker = blockersWi[i];
				if (blocker.State == WorkItemState.Closed)
					continue;
				if (!dataContainer.WiDict.ContainsKey(blocker.Id))
					dataContainer.WiDict.Add(blocker.Id, blocker);
			}
			CleanDict(dataContainer.BlockersDict, dataContainer.WiDict);
		}

		private void CleanDict(Dictionary<int, List<int>> dict, Dictionary<int, WorkItem> wiDict)
		{
			foreach (var pair in dict)
			{
				pair.Value.RemoveAll(i => !wiDict.ContainsKey(i));
			}
			var emptyKeys = dict.Keys.Where(k => dict[k].Count == 0).ToList();
			emptyKeys.ForEach(k => dict.Remove(k));
		}
	}
}
