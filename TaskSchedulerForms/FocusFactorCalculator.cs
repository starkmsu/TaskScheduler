using System;
using System.Collections.Generic;
using System.Linq;
using TaskSchedulerForms.Config;

namespace TaskSchedulerForms
{
	internal class FocusFactorCalculator
	{
		private const double s_defaultFocusFactor = 0.5f;

		internal Dictionary<string, double> FocusFactors { get; set; }

		internal double DefaultFocusFactor { get; private set; }

		internal FocusFactorCalculator(IEnumerable<WorkerFocusFactor> focusFactors)
		{
			FocusFactors = focusFactors != null
				? focusFactors.ToDictionary(i => i.Worker, i => i.FocusFactor)
				: new Dictionary<string, double>();

			DefaultFocusFactor = s_defaultFocusFactor;
		}

		internal int CalculateDaysByTime(double remaining, string user)
		{
			double focusFactor = FocusFactors.ContainsKey(user)
				? FocusFactors[user]
				: s_defaultFocusFactor;
			return (int)Math.Ceiling(remaining / 8 / focusFactor);
		}
	}
}
