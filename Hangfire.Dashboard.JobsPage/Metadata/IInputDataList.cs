using System.Collections.Generic;

namespace Hangfire.Dashboard.JobsPage.Metadata
{
	public interface IInputDataList
	{
		Dictionary<string, string> GetData();
		string GetDefaultValue();
	}
}
