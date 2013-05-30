using System;
using System.Collections.Generic;

namespace TaskPlanningForms
{
	[Serializable]
	public class Config
	{
		public string TfsUrl { get; set; }

		public string AreaPath { get; set; }

		public string IterationPath { get; set; }

		public List<DateTime> Holidays { get; set; }

		public Config()
		{
			TfsUrl = "https://tfs.sts.sitronics.com/sts";
			AreaPath = @"FORIS_Mobile\Product Management Domain\Order Catalogue";
			Holidays = new List<DateTime>();
		}
	}
}
