using System;
using System.Collections.Generic;
using System.Linq;
using TaskSchedulerForms.Config;
using TaskSchedulerForms.Data;

namespace TaskSchedulerForms.Helpers
{
	internal class FreeDaysCalculator
	{
		private Dictionary<string, List<DateTime>> m_vacations = new Dictionary<string, List<DateTime>>(0);
		private List<DateTime> m_holidays = new List<DateTime>(0);

		internal void SetHolidays(List<DateTime> holidays)
		{
			m_holidays = holidays ?? new List<DateTime>(0);
		}

		internal void SetVacations(List<VacationData> vacations)
		{
			m_vacations = new Dictionary<string, List<DateTime>>();
			DateTime today = DateTime.Now.Date;
			foreach (VacationData vacation in vacations)
			{
				m_vacations.Add(
					vacation.User,
					vacation.VacationDays.Where(h => h.Date >= today).ToList());
			}
		}

		internal List<DateTime> GetVacations(string user)
		{
			List<DateTime> result;
			if (m_vacations.TryGetValue(user, out result))
				return result;
			return null;
		}

		internal DayType GetDayType(DateTime dateTime)
		{
			var date = dateTime.Date;
			var dayOfWeek = date.DayOfWeek;
			if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
				return DayType.WeekEnd;
			if (m_holidays.Count == 0)
				return DayType.WorkDay;
			return m_holidays.Contains(date) ? DayType.Holiday : DayType.WorkDay;
		}

		internal DayType GetDayType(DateTime dateTime, string user)
		{
			var date = dateTime.Date;
			var dayOfWeek = date.DayOfWeek;
			if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
				return DayType.WeekEnd;
			if (m_holidays.Contains(date))
				return DayType.Holiday;
			if (m_vacations != null
				&& m_vacations.ContainsKey(user)
				&& m_vacations[user].Any(v => v == date))
				return DayType.Vacations;
			return DayType.WorkDay;
		}

		internal int GetDaysCount(DateTime date, string user)
		{
			List<DateTime> vacations;
			if (!m_vacations.TryGetValue(user, out vacations))
				vacations = new List<DateTime>(0);

			date = date.Date;
			DateTime day = DateTime.Now.Date;
			int result = 0;
			while (day <= date)
			{
				var dayOfWeek = day.DayOfWeek;
				if (dayOfWeek != DayOfWeek.Saturday
					&& dayOfWeek != DayOfWeek.Sunday
					&& !m_holidays.Contains(day)
					&& !vacations.Contains(day))
					++result;
				day = day.AddDays(1);
			}

			return result;
		}
	}
}
