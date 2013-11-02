﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Accessors;
using TfsUtils.Const;
using TfsUtils.Parsers;
using WorkItemLinkType = TfsUtils.Const.WorkItemLinkType;
using WorkItemType = TfsUtils.Const.WorkItemType;

namespace TaskSchedulerForms
{
	internal class DataProcessor
	{
		internal DataContainer ProcessLeadTasks(string tfsUrl, List<WorkItem> leadTasks)
		{
			var result = new DataContainer
			{
				WiDict = new Dictionary<int, WorkItem>(),
				BlockersDict = new Dictionary<int, List<int>>(),
				LeadTaskChildrenDict = new Dictionary<int, List<int>>(),
				NonChildBlockers = new Dictionary<int, WorkItem>()
			};

			AppendLeadTasks(result, leadTasks);

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				List<int> ltChildrenIds = result.LeadTaskChildrenDict.Values.SelectMany(i => i).ToList();
				AppendTasks(ltChildrenIds, null, result, wiqlAccessor);

				CleanDict(result.LeadTaskChildrenDict, result.WiDict, false, true, wiqlAccessor);
				CleanDict(result.BlockersDict, result.WiDict, true, false, wiqlAccessor);

				InitExternalBlockers(result, wiqlAccessor);
			}

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

		private void AppendTasks(
			List<int> childrenIds,
			int? rootLeadTasskId,
			DataContainer dataContainer,
			TfsWiqlAccessor wiqlAccessor)
		{
			if (childrenIds.Count == 0)
				return;

			var childrenTasks = wiqlAccessor.QueryWorkItemsByIds(childrenIds);
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

				var taskLinkDict = WorkItemParser.ParseLinks(task);
				int? currentParentLtId = rootLeadTasskId;
				if (currentParentLtId == null
					&& taskLinkDict.ContainsKey(WorkItemLinkType.Parent))
				{
					int ltId = taskLinkDict[WorkItemLinkType.Parent][0];
					if (dataContainer.LeadTaskChildrenDict.ContainsKey(ltId))
						currentParentLtId = ltId;
				}
				if (currentParentLtId != null)
				{
					var ltChildren = dataContainer.LeadTaskChildrenDict[currentParentLtId.Value];
					if (!ltChildren.Contains(task.Id))
						ltChildren.Add(task.Id);
				}
				if (!taskLinkDict.ContainsKey(WorkItemLinkType.Child))
					continue;
				AppendTasks(
					taskLinkDict[WorkItemLinkType.Child],
					currentParentLtId,
					dataContainer,
					wiqlAccessor);
			}
		}

		private void CleanDict(
			Dictionary<int, List<int>> dict,
			Dictionary<int, WorkItem> wiDict,
			bool deleteEmpty,
			bool useResolvedLinkedfWorkItems,
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
							notFoundIdsDict.Add(id, new List<List<int>>(1) { pair.Value });
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
					if (workItem.State != WorkItemState.Closed
						&& (useResolvedLinkedfWorkItems || workItem.State != WorkItemState.Resolved))
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

		private void InitExternalBlockers(DataContainer dataContainer, TfsWiqlAccessor wiqlAccessor)
		{
			if (dataContainer.BlockersDict.Count == 0)
				return;

			foreach (var pair in dataContainer.BlockersDict)
			{
				foreach (int blockerId in pair.Value.Where(i => !dataContainer.WiDict.ContainsKey(i)))
				{
					if (dataContainer.NonChildBlockers.ContainsKey(blockerId))
						continue;
					dataContainer.NonChildBlockers.Add(blockerId, null);
				}
			}

			if (dataContainer.NonChildBlockers.Keys.Count == 0)
				return;

			var workItems = wiqlAccessor.QueryWorkItemsByIds(dataContainer.NonChildBlockers.Keys.ToList());
			for (int i = 0; i < workItems.Count; i++)
			{
				WorkItem workItem = workItems[i];
				dataContainer.NonChildBlockers[workItem.Id] = workItem;
			}
		}
	}
}
