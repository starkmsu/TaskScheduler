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
				Messages = new List<string>(1) { Messages.LeadTaskHasNoChildren() },
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
				Messages = new List<string>(1) { Messages.TaskHasPriorityLowerThanLeadTask() },
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

		internal static WorkItemVerificationResult VerifyNonChildBlockerExistance(List<int> blockersIds, List<WorkItem> siblings)
		{
			int nonChildBlockerId = blockersIds.FirstOrDefault(i => siblings.All(s => s.Id != i));
			if (nonChildBlockerId > 0)
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { Messages.NonChildBlocker(nonChildBlockerId) },
				};
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}

		internal static WorkItemVerificationResult VerifyBlockersExistance(List<int> blockersIds)
		{
			if (blockersIds != null && blockersIds.Count > 0)
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { string.Join(",", blockersIds) },
				};
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}

		internal static WorkItemVerificationResult VerifyActiveTaskBlocking(WorkItem workItem, List<int> blockersIds)
		{
			if (workItem.IsActive() && blockersIds != null && blockersIds.Count > 0)
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

		internal static WorkItemVerificationResult VerifyNoProposedChildTask(WorkItem leadTask, DataContainer dataContainer)
		{
			if ((leadTask.IsProposed())
				&& dataContainer.LeadTaskChildrenDict.ContainsKey(leadTask.Id))
			{
				foreach (int childTaskId in dataContainer.LeadTaskChildrenDict[leadTask.Id])
				{
					if (!dataContainer.WiDict.ContainsKey(childTaskId))
						continue;
					WorkItem task = dataContainer.WiDict[childTaskId];
					if (!task.IsProposed())
						return new WorkItemVerificationResult
						{
							Result = VerificationResult.Error,
							Messages = new List<string>(1) {Messages.ProposedLeadTaskHasNotProposedChild()},
						};
				}
			}
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}

		internal static WorkItemVerificationResult VerifyTaskWithParentOnSameIteration(WorkItem task, WorkItem leadTask)
		{
			if (task.IterationPath != leadTask.IterationPath)
				return new WorkItemVerificationResult
				{
					Result = VerificationResult.Error,
					Messages = new List<string>(1) { Messages.TaskHasDifferentIteration() },
				};
			return new WorkItemVerificationResult { Result = VerificationResult.Ok };
		}
	}
}
