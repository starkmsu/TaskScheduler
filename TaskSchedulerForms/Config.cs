﻿using System;
using System.Collections.Generic;

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
	}
}