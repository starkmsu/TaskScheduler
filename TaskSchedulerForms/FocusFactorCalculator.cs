using System;

namespace TaskSchedulerForms
{
	internal class FocusFactorCalculator
	{
		private const double m_focusFactor = 0.5f;

		internal int CalculateDaysByTime(double remaining, string user)
		{
			return (int)Math.Ceiling(remaining / 8 / m_focusFactor);
		}
	}
}
