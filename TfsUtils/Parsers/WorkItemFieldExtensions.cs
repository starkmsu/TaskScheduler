using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtils.Parsers
{
	public static class WorkItemFieldExtensions
	{
		public static string Discipline(this WorkItem workItem)
		{
			return workItem["Discipline"].ToString();
		}

		public static string AssignedTo(this WorkItem workItem)
		{
			return workItem["Assigned To"].ToString();
		}

		public static int Priority(this WorkItem workItem)
		{
			return (int)workItem["Priority"];
		}

		public static DateTime? StartDate(this WorkItem workItem)
		{
			return workItem["Start Date"] as DateTime?;
		}

		public static DateTime? FinishDate(this WorkItem workItem)
		{
			return workItem["Finish Date"] as DateTime?;
		}

		public static double? Estimate(this WorkItem workItem)
		{
			return workItem["Estimate"] as double?;
		}

		public static double? Remaining(this WorkItem workItem)
		{
			return workItem["Remaining Work"] as double?;
		}

		public static bool IsDevCompleted(this WorkItem workItem)
		{
			return workItem["Dev Completed Agreed"].ToString().Length > 0;
		}
	}
}
