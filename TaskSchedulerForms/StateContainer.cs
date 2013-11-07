using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TaskSchedulerForms
{
	internal class StateContainer
	{
		private List<string> LastAreaPaths { get; set; }
		private List<string> LastIterationPaths { get; set; }

		internal bool IsAreaFirstMode { get; set; }

		internal string LastTfsUrl { get; set; }

		internal bool LastWithSubTree { get; set; }

		internal StateContainer()
		{
			IsAreaFirstMode = true;
			LastAreaPaths = new List<string>();
			LastIterationPaths = new List<string>();
		}

		internal void SaveChosenFirstToConfig(Config config, List<string> values)
		{
			if (IsAreaFirstMode)
				config.AreaPaths = values;
			else
				config.IterationPaths = values;
		}

		internal void SaveAllSecondToConfig(Config config, List<string> values)
		{
			if (IsAreaFirstMode)
			{
				config.AllIterationPaths = values;
				config.AllAreaPaths = null;
			}
			else
			{
				config.AllAreaPaths = values;
				config.AllIterationPaths = null;
			}
		}

		internal void SaveChosenSecondToConfig(Config config, List<string> values)
		{
			if (IsAreaFirstMode)
				config.IterationPaths = values;
			else
				config.AreaPaths = values;
		}

		internal void SaveChosenFirstToState(List<string> values)
		{
			if (IsAreaFirstMode)
				LastAreaPaths = values;
			else
				LastIterationPaths = values;
		}

		internal void SaveChosenSecondToState(List<string> values)
		{
			if (IsAreaFirstMode)
				LastIterationPaths = values;
			else
				LastAreaPaths = values;
		}

		internal string GetParamForFirst(WorkItem workItem)
		{
			return IsAreaFirstMode ? workItem.AreaPath : workItem.IterationPath;
		}

		internal string GetParamForSecond(WorkItem workItem)
		{
			return IsAreaFirstMode ? workItem.IterationPath : workItem.AreaPath;
		}

		internal List<string> GetFirstList()
		{
			return IsAreaFirstMode ? LastAreaPaths : LastIterationPaths;
		}

		internal bool IsSecondFromStateContains(string value)
		{
			return IsAreaFirstMode ? LastIterationPaths.Contains(value) : LastAreaPaths.Contains(value);
		}
	}
}
