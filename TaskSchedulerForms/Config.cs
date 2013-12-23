using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSchedulerForms
{
	[Serializable]
	public class Config
	{
		public string TfsUrl { get; set; }

		public List<string> AreaPaths { get; set; }

		public List<string> IterationPaths { get; set; }

		public List<string> AllIterationPaths { get; set; }

		public List<string> AllAreaPaths { get; set; }

		public List<DateTime> Holidays { get; set; }

		public List<VacationData> Vacations { get; set; }

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

				AreaPaths = CopyIfNotNull(AreaPaths),
				IterationPaths = CopyIfNotNull(IterationPaths),
				AllIterationPaths = CopyIfNotNull(AllIterationPaths),
				AllAreaPaths = CopyIfNotNull(AllAreaPaths),
				Holidays = CopyIfNotNull(Holidays),
				Vacations = CopyIfNotNull(Vacations),
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
				&& CollectionsEquals(AreaPaths, other.AreaPaths)
				&& CollectionsEquals(IterationPaths, other.IterationPaths)
				&& CollectionsEquals(AllIterationPaths, other.AllIterationPaths)
				&& CollectionsEquals(AllAreaPaths, other.AllAreaPaths)
				&& CollectionsEquals(Holidays, other.Holidays)
				&& CollectionsEquals(Vacations, other.Vacations);
		}

		private List<T> CopyIfNotNull<T>(IEnumerable<T> target)
		{
			if (target == null)
				return null;
			return new List<T>(target);
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
			return first.All(i => second.Any(i.Equals))
				&& second.All(j => first.Any(j.Equals));
		}
	}
}
