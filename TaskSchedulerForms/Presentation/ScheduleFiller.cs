using System;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;
using TaskSchedulerForms.Data;
using TaskSchedulerForms.Helpers;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal class ScheduleFiller
	{
		private static readonly int m_maxInd = (int)DateTime.Now.AddMonths(1).Date.Subtract(DateTime.Now.Date).TotalDays;
		private const double m_focusFactor = 0.5f;

		internal static int AddDatesActive(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem workItem,
			DataGridViewRow row,
			int startInd,
			string user,
			string userMark)
		{
			var taskStart = workItem.StartDate();
			var taskFinish = workItem.FinishDate();
			if (taskFinish == null)
				return viewColumnsIndexes.FirstDateColumnIndex;

			if (taskFinish.Value.Date < DateTime.Now.Date)
			{
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskFinish.Value.ToString("dd.MM");

				var verificationResult = WorkItemVerifier.VerifyFinishDate(workItem);
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].SetColorByVerification(verificationResult.Result);
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].ToolTipText = verificationResult.AllMessagesString;

				double? remaining = workItem.Remaining();
				if (remaining.HasValue)
				{
					var length = (int)Math.Ceiling(remaining.Value / 8 / m_focusFactor);
					return AddDates(
						viewColumnsIndexes,
						freeDaysCalculator,
						row,
						startInd,
						length,
						false,
						user,
						userMark);
				}
			}
			else if (taskStart.HasValue)
			{
				var indStart = (int)taskStart.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				if (indStart < 0)
					row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskStart.Value.ToString("dd.MM");
				indStart = Math.Min(Math.Max(1, indStart), m_maxInd) + viewColumnsIndexes.FirstDateColumnIndex;

				var indFinish = (int)taskFinish.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				indFinish = Math.Min(Math.Max(1, indFinish), m_maxInd) + viewColumnsIndexes.FirstDateColumnIndex;

				AddDates(
					viewColumnsIndexes,
					freeDaysCalculator,
					row,
					indStart,
					indFinish - indStart + 1,
					true,
					user,
					userMark);

				return indFinish + 1;
			}
			return viewColumnsIndexes.FirstDateColumnIndex;
		}

		internal static int AddDatesProposed(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			WorkItem task,
			DataGridViewRow taskRow,
			int startInd,
			string user,
			string userMark,
			bool shouldCheckEstimate)
		{
			var verificationResult = WorkItemVerifier.VerifyEstimatePresence(task);
			if (verificationResult.Result != VerificationResult.Ok)
			{
				if (shouldCheckEstimate)
				{
					var estimateCell = taskRow.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1];
					estimateCell.SetColorByVerification(verificationResult.Result);
					estimateCell.ToolTipText = verificationResult.AllMessagesString;
				}
				return viewColumnsIndexes.FirstDateColumnIndex;
			}

			double? estimate = task.Estimate();

			var length = (int)Math.Ceiling(estimate.Value / 8 / m_focusFactor);

			if (task.FinishDate() != null)
			{
				int finishShift = length - 1;
				DateTime startDate = task.FinishDate().Value.Date;
				DateTime today = DateTime.Now.Date;
				while (finishShift > 0 && startDate >= today)
				{
					startDate = startDate.AddDays(-1);
					if (freeDaysCalculator.GetDayType(startDate, user) == DayType.WorkDay)
						--finishShift;
				}
				var startShift = (int)startDate.Subtract(DateTime.Now.Date).TotalDays;
				startInd = Math.Max(startInd, startShift + viewColumnsIndexes.FirstDateColumnIndex);
			}

			return AddDates(
				viewColumnsIndexes,
				freeDaysCalculator,
				taskRow,
				startInd,
				length,
				false,
				user,
				userMark);
		}

		private static int AddDates(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			int startInd,
			int length,
			bool byDates,
			string user,
			string userMark)
		{
			if (startInd - viewColumnsIndexes.FirstDateColumnIndex > m_maxInd)
				return viewColumnsIndexes.FirstDateColumnIndex + m_maxInd + 1;
			int ind = 0;
			while (length > 0)
			{
				int dateIndexShift = startInd - viewColumnsIndexes.FirstDateColumnIndex + ind;
				if (dateIndexShift > m_maxInd)
					return viewColumnsIndexes.FirstDateColumnIndex + m_maxInd + 1;
				var cell = row.Cells[startInd + ind];
				++ind;
				bool isFreeDay = ColorCellIfFreeDay(
					freeDaysCalculator,
					cell,
					dateIndexShift,
					user,
					userMark);
				if (byDates || !isFreeDay)
					--length;
			}
			return startInd + ind;
		}

		internal static void ColorFdOutDays(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			int startIndex,
			int finishIndex)
		{
			DateTime today = DateTime.Now.Date;
			for (int i = startIndex; i < finishIndex; i++)
			{
				DateTime date = today.AddDays(i - viewColumnsIndexes.FirstDateColumnIndex);
				if (freeDaysCalculator.GetDayType(date) != DayType.WorkDay)
					continue;
				row.Cells[i].SetErrorColor();
				row.Cells[i].ToolTipText = Messages.ChildTaskHasLaterFd();
			}
		}

		private static bool ColorCellIfFreeDay(
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewCell cell,
			int dayIndex,
			string user,
			string userMark)
		{
			DateTime date = DateTime.Today.Date.AddDays(dayIndex);
			DayType dt = freeDaysCalculator.GetDayType(date, user);
			if (dt == DayType.WorkDay)
				cell.Value = userMark;
			cell.SetColorByDayType(dt);
			return dt != DayType.WorkDay;
		}
	}
}
