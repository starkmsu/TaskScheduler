using System;
using System.Collections.Generic;

namespace TaskPlanningForms
{
	[Serializable]
	public class VacationData
	{
		public string User { get; set; }

		public List<DateTime> VacationDays { get; set; }
	}
}
