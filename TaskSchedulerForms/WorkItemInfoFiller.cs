using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Properties;
using TfsUtils.Const;
using TfsUtils.Parsers;

namespace TaskSchedulerForms
{
	internal class WorkItemInfoFiller
	{
		private readonly DataGridView m_dataGridView;
		private readonly ViewColumnsIndexes m_viewColumnsIndexes;

		private const string m_groupPrefix = "g ";
		private const string m_blockersPrefix = "-->";

		internal string GroupPrefix { get { return m_groupPrefix; } }

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
			var priorityCell = leadTaskRow.Cells[0];
			priorityCell.Value = leadTask.Priority();
			priorityCell.SetColorByState(leadTask);
			priorityCell.ToolTipText = leadTask.IsDevCompleted() ? WorkItemState.DevCompleted : leadTask.State;

			var idCell = leadTaskRow.Cells[m_viewColumnsIndexes.LeadTaskColumnIndex];
			idCell.Value = leadTask.Id;
			idCell.ToolTipText = leadTask.IterationPath;
			idCell.Style.BackColor = priorityCell.Style.BackColor;
			if (!data.LeadTaskChildrenDict.ContainsKey(leadTask.Id) || data.LeadTaskChildrenDict[leadTask.Id].Count == 0)
			{
				idCell.SetWarningColor();
				idCell.ToolTipText += Environment.NewLine + Messages.LTHasNoChildren();
			}

			var docsCell = leadTaskRow.Cells[m_viewColumnsIndexes.DocsColumnIndex];
			bool result = true;
			string visionAgreementState = leadTask.VisionAgreementState();
			string hlaAgeementState = leadTask.HlaAgreementState();
			if (visionAgreementState == DocumentAgreementState.No || visionAgreementState == DocumentAgreementState.Waiting)
			{
				docsCell.Value = visionAgreementState;
				docsCell.SetErrorColor();
				docsCell.ToolTipText = Messages.BadVisionAgreemntState(visionAgreementState);
				result = false;
			}
			else if (hlaAgeementState == DocumentAgreementState.No || hlaAgeementState == DocumentAgreementState.Waiting)
			{
				docsCell.Value = hlaAgeementState;
				docsCell.SetErrorColor();
				docsCell.ToolTipText = Messages.BadHlaAgreemntState(hlaAgeementState);
				result = false;
			}
			else
			{
				docsCell.Style.BackColor = priorityCell.Style.BackColor;
			}

			var titleCell = leadTaskRow.Cells[m_viewColumnsIndexes.TitleColumnIndex];
			titleCell.Value = leadTask.Title;
			titleCell.Style.Font = new Font(
				titleCell.Style.Font
					?? titleCell.OwningColumn.DefaultCellStyle.Font
					?? m_dataGridView.ColumnHeadersDefaultCellStyle.Font,
				FontStyle.Underline);
			titleCell.Style.BackColor = priorityCell.Style.BackColor;

			var blockersCell = leadTaskRow.Cells[m_viewColumnsIndexes.BlockersColumnIndex];
			if (blockersIds != null)
			{
				string blockerIdsStr = string.Join(",", blockersIds);
				blockersCell.Value = blockerIdsStr;
				int nonChildBlockerId = blockersIds.FirstOrDefault(data.NonChildBlockers.ContainsKey);
				if (nonChildBlockerId > 0)
				{
					blockersCell.SetErrorColor();
					blockersCell.ToolTipText = Messages.NonChildBlocker(nonChildBlockerId);
				}
				else
				{
					blockerIdsStr = string.Join(Environment.NewLine, blockersIds.Select(b => data.WiDict[b].Title));
					blockersCell.ToolTipText = blockerIdsStr;
				}
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

		internal string FillTaskInfo(
			ViewFiltersBuilder viewFiltersBuilder,
			WorkItem task,
			int? leadTaskPriority,
			DataGridViewRow taskRow,
			DataContainer data,
			List<int> blockerIds)
		{
			var priorityCell = taskRow.Cells[m_viewColumnsIndexes.PriorityColumnIndex];
			priorityCell.Value = task.Priority();
			priorityCell.SetColorByState(task);
			priorityCell.ToolTipText = task.State;

			if (leadTaskPriority.HasValue && task.Priority() > leadTaskPriority.Value)
			{
				var priorityWarningCell = taskRow.Cells[m_viewColumnsIndexes.LeadTaskColumnIndex];
				priorityWarningCell.SetWarningColor();
				priorityWarningCell.ToolTipText = Messages.TaskHasPriorityLowerThanLT();
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
				int nonChildBlockerId = blockerIds.FirstOrDefault(data.NonChildBlockers.ContainsKey);
				if (nonChildBlockerId > 0)
				{
					blockersCell.SetErrorColor();
					blockersCell.ToolTipText = Messages.NonChildBlocker(nonChildBlockerId);
				}
				else if (task.State == WorkItemState.Active)
				{
					blockersCell.SetErrorColor();
					blockersCell.ToolTipText = Messages.ActiveIsBlocked(blockerIdsStr);
				}
				else
				{
					blockerIdsStr = string.Join(Environment.NewLine, blockerIds.Select(b => data.WiDict[b].Title));
					blockersCell.ToolTipText = blockerIdsStr;
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
			string assignedTo = task.AssignedTo();
			assignedCell.Value = assignedTo.Length > 0 ? assignedTo : Resources.Nobody;
			if (assignedTo.StartsWith(m_groupPrefix))
			{
				assignedCell.SetWarningColor();
				assignedCell.ToolTipText = Messages.TaskIsNotAssigned();
			}

			return assignedTo;
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
