using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Accessors;

namespace TaskSchedulerForms
{
	internal class DataLoader
	{
		internal WorkItemCollection GetLeadTasks(string tfsUrl, string queryPath)
		{
			using (var queryAccessor = new TfsQueryAccessor(tfsUrl))
			{
				return queryAccessor.QueryWorkItems(queryPath);
			}
		}

		internal WorkItemCollection GetLeadTasks(
			string tfsUrl,
			bool byArea,
			List<string> values,
			bool withSubTrees,
			bool withSprint)
		{
			return GetLeadTasks(
				tfsUrl,
				values,
				withSubTrees,
				withSprint,
				byArea ? "System.AreaPath" : "System.IterationPath",
				byArea ? "areaPath" : "iteration",
				byArea ? "System.IterationPath" : "System.AreaPath");
		}

		private WorkItemCollection GetLeadTasks(
			string tfsUrl,
			List<string> values,
			bool withSubTrees,
			bool withSprint,
			string systemFieldName,
			string fieldAlias,
			string additionalOrderField)
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
			if (withSprint)
				strBuilder.Append(" AND [Sprint] <> ''");
			strBuilder.Append(" AND [Microsoft.VSTS.Common.Discipline] IN (@discipline)");
			if (withSubTrees)
			{
				strBuilder.Append(" AND (");
				for (int i = 0; i < values.Count; i++)
				{
					string dataParam = fieldAlias + i;
					if (i > 0)
						strBuilder.Append(" OR ");
					strBuilder.Append("[" + systemFieldName + "] UNDER @" + dataParam);
					paramValues.Add(dataParam, values[i]);
				}
				strBuilder.Append(")");
			}
			else
			{
				strBuilder.Append(" AND [" + systemFieldName + "] IN (@" + fieldAlias + ")");
				complexParamValues.Add(fieldAlias, values.Cast<object>().ToList());
			}
			strBuilder.Append(" ORDER BY [Priority], [");
			strBuilder.Append(additionalOrderField + "]");

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
