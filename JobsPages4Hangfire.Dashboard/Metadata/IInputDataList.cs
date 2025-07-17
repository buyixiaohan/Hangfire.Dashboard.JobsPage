using System.Collections.Generic;

namespace JobsPages4Hangfire.Dashboard.Metadata
{
	public interface IInputDataList
	{
		Dictionary<string, string> GetData();
		string GetDefaultValue();
	}
}
