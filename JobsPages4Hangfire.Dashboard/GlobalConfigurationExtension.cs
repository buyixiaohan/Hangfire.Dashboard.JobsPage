using Hangfire;
using Hangfire.Dashboard;
using JobsPages4Hangfire.Dashboard.Pages;
using JobsPages4Hangfire.Dashboard.Support;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace JobsPages4Hangfire.Dashboard
{
    public static class GlobalConfigurationExtension
    {
        private static bool _managementResourcesCreated;
        private static bool _managementIndexCreated;
        private static bool _navigationMenuCreated;
        private static readonly HashSet<string> CreatedManagementMenus = new HashSet<string>();
        private static readonly object ManagementRoutesSyncRoot = new object();

        internal static string FileSuffix()
        {
            var version = typeof(GlobalConfigurationExtension).Assembly.GetName().Version;
            return $"{version.Major}_{version.Minor}_{version.Build}";
        }

        public static void UseManagementPages(this IGlobalConfiguration config, Assembly assembly)
        {
            config.UseManagementPages(assembly, new ManagementPageOptions());
        }

        public static void UseManagementPages(this IGlobalConfiguration config, Assembly assembly, string language)
        {
            config.UseManagementPages(assembly, new ManagementPageOptions { Language = language });
        }

        public static void UseManagementPages(this IGlobalConfiguration config, Assembly assembly, ManagementPageOptions options)
        {
            ManagementLocalization.Configure(options);
            JobsHelper.GetAllJobs(assembly);
            CreateManagement();
        }

        public static void UseManagementPages(this IGlobalConfiguration config, Assembly[] assemblies)
        {
            config.UseManagementPages(assemblies, new ManagementPageOptions());
        }

        public static void UseManagementPages(this IGlobalConfiguration config, Assembly[] assemblies, string language)
        {
            config.UseManagementPages(assemblies, new ManagementPageOptions { Language = language });
        }

        public static void UseManagementPages(this IGlobalConfiguration config, Assembly[] assemblies, ManagementPageOptions options)
        {
            ManagementLocalization.Configure(options);
            foreach (var assembly in assemblies)
            {
                JobsHelper.GetAllJobs(assembly);
            }
            CreateManagement();
        }

        private static void CreateManagement()
        {
            lock (ManagementRoutesSyncRoot)
            {
                CreateManagementCore();
            }
        }

        private static void CreateManagementCore()
        {
            foreach (var menu in JobsHelper.ManagementPageAttrs)
            {
                ManagementBasePage.AddCommands(menu.MenuCode);
                if (CreatedManagementMenus.Add(menu.MenuCode))
                {
                    ManagementSidebarMenu.Items.Add(p => new MenuItem(menu.MenuName, p.Url.To($"{ManagementPage.UrlRoute}/{menu.MenuCode.ScrubURL()}"))
                    {
                        Active = p.RequestPath.StartsWith($"{ManagementPage.UrlRoute}/{menu.MenuCode.ScrubURL()}")
                    });

                    DashboardRoutes.Routes.AddRazorPage($"{ManagementPage.UrlRoute}/{menu.MenuCode.ScrubURL()}", x => new ManagementBasePage(menu.MenuCode));
                }
            }

            if (!_managementIndexCreated)
            {
                DashboardRoutes.Routes.AddRazorPage(ManagementPage.UrlRoute, x => new ManagementPage());
                _managementIndexCreated = true;
            }

            if (!_managementResourcesCreated)
            {
                DashboardRoutes.Routes.Add($"{ManagementPage.UrlRoute}/jsmcss",
                    new CombinedResourceDispatcher(
                        "text/css; charset=utf-8",
                        typeof(GlobalConfigurationExtension).GetTypeInfo().Assembly,
                        $"{typeof(GlobalConfigurationExtension).Namespace}.Content", new[] { "Libraries.dateTimePicker.bootstrap-datetimepicker.min.css", "Libraries.inputmask.inputmask.min.css", "management.css" }
                        )
                    );

                DashboardRoutes.Routes.Add($"{ManagementPage.UrlRoute}/jsm",
                    new CombinedResourceDispatcher(
                        "application/javascript; charset=utf-8",
                        typeof(GlobalConfigurationExtension).GetTypeInfo().Assembly,
                        $"{typeof(GlobalConfigurationExtension).Namespace}.Content",
                        () => $"window.Hangfire=window.Hangfire||{{}};window.Hangfire.ManagementLocalization={JsonConvert.SerializeObject(ManagementLocalization.ClientStrings())};",
                        new[] { "Libraries.dateTimePicker.bootstrap-datetimepicker.min.js", "Libraries.inputmask.jquery.inputmask.bundle.min.js", "management.js", "cron.js" }
                        )
                    );

                DashboardRoutes.Routes.Add(
                    $"{ManagementPage.UrlRoute}/images/(?<file>[a-zA-Z0-9._-]+)",
                    new EmbeddedImageDispatcher(
                        typeof(GlobalConfigurationExtension).GetTypeInfo().Assembly,
                        $"{typeof(GlobalConfigurationExtension).Namespace}.wwwroot.images"));

                _managementResourcesCreated = true;
            }

            if (!_navigationMenuCreated)
            {
                NavigationMenu.Items.Add(page => new MenuItem(ManagementPage.Title, page.Url.To(ManagementPage.UrlRoute))
                {
                    Active = page.RequestPath.StartsWith(ManagementPage.UrlRoute)
                });
                _navigationMenuCreated = true;
            }
        }
    }
}
