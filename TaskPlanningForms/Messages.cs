namespace TaskPlanningForms
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

		internal static string ProposedLeadTaskHasActiveChild()
		{
			return "LeadTask in state Proposed has Child in Active state";
		}

		internal static string BadHlaAgreemtnState(string state)
		{
			return "HLA agreement state is " + state;
		}
	}
}
