using System;
using System.Collections.Generic;

namespace TaskSchedulerForms.Data
{
	internal class WorkItemVerificationResult
	{
		internal VerificationResult Result { get; set; }

		internal List<string> Messages { get; set; }

		internal string AllMessagesString
		{
			get
			{
				return Messages == null
					? string.Empty
					: string.Join(Environment.NewLine, Messages);
			}
		}

		internal string AddidtionalData { get; set; }
	}
}
