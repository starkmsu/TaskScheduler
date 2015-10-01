namespace TaskSchedulerForms.Const
{
	internal class Messages
	{
		internal static string ChildTaskHasLaterFd()
		{
			return "Some Child Task will not be finished before current Finish Date";
		}

		internal static string ActiveIsBlocked(string blockerIds)
		{
			return blockerIds + " - Active WI is blocked";
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

		internal static string NonChildBlocker(int blockerId)
		{
			return blockerId + " - Non child blocker";
		}

		internal static string ProposedLeadTaskHasNotProposedChild()
		{
			return "LeadTask in state Proposed has Child in non Proposed state";
		}

		internal static string BadHlaAgreemntState(string state)
		{
			return "HLA agreement state is " + state;
		}

		internal static string BadVisionAgreemntState(string state)
		{
			return "Vision agreement state is " + state;
		}

		internal static string LeadTaskHasNoChildren()
		{
			return "LeadTask has no child tasks";
		}

		internal static string TaskHasPriorityLowerThanLeadTask()
		{
			return "Task has priority lower than LeadTask";
		}

		internal static string TaskHasDifferentIteration()
		{
			return "Task has different iteration";
		}
	}
}
