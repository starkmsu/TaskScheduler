using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Helpers
{
	internal static class AssignedToExtensions
	{
		private const string s_groupPrefix = "g ";

		internal static bool IsUnassigned(this WorkItem workItem)
		{
			string assignee = workItem.AssignedTo();
			return string.IsNullOrEmpty(assignee) || assignee.StartsWith(s_groupPrefix);
		}

		internal static bool IsUnassigned(this string assignee)
		{
			return string.IsNullOrEmpty(assignee) || assignee.StartsWith(s_groupPrefix);
		}
	}
}
