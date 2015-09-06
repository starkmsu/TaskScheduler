using System;

namespace TaskSchedulerForms.Config
{
	[Serializable]
	public class WorkerFocusFactor : IEquatable<WorkerFocusFactor>
	{
		public string Worker { get; set; }

		public double FocusFactor { get; set; }

		public bool Equals(WorkerFocusFactor other)
		{
			if (other == null)
				return false;

			return Worker == other.Worker
				&& Math.Abs(FocusFactor - other.FocusFactor) < 0.01;
		}
	}
}
