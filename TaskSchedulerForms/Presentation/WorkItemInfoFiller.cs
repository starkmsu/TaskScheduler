using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Properties;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal class WorkItemInfoFiller
	{
		private readonly DataGridView m_dataGridView;
		private readonly ViewColumnsIndexes m_viewColumnsIndexes;

		private const string m_blockersPrefix = "-->";
		private const char m_iterationSeparator = '\\';

		internal WorkItemInfoFiller(DataGridView dataGridView, ViewColumnsIndexes viewColumnsIndexes)
		{
			m_dataGridView = dataGridView;
			m_viewColumnsIndexes = viewColumnsIndexes;
		}

		internal bool FillLeadTaskInfo(
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItem leadTask,
			DataGridViewRow leadTaskRow,
			DataContainer data,
			List<int> blockersIds)
		{
			var priorityCell = leadTaskRow.Cells[m_viewColumnsIndexes.PriorityColumnIndex];
			priorityCell.Value = leadTask.Priority();
			priorityCell.SetColorByState(leadTask);
			priorityCell.ToolTipText = leadTask.IsDevCompleted() ? WorkItemState.DevCompleted : leadTask.State;

			var iterationCell = leadTaskRow.Cells[m_viewColumnsIndexes.IterationColumnIndex];
			string iteration = leadTask.IterationPath;
			int ind = iteration.IndexOf(m_iterationSeparator);
			if (ind != -1)
				iteration = iteration.Substring(ind + 1);
			iterationCell.Value = iteration;

			var idCell = leadTaskRow.Cells[m_viewColumnsIndexes.LeadTaskColumnIndex];
			idCell.Value = leadTask.Id;
			idCell.ToolTipText = leadTask.IterationPath;
			idCell.Style.BackColor = priorityCell.Style.BackColor;
			var verificationResult = WorkItemVerifier.VerifyChildrenExistance(leadTask, data);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				idCell.SetColorByVerification(verificationResult.Result);
				idCell.ToolTipText += Environment.NewLine + verificationResult.AllMessagesString;
			}

			var docsCell = leadTaskRow.Cells[m_viewColumnsIndexes.DocsColumnIndex];
			bool result;
			verificationResult = WorkItemVerifier.VerifyDocumentsAgreement(leadTask);
			if (verificationResult.Result == VerificationResult.Ok)
			{
				docsCell.Style.BackColor = priorityCell.Style.BackColor;
				result = true;
			}
			else
			{
				docsCell.Value = verificationResult.AddidtionalData;
				docsCell.SetColorByVerification(verificationResult.Result);
				docsCell.ToolTipText = verificationResult.AllMessagesString;
				result = false;
			}

			var titleCell = leadTaskRow.Cells[m_viewColumnsIndexes.TitleColumnIndex];
			titleCell.Value = leadTask.Title;
			titleCell.Style.Font = new Font(
				titleCell.Style.Font
					?? titleCell.OwningColumn.DefaultCellStyle.Font
					?? m_dataGridView.ColumnHeadersDefaultCellStyle.Font,
				FontStyle.Underline);
			titleCell.Style.BackColor = priorityCell.Style.BackColor;

			verificationResult = WorkItemVerifier.VerifyNoProposedChildTaks(leadTask, data);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				priorityCell.SetColorByVerification(verificationResult.Result);
				priorityCell.ToolTipText = verificationResult.AllMessagesString;
			}

			var blockersCell = leadTaskRow.Cells[m_viewColumnsIndexes.BlockersColumnIndex];
			verificationResult = WorkItemVerifier.VerifyBlockersExistance(blockersIds);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				blockersCell.SetColorByVerification(verificationResult.Result);
				blockersCell.Value = verificationResult.AllMessagesString;
			}
			if (!string.IsNullOrEmpty(leadTask.BlockingReason()))
			{
				if (!string.IsNullOrEmpty(leadTaskRow.Cells[4].ToolTipText))
					blockersCell.ToolTipText += Environment.NewLine;
				else
					blockersCell.Value = leadTask.BlockingReason();
				blockersCell.ToolTipText += "Blocking Reason: " + leadTask.BlockingReason();
			}

			leadTaskRow.Cells[m_viewColumnsIndexes.AssignedToColumnIndex].Value = leadTask.AssignedTo();

			return result;
		}

		internal void FillTaskInfo(
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItem task,
			List<WorkItem> siblings,
			int? leadTaskPriority,
			DataGridViewRow taskRow,
			DataContainer data,
			List<int> blockerIds)
		{
			var priorityCell = taskRow.Cells[m_viewColumnsIndexes.PriorityColumnIndex];
			priorityCell.Value = task.Priority();
			priorityCell.SetColorByState(task);
			priorityCell.ToolTipText = task.State;

			var verificationResult = WorkItemVerifier.VerifyTaskPriority(task, leadTaskPriority);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				var priorityWarningCell = taskRow.Cells[m_viewColumnsIndexes.LeadTaskColumnIndex];
				priorityWarningCell.SetColorByVerification(verificationResult.Result);
				priorityWarningCell.ToolTipText = verificationResult.AllMessagesString;
			}

			var titleCell = taskRow.Cells[m_viewColumnsIndexes.TitleColumnIndex];
			titleCell.Value = task.Id + " " + task.Title;
			titleCell.ToolTipText =
				task.Discipline() + " "
				+ task.Title + " "
				+ (task.State == WorkItemState.Active
					? "Remaining " + task.Remaining()
					: "Estimate " + task.Estimate());
			titleCell.Style.BackColor = priorityCell.Style.BackColor;

			var blockersCell = taskRow.Cells[m_viewColumnsIndexes.BlockersColumnIndex];
			if (blockerIds != null)
			{
				string blockerIdsStr = string.Join(",", blockerIds);
				blockersCell.Value = blockerIdsStr;

				verificationResult = WorkItemVerifier.VerifyNonChildBlockerExistance(blockerIds, siblings);
				if (verificationResult.Result == VerificationResult.Ok)
					verificationResult = WorkItemVerifier.VerifyActiveTaskBlocking(task, blockerIds);
				if (verificationResult.Result == VerificationResult.Ok)
				{
					blockerIdsStr = string.Join(Environment.NewLine, blockerIds.Select(b => data.WiDict[b].Title));
					blockersCell.ToolTipText = blockerIdsStr;
				}
				else
				{
					blockersCell.SetColorByVerification(verificationResult.Result);
					blockersCell.ToolTipText = verificationResult.AllMessagesString;
				}
			}
			if (!string.IsNullOrEmpty(task.BlockingReason()))
			{
				if (!string.IsNullOrEmpty(blockersCell.ToolTipText))
					blockersCell.ToolTipText += Environment.NewLine;
				else
					blockersCell.Value = task.BlockingReason();
				blockersCell.ToolTipText += "Blocking Reason: " + task.BlockingReason();
			}

			var assignedCell = taskRow.Cells[m_viewColumnsIndexes.AssignedToColumnIndex];
			assignedCell.Value = task.AssignedTo();
			verificationResult = WorkItemVerifier.VerifyAssignation(task);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				
				assignedCell.SetColorByVerification(verificationResult.Result);
				assignedCell.ToolTipText = verificationResult.AllMessagesString;
			}
		}

		internal void FillNotAccessibleTaskInfo(
			ViewColumnsIndexes viewColumnsIndexes,
			DataGridViewRow taskRow,
			int notAccessableChildId)
		{
			taskRow.Cells[viewColumnsIndexes.TitleColumnIndex].Value = notAccessableChildId;
			taskRow.Cells[viewColumnsIndexes.AssignedToColumnIndex].Value = Resources.AccessDenied;
		}

		internal void FillBlockerInfo(DataGridViewRow blockerRow, WorkItem blocker)
		{
			blockerRow.Cells[m_viewColumnsIndexes.LeadTaskColumnIndex].Value = m_blockersPrefix;
			blockerRow.Cells[m_viewColumnsIndexes.DocsColumnIndex].Value = blocker.Type.Name;
			blockerRow.Cells[m_viewColumnsIndexes.TitleColumnIndex].Value = blocker.Id + " " + blocker.Title;
			blockerRow.Cells[m_viewColumnsIndexes.BlockersColumnIndex].Value = blocker.State;
			blockerRow.Cells[m_viewColumnsIndexes.AssignedToColumnIndex].Value = blocker.AssignedTo();
			blockerRow.Visible = false;
		}
	}
}
