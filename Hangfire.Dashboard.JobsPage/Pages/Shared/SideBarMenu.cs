using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Dashboard.JobsPage.Pages
{
    partial class SideBarMenu
    {
        public SideBarMenu([NotNull] IEnumerable<Func<RazorPage, MenuItem>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            Items = items;
        }

        public IEnumerable<Func<RazorPage, MenuItem>> Items { get; }

    }
}
