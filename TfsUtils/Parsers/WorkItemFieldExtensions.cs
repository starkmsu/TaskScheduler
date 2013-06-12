using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtils.Parsers
{
	public static class WorkItemFieldExtensions
	{
		public static string Discipline(this WorkItem workItem)
		{
			return GetStringValue(workItem, "Discipline");
		}

		public static string AssignedTo(this WorkItem workItem)
		{
			return GetStringValue(workItem, "Assigned To");
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
			return GetStringValue(workItem, "Dev Completed Agreed").Length > 0;
		}

		public static string HlaAgreementState(this WorkItem workItem)
		{
			return GetStringValue(workItem, "HLA Agreed");
		}

		private static string GetStringValue(WorkItem workItem, string fieldName)
		{
			if (!workItem.Fields.Contains(fieldName))
				return string.Empty;

			object fieldObj = workItem[fieldName];
			if (fieldObj == null)
				return string.Empty;

			return fieldObj.ToString();
		}
	}
}
