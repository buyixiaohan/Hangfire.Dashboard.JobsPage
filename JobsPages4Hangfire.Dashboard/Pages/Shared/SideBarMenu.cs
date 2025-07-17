using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;

namespace JobsPages4Hangfire.Dashboard.Pages
{
    internal partial class SideBarMenu
    {
        public SideBarMenu([NotNull] IEnumerable<Func<RazorPage, MenuItem>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            Items = items;
        }

        public IEnumerable<Func<RazorPage, MenuItem>> Items { get; }

    }
}
