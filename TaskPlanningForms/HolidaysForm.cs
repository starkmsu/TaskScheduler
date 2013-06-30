using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskPlanningForms
{
	public partial class HolidaysForm : Form
	{
		private readonly List<DateTime> m_holidays;

		public List<DateTime> Holidays
		{
			get { return new List<DateTime>(m_holidays); }
		}

		public HolidaysForm(List<DateTime> holidays, string user)
		{
			m_holidays = holidays;

			InitializeComponent();

			if (!string.IsNullOrEmpty(user))
				Text = user + " Holidays";

			holidaysListBox.DataSource = m_holidays;
		}

		private void AddHolidayButtonClick(object sender, EventArgs e)
		{
			DateTime start = holidaysCalendar.SelectionStart;
			DateTime finish = holidaysCalendar.SelectionEnd;
			for (DateTime i = start; i <= finish; i = i.AddDays(1))
			{
				if (m_holidays.Contains(i))
					continue;
				m_holidays.Add(i);
			}
			m_holidays.Sort();
			holidaysListBox.DataSource = null;
			holidaysListBox.DataSource = m_holidays;
		}

		private void DeleteHolidaysButtonClick(object sender, EventArgs e)
		{
			foreach (object item in holidaysListBox.SelectedItems)
			{
				var date = (DateTime) item;
				m_holidays.Remove(date);
			}
			holidaysListBox.DataSource = null;
			holidaysListBox.DataSource = m_holidays;
		}
	}
}
