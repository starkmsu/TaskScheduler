using System;
using System.Collections.Generic;

namespace TaskSchedulerForms
{
	internal class LeadTaskData
	{
		internal List<int> Children { get; set; }

		internal DateTime? ScheduledFinishDate { get; set; }
	}
}
