using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Helpers
{
	internal class TaskPriorityComparer : IComparer<Tuple<WorkItem, WorkItem>>
	{
		private const int s_lowestPriority = 999;

		public int Compare(Tuple<WorkItem, WorkItem> x, Tuple<WorkItem, WorkItem> y)
		{
			if ((x.Item1.Priority() ?? s_lowestPriority) < (y.Item1.Priority() ?? s_lowestPriority))
				return -1;
			if ((x.Item1.Priority() ?? s_lowestPriority) > (y.Item1.Priority() ?? s_lowestPriority))
				return 1;
			if ((x.Item2.Priority() ?? s_lowestPriority) < (y.Item2.Priority() ?? s_lowestPriority))
				return -1;
			if ((x.Item2.Priority() ?? s_lowestPriority) > (y.Item2.Priority() ?? s_lowestPriority))
				return 1;
			return string.Compare(x.Item2.IterationPath, y.Item2.IterationPath, StringComparison.Ordinal);
		}
	}
}
