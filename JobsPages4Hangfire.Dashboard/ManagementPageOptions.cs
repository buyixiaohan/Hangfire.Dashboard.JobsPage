using System.Globalization;

namespace JobsPages4Hangfire.Dashboard
{
    public sealed class ManagementPageOptions
    {
        public string Language { get; set; }

        internal CultureInfo GetCulture()
        {
            return string.IsNullOrWhiteSpace(Language)
                ? null
                : CultureInfo.GetCultureInfo(Language);
        }
    }
}
