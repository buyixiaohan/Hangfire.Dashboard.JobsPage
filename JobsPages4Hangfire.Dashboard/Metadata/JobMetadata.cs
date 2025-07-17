using JobsPages4Hangfire.Dashboard.Support;
using System;
using System.Reflection;

namespace JobsPages4Hangfire.Dashboard.Metadata
{
    public class JobMetadata
    {
        public string SectionTitle { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string MenuName { get; set; }

        public string MenuCode { get; set; }
        public bool AllowMultiple { get; set; }

        public string Queue { get; set; }

        public string JobName { get; set; }
        public Type Type { get; set; }
        public MethodInfo MethodInfo { get; set; }

        public string MethodName => Type.Name + "_" + MethodInfo.Name;
        public string JobId => $"{MenuCode}/{JobName.ScrubURL()}";
        public string Name => $"{DisplayName ?? MethodName}";


    }
}
