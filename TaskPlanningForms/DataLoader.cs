using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		internal WorkItemCollection GetLeadTasks(
			string tfsUrl,
			List<string> areaPaths,
			bool withSubAreaPaths)
		{
			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
				{"discipline", new List<object>{"Development"}},
				{"wiType", new List<object>{"LeadTask"}},
				{"wiState", new List<object>{"Proposed", "Active"}},
			};

			var strBuilder = new StringBuilder();
			strBuilder.Append("SELECT * FROM WorkItems");
			strBuilder.Append(" WHERE [System.TeamProject] = @project");
			strBuilder.Append(" AND [System.WorkItemType] IN (@wiType)");
			strBuilder.Append(" AND [System.State] IN (@wiState)");
			strBuilder.Append(" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)");
			if (withSubAreaPaths)
			{
				strBuilder.Append(" AND (");
				for (int i = 0; i < areaPaths.Count; i++)
				{
					string areaPathParam = "areaPath" + i;
					if (i > 0)
						strBuilder.Append(" OR ");
					strBuilder.Append("[System.AreaPath] UNDER @" + areaPathParam);
					paramValues.Add(areaPathParam, areaPaths[i]);
				}
				strBuilder.Append(")");
			}
			else
			{
				strBuilder.Append(" AND [System.AreaPath] IN (@areaPath)");
				complexParamValues.Add("areaPath", areaPaths.Cast<object>().ToList());
			}
			strBuilder.Append(" ORDER BY [Priority]");

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				return wiqlAccessor.QueryWorkItems(
					strBuilder.ToString(),
					paramValues,
					complexParamValues);
			}
		}

		internal WorkItemCollection GetLeadTasks(
			string tfsUrl,
			List<object> areaPaths,
			bool withSubAreaPaths,
			List<object> iterationPaths)
		{
			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
				{"iterationPath", iterationPaths},
				{"discipline", new List<object>{"Development"}},
				{"wiType", new List<object>{"LeadTask"}},
				{"wiState", new List<object>{"Proposed", "Active"}},
			};

			var strBuilder = new StringBuilder();
			strBuilder.Append("SELECT * FROM WorkItems");
			strBuilder.Append(" WHERE [System.TeamProject] = @project");
			strBuilder.Append(" AND [System.WorkItemType] IN (@wiType)");
			strBuilder.Append(" AND [System.State] IN (@wiState)");
			strBuilder.Append(" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)");
			if (withSubAreaPaths)
			{
				strBuilder.Append(" AND (");
				for (int i = 0; i < areaPaths.Count; i++)
				{
					string areaPathParam = "areaPath" + i;
					if (i > 0)
						strBuilder.Append(" OR ");
					strBuilder.Append("[System.AreaPath] UNDER @" + areaPathParam);
					paramValues.Add(areaPathParam, areaPaths[i]);
				}
				strBuilder.Append(")");
			}
			else
			{
				strBuilder.Append(" AND [System.AreaPath] IN (@areaPath)");
				complexParamValues.Add("areaPath", areaPaths);
			}
			strBuilder.Append(" AND [System.IterationPath] IN (@iterationPath)");
			strBuilder.Append(" ORDER BY [Priority]");

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				return wiqlAccessor.QueryWorkItems(
					strBuilder.ToString(),
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
