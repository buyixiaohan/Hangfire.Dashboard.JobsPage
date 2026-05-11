using System.Linq;
using System.Text.RegularExpressions;

namespace JobsPages4Hangfire.Dashboard.Support
{
    public static class ExtensionMethods
    {
        public static string ScrubURL(this string seed)
        {
            if (string.IsNullOrWhiteSpace(seed))
            {
                return string.Empty;
            }

            var validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/\\_-".ToCharArray();
            var result = string.Empty;
            foreach (var s in seed.ToCharArray())
            {
                result += validCharacters.Contains(s) ? s.ToString() : "-";
            }

            return Regex.Replace(result, "-{2,}", "-").Trim('-');
        }
    }
}
