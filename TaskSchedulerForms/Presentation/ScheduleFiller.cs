using System;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;
using TaskSchedulerForms.Data;
using TfsUtils.Parsers;

namespace TaskSchedulerForms.Presentation
{
	internal static class ScheduleFiller
	{
		internal static int AddDatesActive(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			FocusFactorCalculator focusFactorCalculator,
			WorkItem workItem,
			DataGridViewRow row,
			int startInd,
			string user,
			string userMark)
		{
			var taskStart = workItem.StartDate();
			var taskFinish = workItem.FinishDate();
			DateTime today = DateTime.Now.Date;
			if (taskFinish == null || taskFinish.Value.Date < today)
			{
				if (taskFinish != null)
					row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskFinish.Value.ToString("dd.MM");

				var verificationResult = WorkItemVerifier.VerifyFinishDate(workItem);
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].SetColorByVerification(verificationResult.Result);
				row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].ToolTipText = verificationResult.AllMessagesString;

				double? remaining = workItem.Remaining();
				if (remaining != null)
				{
					var length = focusFactorCalculator.CalculateDaysByTime(remaining.Value, user);
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
				int maxInd = row.Cells.Count - viewColumnsIndexes.FirstDateColumnIndex - 1;
				var indStart = (int)taskStart.Value.Date.Subtract(today).TotalDays;
				if (indStart < 0)
					row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskStart.Value.ToString("dd.MM");
				indStart = Math.Min(Math.Max(1, indStart), maxInd) + viewColumnsIndexes.FirstDateColumnIndex;

				var indFinish = (int)taskFinish.Value.Date.Subtract(today).TotalDays;
				indFinish = Math.Min(Math.Max(1, indFinish), maxInd) + viewColumnsIndexes.FirstDateColumnIndex;

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
			FocusFactorCalculator focusFactorCalculator,
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

			var length = focusFactorCalculator.CalculateDaysByTime(estimate.Value, user);
			DateTime? finish = task.FinishDate();
			if (finish != null)
			{
				verificationResult = WorkItemVerifier.VerifyFinishDate(task);
				var estimateCell = taskRow.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1];
				estimateCell.SetColorByVerification(verificationResult.Result);
				estimateCell.ToolTipText = verificationResult.AllMessagesString;
				estimateCell.Value = finish.Value.ToString("dd.MM");

				int finishShift = length - 1;
				DateTime startDate = finish.Value.Date;
				DateTime today = DateTime.Now.Date;
				while (finishShift > 0 && startDate >= today)
				{
					startDate = startDate.AddDays(-1);
					if (freeDaysCalculator.GetDayType(startDate, user) == DayType.WorkDay)
						--finishShift;
				}
				if (finishShift == 0)
				{
					var startShift = (int) startDate.Subtract(today).TotalDays;
					startInd = Math.Max(startInd, startShift + viewColumnsIndexes.FirstDateColumnIndex);
				}
				else
				{
					length -= finishShift;
				}
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
			int maxInd = row.Cells.Count - viewColumnsIndexes.FirstDateColumnIndex - 1;
			if (startInd - viewColumnsIndexes.FirstDateColumnIndex > maxInd)
				return viewColumnsIndexes.FirstDateColumnIndex + maxInd + 1;
			int ind = 0;
			while (length > 0)
			{
				int dateIndexShift = startInd - viewColumnsIndexes.FirstDateColumnIndex + ind;
				if (dateIndexShift > maxInd)
					return viewColumnsIndexes.FirstDateColumnIndex + maxInd + 1;
				var cell = row.Cells[startInd + ind];
				++ind;
				DateTime date = DateTime.Today.Date.AddDays(dateIndexShift);
				DayType dt = freeDaysCalculator.GetDayType(date, user);
				if (dt == DayType.WorkDay)
					cell.Value = userMark;
				cell.SetColorByDayType(dt);
				if (byDates || dt == DayType.WorkDay || dt == DayType.Vacations)
					--length;
			}
			return startInd + ind;
		}

		internal static int AddDatesFromSchedule(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			int startInd,
			int length,
			string user,
			string userMark)
		{
			int maxInd = row.Cells.Count - viewColumnsIndexes.FirstDateColumnIndex - 1;
			if (startInd > maxInd)
				return viewColumnsIndexes.FirstDateColumnIndex + maxInd + 1;
			int ind = 0;
			DateTime date = freeDaysCalculator.GetWorkDayFromCount(startInd);
			DateTime today = DateTime.Now.Date;
			while (length > 0)
			{
				var dayIndex =  (int) Math.Ceiling(date.Subtract(today).TotalDays);
				if (dayIndex > maxInd)
					return viewColumnsIndexes.FirstDateColumnIndex + maxInd + 1;
				var cell = row.Cells[dayIndex + viewColumnsIndexes.FirstDateColumnIndex];
				DayType dt = freeDaysCalculator.GetDayType(date, user);
				if (dt == DayType.WorkDay)
					cell.Value = userMark;
				cell.SetColorByDayType(dt);
				if (dt == DayType.WorkDay || dt == DayType.Vacations)
					--length;

				++ind;
				date = date.AddDays(1);
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
	}
}
