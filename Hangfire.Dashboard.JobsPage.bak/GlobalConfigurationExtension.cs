using Hangfire.Dashboard.JobsPage.Pages;
using Hangfire.Dashboard.JobsPage.Support;
using System.Collections.Generic;
using System.Reflection;

namespace Hangfire.Dashboard.JobsPage
{
    public static class GlobalConfigurationExtension
    {

        internal static string FileSuffix()
        {
            var version = typeof(GlobalConfigurationExtension).Assembly.GetName().Version;
            return $"{version.Major}_{version.Minor}_{version.Build}";
        }
        public static void UseManagementPages(this IGlobalConfiguration config, Assembly assembly)
        {

            JobsHelper.GetAllJobs(assembly);
            CreateManagement();
        }
        public static void UseManagementPages(this IGlobalConfiguration config, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                JobsHelper.GetAllJobs(assembly);
            }
            CreateManagement();
        }

        private static void CreateManagement()
        {

            var pageSet = new List<string>();
            foreach (var menu in JobsHelper.ManagementPageAttrs)
            {
                ManagementBasePage.AddCommands(menu.MenuName);
                if (!pageSet.Contains(menu.MenuName))
                {
                    pageSet.Add(menu.MenuName);
                    ManagementSidebarMenu.Items.Add(p => new MenuItem(menu.MenuName, p.Url.To($"{ManagementPage.UrlRoute}/{menu.MenuCode.ScrubURL()}"))
                    {
                        Active = p.RequestPath.StartsWith($"{ManagementPage.UrlRoute}/{menu.MenuCode.ScrubURL()}")
                    });
                }

                DashboardRoutes.Routes.AddRazorPage($"{ManagementPage.UrlRoute}/{menu.MenuCode.ScrubURL()}", x => new ManagementBasePage(menu.MenuCode));
            }

            //note: have to use new here as the pages are dispatched and created each time. If we use an instance, the page gets duplicated on each call
            DashboardRoutes.Routes.AddRazorPage(ManagementPage.UrlRoute, x => new ManagementPage());

            // can't use the method of Hangfire.Console as it's usage overrides any similar usage here. Thus
            // we have to add our own endpoint to load it and call it from our code. Actually is a lot less work

            DashboardRoutes.Routes.Add($"{ManagementPage.UrlRoute}/jsmcss",
                new CombinedResourceDispatcher(
                    "text/css",
                    typeof(GlobalConfigurationExtension).GetTypeInfo().Assembly,
                    $"{typeof(GlobalConfigurationExtension).Namespace}.Content", new[] { "Libraries.dateTimePicker.bootstrap-datetimepicker.min.css", "Libraries.inputmask.inputmask.min.css", "management.css" }
                    )
                );
            DashboardRoutes.Routes.Add($"{ManagementPage.UrlRoute}/jsm",
                new CombinedResourceDispatcher(
                    "application/javascript",
                    typeof(GlobalConfigurationExtension).GetTypeInfo().Assembly,
                    $"{typeof(GlobalConfigurationExtension).Namespace}.Content", new[] { "Libraries.dateTimePicker.bootstrap-datetimepicker.min.js", "Libraries.inputmask.jquery.inputmask.bundle.min.js", "management.js", "cron.js" }
                    )
                );

            NavigationMenu.Items.Add(page => new MenuItem(ManagementPage.Title, page.Url.To(ManagementPage.UrlRoute))
            {
                Active = page.RequestPath.StartsWith(ManagementPage.UrlRoute)
            });

        }
    }


}
