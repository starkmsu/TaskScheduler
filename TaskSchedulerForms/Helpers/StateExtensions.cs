using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Const;

namespace TaskSchedulerForms.Helpers
{
	internal static class StateExtensions
	{
		internal static bool IsProposed(this WorkItem workItem)
		{
			return workItem.State == WorkItemState.Proposed
				|| workItem.State == WorkItemState.ToDo;
		}

		internal static bool IsActive(this WorkItem workItem)
		{
			return workItem.State == WorkItemState.Active;
		}

		internal static bool IsResolved(this WorkItem workItem)
		{
			return workItem.State == WorkItemState.Resolved
				|| workItem.State == WorkItemState.Done;
		}

		internal static bool IsClosed(this WorkItem workItem)
		{
			return workItem.State == WorkItemState.Closed;
		}
	}
}
