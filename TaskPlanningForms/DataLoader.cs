using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Accessors;

namespace TaskSchedulerForms
{
	internal class DataLoader
	{
		internal WorkItemCollection GetLeadTasksByAreas(
			string tfsUrl,
			List<string> areas,
			bool withSubTrees)
		{
			return GetLeadTasks(
				tfsUrl,
				areas,
				withSubTrees,
				"System.AreaPath",
				"areaPath");
		}

		internal WorkItemCollection GetLeadTasksByIterations(
			string tfsUrl,
			List<string> iterations,
			bool withSubTrees)
		{
			return GetLeadTasks(
				tfsUrl,
				iterations,
				withSubTrees,
				"System.IterationPath",
				"iteration");
		}

		private WorkItemCollection GetLeadTasks(
			string tfsUrl,
			List<string> data,
			bool withSubTrees,
			string systemFieldName,
			string fieldAlias)
		{
			var paramValues = new Dictionary<string, object>
			{
				{"project", @"FORIS_Mobile"},
			};

			var complexParamValues = new Dictionary<string, List<object>>
			{
				{"discipline", new List<object>{"Development"}},
				{"wiType", new List<object>{"LeadTask"}},
				{"wiState", new List<object>{"Proposed", "Active"}},
			};

			var strBuilder = new StringBuilder();
			strBuilder.Append("SELECT * FROM WorkItems");
			strBuilder.Append(" WHERE [System.TeamProject] = @project");
			strBuilder.Append(" AND [System.WorkItemType] IN (@wiType)");
			strBuilder.Append(" AND [System.State] IN (@wiState)");
			strBuilder.Append(" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)");
			if (withSubTrees)
			{
				strBuilder.Append(" AND (");
				for (int i = 0; i < data.Count; i++)
				{
					string dataParam = fieldAlias + i;
					if (i > 0)
						strBuilder.Append(" OR ");
					strBuilder.Append("[" + systemFieldName + "] UNDER @" + dataParam);
					paramValues.Add(dataParam, data[i]);
				}
				strBuilder.Append(")");
			}
			else
			{
				strBuilder.Append(" AND [" + systemFieldName + "] IN (@" + fieldAlias + ")");
				complexParamValues.Add(fieldAlias, data.Cast<object>().ToList());
			}
			strBuilder.Append(" ORDER BY [Priority]");

			using (var wiqlAccessor = new TfsWiqlAccessor(tfsUrl))
			{
				return wiqlAccessor.QueryWorkItems(
					strBuilder.ToString(),
					paramValues,
					complexParamValues);
			}
		}
	}
}
