using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtils.Accessors
{
	public class TfsWiqlAccessor : IDisposable
	{
		private readonly WorkItemStore m_workItemStore;
		private readonly TfsAccessor m_tfsAccessor;

		public TfsWiqlAccessor(string tfsUrl)
		{
			m_tfsAccessor = new TfsAccessor(tfsUrl);
			m_workItemStore = m_tfsAccessor.GetWorkItemStore();
		}

		public void Dispose()
		{
			m_tfsAccessor.Dispose();
		}

		public string AddUsersConditions(string wiqlString, List<string> users)
		{
			if (users == null || users.Count == 0)
				return wiqlString;

			return wiqlString + GenerateUsersConditions(users);
		}

		public WorkItemCollection QueryWorkItemsByIds(List<int> ids)
		{
			const string queryStr = "SELECT * FROM WorkItems WHERE [System.Id] IN (@ids)";

			var paramValues = new Dictionary<string, object>();

			var complexParamValues = new Dictionary<string, List<object>>
			{
				{"ids", ids.ConvertAll(i => (object)i)}
			};

			return QueryWorkItems(
				queryStr,
				paramValues,
				complexParamValues);
		}

		public WorkItemCollection QueryWorkItems(
			string wiqlString,
			Dictionary<string, object> paramValues,
			Dictionary<string, List<object>> complexParamValues)
		{
			if (complexParamValues != null && complexParamValues.Count > 0)
				wiqlString = UpdateParams(wiqlString, paramValues, complexParamValues);

			var query = new Query(m_workItemStore, wiqlString, paramValues);

			return query.RunQuery();
		}

		public WorkItemLinkInfo[] QueryLinks(
			string wiqlString,
			Dictionary<string, object> paramValues,
			Dictionary<string, List<object>> complexParamValues)
		{
			if (complexParamValues != null && complexParamValues.Count > 0)
				wiqlString = UpdateParams(wiqlString, paramValues, complexParamValues);

			var query = new Query(m_workItemStore, wiqlString, paramValues);

			return query.RunLinkQuery();
		}

		private string UpdateParams(
			string wiqlString,
			Dictionary<string, object> paramValues,
			Dictionary<string, List<object>> complexParamValues)
		{
			var strBuilder = new StringBuilder(wiqlString);
			foreach (var complexParamValue in complexParamValues)
			{
				if (paramValues.ContainsKey(complexParamValue.Key))
					continue;

				string complexParamKey = "@" + complexParamValue.Key;
				if (wiqlString.IndexOf(complexParamKey, StringComparison.OrdinalIgnoreCase) == -1
					|| complexParamValue.Value.Count == 0)
					continue;

				var paramStrBuilder = new StringBuilder();
				for (int i = 0; i < complexParamValue.Value.Count; i++)
				{
					if (i > 0)
						paramStrBuilder.Append(',');
					paramStrBuilder.Append('@');
					string paramKey = complexParamValue.Key + i;
					paramStrBuilder.Append(paramKey);

					paramValues.Add(paramKey, complexParamValue.Value[i]);
				}
				strBuilder.Replace(complexParamKey, paramStrBuilder.ToString());
			}
			return strBuilder.ToString();
		}

		private string GenerateUsersConditions(List<string> users)
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(" AND (");
			for (int i = 0; i < users.Count; i++)
			{
				if (i > 0)
					strBuilder.Append(" OR ");
				strBuilder.Append("[System.ChangedBy] EVER '");
				strBuilder.Append(users[i]);
				strBuilder.Append('\'');
			}
			strBuilder.Append(')');
			return strBuilder.ToString();
		}
	}
}
