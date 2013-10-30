using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUtils.Accessors;

namespace TaskPlanningForms
{
	internal class DataLoader
	{
		internal WorkItemCollection GetLeadTasks(
			string tfsUrl,
			List<string> areaPaths,
			bool withSubAreaPaths)
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
			if (withSubAreaPaths)
			{
				strBuilder.Append(" AND (");
				for (int i = 0; i < areaPaths.Count; i++)
				{
					string areaPathParam = "areaPath" + i;
					if (i > 0)
						strBuilder.Append(" OR ");
					strBuilder.Append("[System.AreaPath] UNDER @" + areaPathParam);
					paramValues.Add(areaPathParam, areaPaths[i]);
				}
				strBuilder.Append(")");
			}
			else
			{
				strBuilder.Append(" AND [System.AreaPath] IN (@areaPath)");
				complexParamValues.Add("areaPath", areaPaths.Cast<object>().ToList());
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
