﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TfsUtils.Parsers;

namespace TaskSchedulerForms
{
	internal class WorkItemsScheduler
	{
		private const double s_workerFocusFactor = 0.5;
		private const int s_maxUserReferenceCount = 16;

		internal static Dictionary<int, Tuple<int?, int>> MakeSchedule(DataContainer dataContainer, FreeDaysCalculator freeDaysCalculator)
		{
			var usersTasksDict = SeparateByUser(dataContainer);
			var result = new Dictionary<int, Tuple<int?, int>>();
			var usersBlockersDict = new Dictionary<string, Dictionary<int, BlockerData>>();
			var usersToProcess = new List<string>(usersTasksDict.Keys);

			while (usersToProcess.Count > 0)
			{
				string user = usersToProcess[0];
				var blockersFromOtherUsers = ScheduleUserTasks(
					usersTasksDict[user],
					user,
					freeDaysCalculator,
					dataContainer,
					result);

				var usersToRecalculate = ProcessBlockers(
					blockersFromOtherUsers,
					usersBlockersDict,
					result,
					user);

				usersToProcess.RemoveAt(0);

				usersToProcess.AddRange(usersToRecalculate);

				if (usersToProcess.Count > s_maxUserReferenceCount)
					throw new InvalidOperationException("Cycle blockers collision!");
			}
			return result;
		}

		private static HashSet<string> ProcessBlockers(
			Dictionary<string, HashSet<int>> blockersFromOtherUsers,
			Dictionary<string, Dictionary<int, BlockerData>> usersBlockersDict,
			Dictionary<int, Tuple<int?, int>> taskSchedule,
			string currentUser)
		{
			foreach (KeyValuePair<string, HashSet<int>> pair in blockersFromOtherUsers)
			{
				if (pair.Key == currentUser)
					continue;

				Dictionary<int, BlockerData> userBlockersDict;
				if (usersBlockersDict.ContainsKey(pair.Key))
				{
					userBlockersDict = usersBlockersDict[pair.Key];
				}
				else
				{
					userBlockersDict = new Dictionary<int, BlockerData>();
					usersBlockersDict.Add(pair.Key, userBlockersDict);
				}
				foreach (int blockerId in pair.Value)
				{
					if (userBlockersDict.ContainsKey(blockerId))
						userBlockersDict[blockerId].BlockedUsers.Add(currentUser);
					else
						userBlockersDict.Add(
							blockerId, new BlockerData { BlockedUsers = new HashSet<string> { currentUser } });

					userBlockersDict[blockerId].Start = taskSchedule.ContainsKey(blockerId) ? taskSchedule[blockerId].Item1 : null;
					userBlockersDict[blockerId].DaysCount = taskSchedule.ContainsKey(blockerId) ? taskSchedule[blockerId].Item2 : 0;
				}
			}

			var result = new HashSet<string>();

			if (usersBlockersDict.ContainsKey(currentUser))
			{
				foreach (KeyValuePair<int, BlockerData> pair in usersBlockersDict[currentUser])
				{
					// not a task assigned to this user
					if (!taskSchedule.ContainsKey(pair.Key))
						continue;
					var currentSchedule = taskSchedule[pair.Key];
					if (currentSchedule.Item1 == null)
						continue;
					BlockerData bd = pair.Value;
					if (bd.Start != null)
					{
						int currentScheduledFinish = currentSchedule.Item1.Value + currentSchedule.Item2;
						int oldScheduledFinish = bd.Start.Value + bd.DaysCount;
						if (currentScheduledFinish == oldScheduledFinish)
							continue;
					}
					bd.Start = currentSchedule.Item1;
					bd.DaysCount = currentSchedule.Item2;
					foreach (string blockedUser in pair.Value.BlockedUsers)
					{
						result.Add(blockedUser);
					}
				}
			}

			return result;
		}

		private static int CalculateDaysByRemaining(double remaining)
		{
			return (int)Math.Ceiling(remaining/8/s_workerFocusFactor);
		}

		private static Dictionary<string, List<Tuple<WorkItem, WorkItem>>> SeparateByUser(DataContainer dataContainer)
		{
			var result = new Dictionary<string, List<Tuple<WorkItem, WorkItem>>>();
			foreach (var ltChildrenPair in dataContainer.LeadTaskChildrenDict)
			{
				WorkItem lt = dataContainer.WiDict[ltChildrenPair.Key];
				foreach (int childId in ltChildrenPair.Value)
				{
					WorkItem child = dataContainer.WiDict[childId];
					string assignee = child.AssignedTo();
					var childTuple = new Tuple<WorkItem, WorkItem>(child, lt);
					if (!result.ContainsKey(assignee))
						result.Add(assignee, new List<Tuple<WorkItem, WorkItem>> { childTuple });
					else
						result[assignee].Add(childTuple);
				}
			}
			return result;
		}

		private static Dictionary<string, HashSet<int>> ScheduleUserTasks(
			IEnumerable<Tuple<WorkItem, WorkItem>> userTasks,
			string user,
			FreeDaysCalculator freeDaysCalculator,
			DataContainer dataContainer,
			Dictionary<int, Tuple<int?, int>> scheduledTasksDict)
		{
			var nonBlockedTasks = new List<Tuple<WorkItem, WorkItem>>();
			var activeBlockedTasks = new List<Tuple<WorkItem, WorkItem>>();
			var proposedBlockedTasks = new List<Tuple<WorkItem, WorkItem>>();
			foreach (Tuple<WorkItem, WorkItem> tuple in userTasks)
			{
				if (tuple.Item1.IsActive())
				{
					if (!dataContainer.BlockersDict.ContainsKey(tuple.Item1.Id))
						nonBlockedTasks.Add(tuple);
					else
						activeBlockedTasks.Add(tuple);
				}
				else if (tuple.Item1.IsProposed())
				{
					if (!dataContainer.BlockersDict.ContainsKey(tuple.Item1.Id))
						nonBlockedTasks.Add(tuple);
					else
						proposedBlockedTasks.Add(tuple);
				}
			}

			var schedule = AppendNonBlockedTasks(
				nonBlockedTasks,
				user,
				scheduledTasksDict,
				freeDaysCalculator);

			var result = new Dictionary<string, HashSet<int>>();

			AppendBlockedTasks(
				proposedBlockedTasks,
				schedule,
				dataContainer,
				scheduledTasksDict,
				result);
			AppendBlockedTasks(
				activeBlockedTasks,
				schedule,
				dataContainer,
				scheduledTasksDict,
				result);

			int currentDay = 0;
			foreach (var pair in schedule)
			{
				bool isTaskActive = pair.Item1.Item1.IsActive();
				scheduledTasksDict[pair.Item1.Item1.Id] = new Tuple<int?, int>(isTaskActive ? 0 : currentDay, pair.Item2);
				currentDay += isTaskActive ? Math.Max(pair.Item2 - currentDay, 0) : pair.Item2;
			}

			return result;
		}

		private static int? MaxDay(int? first, int? second)
		{
			if (first == null)
				return second;
			if (second == null)
				return first;
			return first.Value >= second.Value ? first : second;
		}

		private static int? GetFinishDay(WorkItem task, Dictionary<int, Tuple<int?, int>> scheduledTasksDict)
		{
			int? result = null;
			if (scheduledTasksDict.ContainsKey(task.Id))
			{
				var blockerSchedule = scheduledTasksDict[task.Id];
				if (blockerSchedule.Item1 != null)
					result = blockerSchedule.Item1.Value + blockerSchedule.Item2;
			}
			else
			{
				double? remaining = task.IsProposed()
					? task.Estimate()
					: task.Remaining();
				if (remaining != null)
					result = CalculateDaysByRemaining(remaining.Value);
			}
			return result;
		}

		private static Tuple<int?, Dictionary<string, HashSet<int>>> GetFinishDateForBlockedTask(
			WorkItem blockedTask,
			DataContainer dataContainer,
			Dictionary<int, Tuple<int?, int>> scheduledTasksDict)
		{
			int? finish = GetFinishDay(blockedTask, scheduledTasksDict);

			Dictionary<string, HashSet<int>> userBlockersDict;

			if (dataContainer.BlockersDict.ContainsKey(blockedTask.Id))
			{
				userBlockersDict = new Dictionary<string, HashSet<int>>();
				foreach (int blockerId in dataContainer.BlockersDict[blockedTask.Id])
				{
					WorkItem blocker = dataContainer.WiDict[blockerId];
					var blockerData = GetFinishDateForBlockedTask(
						blocker,
						dataContainer,
						scheduledTasksDict);

					int? currentFinish = GetFinishDay(blocker, scheduledTasksDict);
					currentFinish = MaxDay(currentFinish, blockerData.Item1);
					finish = MaxDay(finish, currentFinish);

					foreach (var blockerAssignData in blockerData.Item2)
					{
						if (!userBlockersDict.ContainsKey(blockerAssignData.Key))
							userBlockersDict.Add(blockerAssignData.Key, new HashSet<int>());
						var ids = userBlockersDict[blockerAssignData.Key];
						foreach (int parentBlockerId in blockerAssignData.Value)
						{
							ids.Add(parentBlockerId);
						}
					}

					string blockerAssignee = blocker.AssignedTo();
					if (userBlockersDict.ContainsKey(blockerAssignee))
						userBlockersDict[blockerAssignee].Add(blockerId);
					else
						userBlockersDict.Add(blockerAssignee, new HashSet<int> {blockerId});
				}
			}
			else
			{
				userBlockersDict = new Dictionary<string, HashSet<int>>(0);
			}

			var result = new Tuple<int?, Dictionary<string, HashSet<int>>>(finish, userBlockersDict);

			return result;
		}

		private static void AppendBlockedTasks(
			IEnumerable<Tuple<WorkItem, WorkItem>> blockedTasks,
			List<Tuple<Tuple<WorkItem, WorkItem>, int>> schedule,
			DataContainer dataContainer,
			Dictionary<int, Tuple<int?, int>> scheduledTasksDict,
			Dictionary<string, HashSet<int>> usersBlockers)
		{
			var comparer = new TaskPriorityComparer();
			foreach (var tuple in blockedTasks)
			{
				WorkItem blockedTask = tuple.Item1;
				var finishData = GetFinishDateForBlockedTask(
					blockedTask,
					dataContainer,
					scheduledTasksDict);
				if (finishData.Item1 == null)
				{
					schedule.Add(new Tuple<Tuple<WorkItem, WorkItem>, int>(tuple, 0));
				}
				else
				{
					double? remaining = tuple.Item1.IsActive()
						? blockedTask.Remaining()
						: blockedTask.Estimate();
					int taskDaysCount = remaining == null ? 0 : CalculateDaysByRemaining(remaining.Value);
					int startDay = 0;
					bool added = false;
					for (int i = 0; i < schedule.Count; i++)
					{
						var taskData = schedule[i];
						if (taskData.Item1.Item1.IsActive())
							continue;
						if (startDay > finishData.Item1.Value && comparer.Compare(tuple, taskData.Item1) < 0)
						{
							schedule.Insert(i, new Tuple<Tuple<WorkItem, WorkItem>, int>(tuple, taskDaysCount));
							added = true;
							break;
						}
						startDay += taskData.Item1.Item1.IsActive()
							? Math.Max(taskData.Item2 - startDay, 0)
							: taskData.Item2;
					}
					if (!added)
						schedule.Add(new Tuple<Tuple<WorkItem, WorkItem>, int>(tuple, taskDaysCount));
				}

				foreach (var userBlockersPair in finishData.Item2)
				{
					HashSet<int> blockers;
					if (usersBlockers.ContainsKey(userBlockersPair.Key))
					{
						blockers = usersBlockers[userBlockersPair.Key];
					}
					else
					{
						blockers = new HashSet<int>();
						usersBlockers.Add(userBlockersPair.Key, blockers);
					}
					foreach (int blockerId in userBlockersPair.Value)
					{
						blockers.Add(blockerId);
					}
				}
			}
		}

		private static List<Tuple<Tuple<WorkItem, WorkItem>, int>> AppendNonBlockedTasks(
			ICollection<Tuple<WorkItem, WorkItem>> nonBlockedTasks,
			string user,
			Dictionary<int, Tuple<int?, int>> scheduledTasksDict,
			FreeDaysCalculator freeDaysCalculator)
		{
			var result = new List<Tuple<Tuple<WorkItem, WorkItem>, int>>(nonBlockedTasks.Count);
			result.AddRange(
				nonBlockedTasks
					.OrderBy(i =>
						i, new TaskPriorityComparer())
					.Select(pair =>
						new Tuple<Tuple<WorkItem, WorkItem>, int>(
							pair,
							GetDaysCount(
								pair.Item1,
								user,
								scheduledTasksDict,
								freeDaysCalculator))));
			return result;
		}

		private static int GetDaysCount(
			WorkItem task,
			string user,
			Dictionary<int, Tuple<int?, int>> scheduledTasksDict,
			FreeDaysCalculator freeDaysCalculator)
		{
			if (task.IsActive())
			{
				DateTime? finishDate = task.FinishDate();
				int finish = scheduledTasksDict.ContainsKey(task.Id)
					? scheduledTasksDict[task.Id].Item2
					: (finishDate == null
						? 0
						: freeDaysCalculator.GetDaysCount(finishDate.Value, user));
				double? remaining = task.Remaining();
				if (remaining != null && remaining > 0)
				{
					int finishByRemaining = CalculateDaysByRemaining(remaining.Value);
					if (finish < finishByRemaining)
						finish = finishByRemaining;
				}
				return finish;
			}
			double? estimate = task.Estimate();
			return estimate == null
				? 0
				: CalculateDaysByRemaining(estimate.Value);
		}
	}
}
