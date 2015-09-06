using System;
using System.Collections.Generic;
using System.Linq;
using TaskSchedulerForms.Const;

namespace TaskSchedulerForms.Config
{
	[Serializable]
	public class Config
	{
		public string TfsUrl { get; set; }

		public List<string> AreaPathsByArea { get; set; }

		public List<string> IterationPathsByArea { get; set; }

		public List<string> AreaPathsByIteration { get; set; }

		public List<string> IterationPathsByIteration { get; set; }

		public List<string> LastAreaPaths { get; set; }

		public List<string> LastIterationPaths { get; set; }

		public List<string> AllAreaPaths { get; private set; }

		public List<string> AllIterationPaths { get; private set; }

		public List<DateTime> Holidays { get; set; }

		public List<VacationData> Vacations { get; set; }

		public List<WorkerFocusFactor> FocusFactors { get; set; }

		public bool WithSubAreaPaths { get; set; }

		public WorkMode WorkMode { get; set; }

		public bool ByArea { get; set; }

		public string QueryPath { get; set; }

		public Config()
		{
			TfsUrl = "https://tfs.sts.sitronics.com/sts";
			Holidays = new List<DateTime>();
			Vacations = new List<VacationData>();
			WorkMode = WorkMode.AreaFirst;
			ByArea = true;
			AllAreaPaths = new List<string>();
			AllIterationPaths = new List<string>();
			FocusFactors = new List<WorkerFocusFactor>();
		}

		public Config Copy()
		{
			return new Config
			{
				TfsUrl = TfsUrl,
				WorkMode = WorkMode,
				WithSubAreaPaths = WithSubAreaPaths,
				ByArea = ByArea,
				QueryPath = QueryPath,

				AreaPathsByArea = CopyIfNotNull(AreaPathsByArea),
				IterationPathsByArea = CopyIfNotNull(IterationPathsByArea),
				AreaPathsByIteration = CopyIfNotNull(AreaPathsByIteration),
				IterationPathsByIteration = CopyIfNotNull(IterationPathsByIteration),
				LastAreaPaths = CopyIfNotNull(LastAreaPaths),
				LastIterationPaths = CopyIfNotNull(LastIterationPaths),
				AllAreaPaths = CopyIfNotNull(AllAreaPaths),
				AllIterationPaths = CopyIfNotNull(AllIterationPaths),
				Holidays = CopyIfNotNull(Holidays),
				Vacations = CopyIfNotNull(Vacations),
				FocusFactors = CopyIfNotNull(FocusFactors),
			};
		}

		public bool Equals(Config other)
		{
			if (other == null)
				return false;
			return TfsUrl == other.TfsUrl
				&& WorkMode == other.WorkMode
				&& WithSubAreaPaths == other.WithSubAreaPaths
				&& ByArea == other.ByArea
				&& QueryPath == other.QueryPath
				&& CollectionsEquals(AreaPathsByArea, other.AreaPathsByArea)
				&& CollectionsEquals(IterationPathsByArea, other.IterationPathsByArea)
				&& CollectionsEquals(AreaPathsByIteration, other.AreaPathsByIteration)
				&& CollectionsEquals(IterationPathsByIteration, other.IterationPathsByIteration)
				&& CollectionsEquals(LastIterationPaths, other.LastIterationPaths)
				&& CollectionsEquals(LastAreaPaths, other.LastAreaPaths)
				&& CollectionsEquals(AllAreaPaths, other.AllAreaPaths)
				&& CollectionsEquals(AllIterationPaths, other.AllIterationPaths)
				&& CollectionsEquals(Holidays, other.Holidays)
				&& CollectionsEquals(Vacations, other.Vacations)
				&& CollectionsEquals(FocusFactors, other.FocusFactors);
		}

		private List<T> CopyIfNotNull<T>(IEnumerable<T> target)
		{
			if (target == null)
				return null;
			return new List<T>(target.Distinct());
		}

		private bool CollectionsEquals<T>(List<T> first, List<T> second)
			where  T : IEquatable<T>
		{
			if (first == null)
			{
				if (second == null || second.Count == 0)
					return true;
				return false;
			}
			if (second == null)
				return first.Count == 0;
			if (first.Count != second.Count)
				return false;
			return first.All(i => second.Any(arg => i.Equals(arg)))
				&& second.All(j => first.Any(arg => j.Equals(arg)));
		}
	}
}
