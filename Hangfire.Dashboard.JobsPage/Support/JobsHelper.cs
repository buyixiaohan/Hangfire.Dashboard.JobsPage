using Hangfire.Dashboard.JobsPage.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hangfire.Dashboard.JobsPage.Support
{
    public static class JobsHelper
    {
        public static List<JobMetadata> JobMetadatas { get; private set; } = new List<JobMetadata>();
        internal static List<ManagementPageAttribute> ManagementPageAttrs { get; set; } = new List<ManagementPageAttribute>();

        internal static void GetAllJobs(Assembly assembly)
        {
            foreach (Type ti in assembly.GetTypes().Where(x => typeof(IJob).IsAssignableFrom(x) && x.Name != (typeof(IJob).Name)))
            {
                var q = "default";

                if (ti.GetCustomAttributes(true).OfType<ManagementPageAttribute>().Any())
                {
                    var mgmtPageAttr = ti.GetCustomAttribute<ManagementPageAttribute>();
                    //用code做路由，空则不使用
                    if (mgmtPageAttr != null && !string.IsNullOrEmpty(mgmtPageAttr.MenuCode))
                    {

                        if (string.IsNullOrEmpty(mgmtPageAttr.Desc))
                        {

                            if (ti.GetCustomAttributes(true).OfType<DescriptionAttribute>().Any())
                            {
                                mgmtPageAttr.Desc = ti.GetCustomAttribute<DescriptionAttribute>().Description;

                            }
                        }
                        if (!ManagementPageAttrs.Any(x => x.MenuCode == mgmtPageAttr.MenuCode))
                        {
                            ManagementPageAttrs.Add(mgmtPageAttr);
                        }

                        foreach (var methodInfo in ti.GetMethods().Where(m => m.DeclaringType == ti))
                        {
                            var meta = new JobMetadata
                            {
                                Type = ti,
                                Queue = q,
                                SectionTitle = mgmtPageAttr.Title,
                                MenuName = mgmtPageAttr.MenuName,
                                MenuCode = mgmtPageAttr.MenuCode,
                            };


                            meta.MethodInfo = methodInfo;

                            if (methodInfo.GetCustomAttributes(true).OfType<QueueAttribute>().Any())
                            {
                                meta.Queue = methodInfo.GetCustomAttribute<QueueAttribute>().Queue;
                            }

                            if (methodInfo.GetCustomAttributes(true).OfType<DescriptionAttribute>().Any())
                            {
                                meta.Description = methodInfo.GetCustomAttribute<DescriptionAttribute>().Description;
                            }

                            if (methodInfo.GetCustomAttributes(true).OfType<DisplayNameAttribute>().Any())
                            {
                                meta.DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            }

                            if (methodInfo.GetCustomAttributes(true).OfType<AllowMultipleAttribute>().Any())
                            {
                                meta.AllowMultiple = methodInfo.GetCustomAttribute<AllowMultipleAttribute>().AllowMultiple;
                            }
                            if (methodInfo.GetCustomAttributes(true).OfType<JobDisplayNameAttribute>().Any())
                            {
                                meta.JobName = methodInfo.GetCustomAttribute<JobDisplayNameAttribute>().DisplayName;
                            }

                            JobMetadatas.Add(meta);
                        }
                    }

                }


            }
        }
        public static List<string> GetAllQueues()
        {
            var queues = JobMetadatas.Select(m => m.Queue).Distinct().ToList();
            Regex rx = new Regex("[^a-z0-9_-]+");
            if (queues.Any(q => rx.Match(q).Success))
            {
                throw new Exception("The queue name must consist of lowercase letters, digits, underscore, and dash characters only.");
            }
            return queues;
        }
    }
}
