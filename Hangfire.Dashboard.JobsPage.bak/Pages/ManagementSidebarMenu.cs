using System;
using System.Collections.Generic;

namespace Hangfire.Dashboard.JobsPage.Pages
{
	public static class ManagementSidebarMenu
	{
		public static List<Func<RazorPage, MenuItem>> Items = new List<Func<RazorPage, MenuItem>>();
	}
}
