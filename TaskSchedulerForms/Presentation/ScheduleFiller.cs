using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
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
						userMark);
				}
			}
			else if (taskStart.HasValue)
			{
				var indStart = (int)taskStart.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				if (indStart < 0)
					row.Cells[viewColumnsIndexes.FirstDateColumnIndex - 1].Value = taskStart.Value.ToString("dd.MM");
				indStart = Math.Min(Math.Max(0, indStart), m_maxInd) + viewColumnsIndexes.FirstDateColumnIndex;

				var indFinish = (int)taskFinish.Value.Date.Subtract(DateTime.Now.Date).TotalDays;
				indFinish = Math.Min(Math.Max(0, indFinish), m_maxInd) + viewColumnsIndexes.FirstDateColumnIndex;
				DateTime today = DateTime.Now.Date;
				for (int i = indStart; i <= indFinish; i++)
				{
					DateTime date = today.AddDays(i - viewColumnsIndexes.FirstDateColumnIndex);
					if (ColorCellIfFreeDay(
						freeDaysCalculator,
						row.Cells[i],
						date,
						userMark))
						continue;
					row.Cells[i].Value = userMark;
				}
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
					if (freeDaysCalculator.GetDayType(startDate, userMark) == DayType.WorkDay)
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
				userMark);
		}

		private static int AddDates(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			int startInd,
			int length,
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
				var date = DateTime.Now.AddDays(dateIndexShift);
				var cell = row.Cells[startInd + ind];
				if (ColorCellIfFreeDay(
					freeDaysCalculator,
					cell,
					date,
					userMark))
				{
					++ind;
					continue;
				}
				if (cell.Value == null)
					cell.Value = userMark;
				else
					cell.Value = cell.Value + userMark;
				++ind;
				--length;
			}
			return startInd + ind;
		}

		internal static void ColorVacations(
			ViewColumnsIndexes viewColumnsIndexes,
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewRow row,
			string user)
		{
			List<DateTime> vacations = freeDaysCalculator.GetVacations(user);
			if (vacations == null || vacations.Count == 0)
				return;

			DateTime start = DateTime.Now.Date;
			DateTime finish = DateTime.Now.AddMonths(1).Date;
			if (finish < vacations[0])
				return;
			if (start > vacations[vacations.Count - 1])
				return;
			int ind = viewColumnsIndexes.FirstDateColumnIndex;
			for (DateTime i = start; i <= finish; i = i.AddDays(1).Date)
			{
				if (vacations.Any(d => d == i))
					row.Cells[ind].SetColorByDayType(DayType.Vacations);
				++ind;
			}
		}

		private static bool ColorCellIfFreeDay(
			FreeDaysCalculator freeDaysCalculator,
			DataGridViewCell cell,
			DateTime dateTime,
			string user)
		{
			DayType dt = freeDaysCalculator.GetDayType(dateTime, user);
			cell.SetColorByDayType(dt);
			return dt != DayType.WorkDay;
		}
	}
}
