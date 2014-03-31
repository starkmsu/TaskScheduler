using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TfsUtils.Const;
using TfsUtils.Parsers;
using WorkItemType = TfsUtils.Const.WorkItemType;

namespace TaskSchedulerForms
{
	internal class WorkItemVerifier
	{
		internal static WorkItemVerificationResult VerifyFinishDate(WorkItem workItem)
		{
			DateTime? finishDate = workItem.FinishDate();
			if (finishDate == null || finishDate.Value.Date >= DateTime.Now.Date)
				return new WorkItemVerificationResult {Result = VerificationResult.Ok};

			return new WorkItemVerificationResult
			{
				Result = workItem.Type.Name == WorkItemType.LeadTask
					? VerificationResult.Error
					: VerificationResult.Warning,
					Messages = new List<string>(1) {Messages.ExpiredFd()},
			};
		}

		internal static WorkItemVerificationResult VerifyChildrenExistance(WorkItem leadTask, DataContainer dataContainer)
		{
			if (dataContainer.LeadTaskChildrenDict.ContainsKey(leadTask.Id)
				&& dataContainer.LeadTaskChildrenDict[leadTask.Id].Count > 0)
				return new WorkItemVerificationResult {Result = VerificationResult.Ok};

			return new WorkItemVerificationResult
			{
				Result = VerificationResult.Warning,
				Messages = new List<string>(1) { Messages.LTHasNoChildren() },
			};
		}

		internal static WorkItemVerificationResult VerifyTaskPriority(WorkItem task, int? leadTaskPriority)
		{
			int? priority = task.Priority();
			if (leadTaskPriority == null
				|| priority == null
				|| priority.Value <= leadTaskPriority.Value)
				return new WorkItemVerificationResult { Result = VerificationResult.Ok };

			return new WorkItemVerificationResult
			{
				Result = VerificationResult.Warning,
				Messages = new List<string>(1) { Messages.TaskHasPriorityLowerThanLT() },
			};
		}

		internal static WorkItemVerificationResult VerifyAssignation(WorkItem task)
		{
			if (task.IsUnassigned())
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Warning,
					Messages = new List<string>(1) { Messages.TaskIsNotAssigned() },
				};
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}

		internal static WorkItemVerificationResult VerifyDocumentsAgreement(WorkItem leadTask)
		{
			string visionAgreementState = leadTask.VisionAgreementState();
			string hlaAgeementState = leadTask.HlaAgreementState();
			if (visionAgreementState == DocumentAgreementState.No || visionAgreementState == DocumentAgreementState.Waiting)
			{
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { Messages.BadVisionAgreemntState(visionAgreementState) },
					AddidtionalData = visionAgreementState,
				};
			}
			if (hlaAgeementState == DocumentAgreementState.No || hlaAgeementState == DocumentAgreementState.Waiting)
			{
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { Messages.BadHlaAgreemntState(hlaAgeementState) },
					AddidtionalData = hlaAgeementState,
				};
			}
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}

		internal static WorkItemVerificationResult VerifyNonChildBlockerExistance(
			WorkItem workItem,
			List<int> blockersIds,
			DataContainer dataContainer,
			bool checkState)
		{
			int nonChildBlockerId = blockersIds.FirstOrDefault(dataContainer.NonChildBlockers.ContainsKey);
			if (nonChildBlockerId > 0)
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { Messages.NonChildBlocker(nonChildBlockerId) },
				};
			if (checkState && workItem.State == WorkItemState.Active)
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { Messages.ActiveIsBlocked(string.Join(",", blockersIds)) },
				};
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}

		internal static WorkItemVerificationResult VerifyEstimatePresence(WorkItem workItem)
		{
			double? estimate = workItem.Estimate();
			if (estimate != null)
				return new WorkItemVerificationResult { Result = VerificationResult.Ok };
			return new WorkItemVerificationResult
			{
				Result = VerificationResult.Error,
				Messages = new List<string>(1) { Messages.NoEstimate() },
			};
		}

		/*
		private static void UpdateResult(
			WorkItemVerificationResult result,
			VerificationResult currentResult,
			string message)
		{
			if (currentResult == VerificationResult.Ok)
				return;

			switch (result.Result)
			{
				case VerificationResult.Error:
					if (currentResult != VerificationResult.Error)
						break;
					result.Messages.Add(message);
					break;
				case VerificationResult.Warning:
					if (currentResult == VerificationResult.Error)
					{
						result.Result = VerificationResult.Error;
						result.Messages = new List<string> {message};
					}
					else
					{
						result.Messages.Add(message);
					}
					break;
				default:
					result.Result = currentResult;
					result.Messages = new List<string> { message };
					break;
			}
		}
		*/
	}
}
