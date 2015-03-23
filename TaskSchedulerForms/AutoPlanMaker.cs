using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskSchedulerForms
{
	internal class AutoPlanMaker
	{
		internal static Dictionary<int, string> Make(
			Dictionary<string, List<string>> usersToPlanByDiscipline,
			List<Tuple<int, string>> planningAssignments,
			DataContainer data)
		{
			var result = new Dictionary<int, string>(planningAssignments.Count);
			var usersWork = new Dictionary<string, double>();

			foreach (Tuple<int, string> assignment in planningAssignments)
			{
				WorkItem workItem = data.WiDict[assignment.Item1];
				if (!assignment.Item2.IsUnassigned())
				{
					if (!usersWork.ContainsKey(assignment.Item2))
						usersWork.Add(assignment.Item2, 0);
					usersWork[assignment.Item2] += GetWork(workItem);
					continue;
				}
				string discinpline = workItem.Discipline();
				var availableUsers = usersToPlanByDiscipline[discinpline];
				if (availableUsers.Count == 0)
					continue;
				double min = int.MaxValue;
				bool assigned = false;
				string minUser = null;
				foreach (string availableUser in availableUsers)
				{
					if (!usersWork.ContainsKey(availableUser))
					{
						usersWork.Add(availableUser, GetWork(workItem));
						result.Add(assignment.Item1, availableUser);
						assigned = true;
						break;
					}
					if (min <= usersWork[availableUser])
						continue;
					min = usersWork[availableUser];
					minUser = availableUser;
				}
				if (assigned)
					continue;
				if (minUser != null)
				{
					if (assignment.Item2.IsUnassigned())
						result.Add(assignment.Item1, minUser);
					usersWork[minUser] += GetWork(workItem);
				}
			}

			return result;
		}

		private static double GetWork(WorkItem workItem)
		{
			if (workItem.State == WorkItemState.Active)
				return workItem.Remaining().Value;
			return workItem.Estimate() ?? 0;
		}
	}
}
