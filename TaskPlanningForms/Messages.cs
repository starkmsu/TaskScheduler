namespace TaskPlanningForms
{
	internal class Messages
	{
		internal static string ChildTaskHasLaterFd()
		{
			return "Some Child Task will not be finished before current Finish Date";
		}

		internal static string ActiveIsBlocked()
		{
			return "Active WI is blocked";
		}

		internal static string TaskIsNotAssigned()
		{
			return "Task is not assigned";
		}

		internal static string ExpiredFd()
		{
			return "Expired Finish Date";
		}

		internal static string NoEstimate()
		{
			return "Estimate not present";
		}
	}
}
