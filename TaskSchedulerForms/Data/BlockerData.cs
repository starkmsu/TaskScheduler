using System.Collections.Generic;

namespace TaskSchedulerForms.Data
{
	internal class BlockerData
	{
		internal int? Start { get; set; }

		internal int DaysCount { get; set; }

		internal HashSet<string> BlockedUsers { get; set; }
	}
}
