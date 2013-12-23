using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSchedulerForms
{
	[Serializable]
	public class VacationData : IEquatable<VacationData>
	{
		public string User { get; set; }

		public List<DateTime> VacationDays { get; set; }

		public bool Equals(VacationData other)
		{
			if (other == null)
				return false;

			if (User != other.User)
				return false;

			if ((VacationDays == null) != (other.VacationDays == null))
				return false;

			if (VacationDays == null)
				return true;

			if (VacationDays.Count != other.VacationDays.Count)
				return false;

			return VacationDays.All(i => other.VacationDays.Any(i.Equals))
				&& other.VacationDays.All(j => VacationDays.Any(j.Equals));
		}
	}
}
