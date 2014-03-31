using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TaskSchedulerForms.Const;

namespace TaskSchedulerForms.Config
{
	internal class StateContainer
	{
		private WorkMode m_workMode;
		private List<string> LastAreaPaths { get; set; }
		private List<string> LastIterationPaths { get; set; }

		internal WorkMode WorkMode
		{
			get { return m_workMode; }
			set
			{
				if (value != WorkMode.Query)
					ByArea = value == WorkMode.AreaFirst;
				m_workMode = value;
			}
		}

		internal bool ByArea { get; set; }

		internal string LastTfsUrl { get; set; }

		internal bool LastWithSubTree { get; set; }

		internal StateContainer()
		{
			ByArea = true;
			WorkMode = WorkMode.AreaFirst;
			LastAreaPaths = new List<string>();
			LastIterationPaths = new List<string>();
		}

		internal void SaveChosenFirstToConfig(TaskSchedulerForms.Config.Config config, List<string> values)
		{
			if (WorkMode == WorkMode.AreaFirst)
				config.AreaPaths = values;
			else if (WorkMode == WorkMode.IterationFirst)
				config.IterationPaths = values;
		}

		internal void SaveAllSecondToConfig(TaskSchedulerForms.Config.Config config, List<string> values)
		{
			if (WorkMode == WorkMode.AreaFirst)
			{
				config.AllIterationPaths = values;
				config.AllAreaPaths = null;
			}
			else if (WorkMode == WorkMode.IterationFirst)
			{
				config.AllAreaPaths = values;
				config.AllIterationPaths = null;
			}
		}

		internal void SaveChosenSecondToConfig(TaskSchedulerForms.Config.Config config, List<string> values)
		{
			if (WorkMode == WorkMode.AreaFirst)
			{
				config.IterationPaths = values;
				config.WorkMode = WorkMode.AreaFirst;
				config.ByArea = true;
			}
			else if (WorkMode == WorkMode.IterationFirst)
			{
				config.AreaPaths = values;
				config.WorkMode = WorkMode.IterationFirst;
				config.ByArea = false;
			}
		}

		internal void SaveChosenFirstToState(List<string> values)
		{
			if (WorkMode == WorkMode.AreaFirst)
				LastAreaPaths = values;
			else if (WorkMode == WorkMode.IterationFirst)
				LastIterationPaths = values;
		}

		internal void SaveChosenSecondToState(List<string> values)
		{
			if (WorkMode == WorkMode.AreaFirst)
				LastIterationPaths = values;
			else if (WorkMode == WorkMode.IterationFirst)
				LastAreaPaths = values;
		}

		internal string GetParamForFirst(WorkItem workItem)
		{
			return WorkMode == WorkMode.AreaFirst ? workItem.AreaPath : workItem.IterationPath;
		}

		internal string GetParamForSecond(WorkItem workItem)
		{
			return WorkMode == WorkMode.AreaFirst ? workItem.IterationPath : workItem.AreaPath;
		}

		internal List<string> GetFirstList()
		{
			return WorkMode == WorkMode.AreaFirst ? LastAreaPaths : LastIterationPaths;
		}

		internal bool IsSecondFromStateContains(string value)
		{
			return WorkMode == WorkMode.AreaFirst ? LastIterationPaths.Contains(value) : LastAreaPaths.Contains(value);
		}
	}
}
