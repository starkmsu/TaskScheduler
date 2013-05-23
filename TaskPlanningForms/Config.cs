using System;

namespace TaskPlanningForms
{
	[Serializable]
	public class Config
	{
		public string TfsUrl { get; set; }

		public Config()
		{
			TfsUrl = "https://tfs.sts.sitronics.com/sts";
		}
	}
}
