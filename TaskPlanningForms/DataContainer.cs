using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TaskPlanningForms
{
	internal class DataContainer
	{
		internal Dictionary<int, List<int>> LeadTaskChildrenDict;

		internal Dictionary<int, WorkItem> WiDict;

		internal Dictionary<int, List<int>> BlockersDict;

		internal Dictionary<int, WorkItem> NonChildBlockers;
	}
}
