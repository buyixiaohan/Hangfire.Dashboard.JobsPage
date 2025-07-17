using System;

namespace Hangfire.Dashboard.JobsPage.Metadata
{
    public class ManagementPageAttribute : Attribute
    {
        /// <summary>
        /// Title to display as header for Jobs
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 方法编码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// Name of the Menu that contains the jobs
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// desc
        /// </summary>
        public string Desc { get; set; }

        public ManagementPageAttribute(string menuName, string title, string munuCode, string desc)
        {
            Title = title;
            MenuName = menuName;
            MenuCode = munuCode;
            Desc = desc;
        }
        public ManagementPageAttribute()
        {
        }
    }
}
