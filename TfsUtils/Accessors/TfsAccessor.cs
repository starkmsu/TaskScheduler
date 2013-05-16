﻿using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUtils.Accessors
{
	public class TfsAccessor : IDisposable
	{
		private TfsTeamProjectCollection m_tpc;

		public TfsAccessor(string tfsUrl)
		{
			Authenticate(tfsUrl);
		}

		public void Dispose()
		{
			if (m_tpc != null)
				m_tpc.Dispose();
		}

		public WorkItemStore GetWorkItemStore()
		{
			CheckLogon();

			return m_tpc.GetService<WorkItemStore>();
		}

		public VersionControlServer GetVersionControlServer()
		{
			CheckLogon();

			return m_tpc.GetService<VersionControlServer>();
		}

		private void CheckLogon()
		{
			if (m_tpc == null)
				throw new InvalidOperationException("There is no authenticated TfsTeamProjectCollection");
		}

		private void Authenticate(string tfsUrl)
		{
			try
			{
				var credentials = new UICredentialsProvider();
				m_tpc = new TfsTeamProjectCollection(new Uri(tfsUrl), credentials);
				m_tpc.EnsureAuthenticated();
			}
			catch
			{
				m_tpc = null;
				throw;
			}
		}
	}
}
