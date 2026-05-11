using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace JobsPages4Hangfire.Dashboard.Support
{
    internal class EmbeddedImageDispatcher : IDashboardDispatcher
    {
        private static readonly IDictionary<string, string> ContentTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".gif", "image/gif" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".svg", "image/svg+xml" },
            { ".webp", "image/webp" }
        };

        private readonly Assembly _assembly;
        private readonly string _baseNamespace;

        public EmbeddedImageDispatcher(Assembly assembly, string baseNamespace)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
        }

        public async Task Dispatch(DashboardContext context)
        {
            var fileName = context.UriMatch.Groups["file"].Value;
            if (string.IsNullOrWhiteSpace(fileName) || fileName.IndexOfAny(new[] { '/', '\\' }) >= 0)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var extension = Path.GetExtension(fileName);
            if (!ContentTypes.TryGetValue(extension, out var contentType))
            {
                context.Response.StatusCode = 404;
                return;
            }

            var resourceName = $"{_baseNamespace}.{fileName}";
            using (var inputStream = _assembly.GetManifestResourceStream(resourceName))
            {
                if (inputStream == null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                context.Response.ContentType = contentType;
                context.Response.SetExpire(DateTimeOffset.Now.AddYears(1));
                await inputStream.CopyToAsync(context.Response.Body).ConfigureAwait(false);
            }
        }
    }
}
