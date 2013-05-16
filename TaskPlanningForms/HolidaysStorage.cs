using System;
using System.Collections.Generic;
using System.IO;

namespace TaskPlanningForms
{
	internal class HolidaysStorage
	{
		private const string HolidaysFileName = "Holidays.cfg";

		internal List<DateTime> LoadHolidays()
		{
			var result = new List<DateTime>();
			using (var fs = new FileStream(HolidaysFileName, FileMode.OpenOrCreate, FileAccess.Read))
			{
				using (var fr = new StreamReader(fs))
				{
					while (!fr.EndOfStream)
					{
						string valStr = fr.ReadLine();
						DateTime val;
						if (DateTime.TryParse(valStr, out val))
							result.Add(val);
					}
				}
			}
			return result;
		}

		internal void SaveHolidays(List<DateTime> holidays)
		{
			using (var fs = new FileStream(HolidaysFileName, FileMode.Create, FileAccess.Write))
			{
				using (var fw = new StreamWriter(fs))
				{
					holidays.ForEach(h => fw.WriteLine(h.ToShortDateString()));
				}
			}
		}
	}
}
