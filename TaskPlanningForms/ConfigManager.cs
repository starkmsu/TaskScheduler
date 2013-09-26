using System;
using System.IO;
using System.Xml.Serialization;

namespace TaskPlanningForms
{
	internal class ConfigManager
	{
		private const string s_oldConfigFileName = "config.cfg";
		private const string s_configExtension = "cfg";

		internal static Config LoadConfig()
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
				return new Config();

			using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				try
				{
					var configObj = new XmlSerializer(typeof(Config)).Deserialize(fs);
					return configObj as Config;
				}
				catch (Exception)
				{
					return new Config();
				}
			}
		}

		internal static void SaveConfig(Config config)
		{
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
				try
				{
					var serializer = new XmlSerializer(typeof(Config));
					serializer.Serialize(fs, config);
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
	}
}
