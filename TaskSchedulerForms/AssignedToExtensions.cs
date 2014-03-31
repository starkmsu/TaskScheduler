using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Parsers;

namespace TaskSchedulerForms
{
	internal static class AssignedToExtensions
	{
		private const string s_groupPrefix = "g ";

		internal static bool IsUnassigned(this WorkItem workItem)
		{
			return workItem.AssignedTo().StartsWith(s_groupPrefix);
		}

		internal static bool IsUnassigned(this string workItemAssignation)
		{
			return workItemAssignation.StartsWith(s_groupPrefix);
		}
	}
}
