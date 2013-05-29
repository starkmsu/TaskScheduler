using System;
using System.IO;
using System.Xml.Serialization;

namespace TaskPlanningForms
{
	internal class ConfigManager
	{
		private const string s_configFIleName = "config.cfg";

		internal static Config LoadConfig()
		{
			if (!File.Exists(s_configFIleName))
				return new Config();

			using (var fs = new FileStream(s_configFIleName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
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
			using (var fs = new FileStream(s_configFIleName, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				new XmlSerializer(typeof(Config)).Serialize(fs, config);
			}
		}
	}
}
