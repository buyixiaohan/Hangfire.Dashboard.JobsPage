using Hangfire.Dashboard;
using System;
using System.Collections.Generic;

namespace JobsPages4Hangfire.Dashboard.Pages
{
    public static class ManagementSidebarMenu
    {
        public static List<Func<RazorPage, MenuItem>> Items = new List<Func<RazorPage, MenuItem>>();
    }
}
