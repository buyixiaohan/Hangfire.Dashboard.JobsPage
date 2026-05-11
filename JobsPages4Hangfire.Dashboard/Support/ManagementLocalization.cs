using System.Collections.Generic;
using System.Globalization;
using JobsPages4Hangfire.Dashboard.resx;

namespace JobsPages4Hangfire.Dashboard.Support
{
    internal static class ManagementLocalization
    {
        public static void Configure(ManagementPageOptions options)
        {
            Resource.Culture = options?.GetCulture();
        }

        public static string Get(string name)
        {
            return Resource.ResourceManager.GetString(name, Resource.Culture) ?? name;
        }

        public static string Format(string name, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, Get(name), args);
        }

        public static IReadOnlyDictionary<string, string> ClientStrings()
        {
            return new Dictionary<string, string>
            {
                { "taskCreatedMessage", Get("Client_TaskCreatedMessage") },
                { "viewJob", Get("Client_ViewJob") },
                { "unknownError", Get("Client_UnknownError") },
                { "taskCreatedTitle", Get("Client_TaskCreatedTitle") },
                { "errorTitle", Get("Client_ErrorTitle") }
            };
        }
    }
}
