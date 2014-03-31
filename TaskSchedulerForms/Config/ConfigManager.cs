using System;
using System.IO;
using System.Xml.Serialization;

namespace TaskSchedulerForms.Config
{
	internal class ConfigManager
	{
		private const string s_oldConfigFileName = "config.cfg";
		private const string s_configExtension = "cfg";

		private static TaskSchedulerForms.Config.Config m_lastConfig;

		internal static TaskSchedulerForms.Config.Config LoadConfig()
		{
			string userAppDaraPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string appName = AppDomain.CurrentDomain.FriendlyName;
			while (appName.Contains("."))
			{
				appName = Path.GetFileNameWithoutExtension(appName);
			}
			appName += "." + s_configExtension;
			string path = Path.Combine(userAppDaraPath, appName);

			// search in user folder
			if (!File.Exists(path))
				path = s_oldConfigFileName;
			// search in local folder
			if (!File.Exists(path))
				return new TaskSchedulerForms.Config.Config();

			using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				try
				{
					var configObj = new XmlSerializer(typeof(TaskSchedulerForms.Config.Config)).Deserialize(fs);
					var config = configObj as TaskSchedulerForms.Config.Config;
					if (config != null)
						m_lastConfig = config.Copy();
					return config;
				}
				catch (Exception)
				{
					return new TaskSchedulerForms.Config.Config();
				}
			}
		}

		internal static void SaveConfig(TaskSchedulerForms.Config.Config config)
		{
			if (config.Equals(m_lastConfig))
				return;

			string userAppDaraPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string appName = AppDomain.CurrentDomain.FriendlyName;
			while (appName.Contains("."))
			{
				appName = Path.GetFileNameWithoutExtension(appName);
			}
			appName += "." + s_configExtension;
			string path = Path.Combine(userAppDaraPath, appName);

			using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				var serializer = new XmlSerializer(typeof(TaskSchedulerForms.Config.Config));
				serializer.Serialize(fs, config);
			}
		}
	}
}
