using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobsPages4Hangfire.Dashboard.Support
{
    internal class CombinedResourceDispatcher : EmbeddedResourceDispatcher
    {
        private readonly Assembly _assembly;
        private readonly string _baseNamespace;
        private readonly string[] _resourceNames;
        private readonly Func<string> _prefixFactory;

        public CombinedResourceDispatcher(
            [NotNull] string contentType,
            [NotNull] Assembly assembly,
            string baseNamespace,
            params string[] resourceNames) : base(contentType, assembly, null)
        {
            _assembly = assembly;
            _baseNamespace = baseNamespace;
            _resourceNames = resourceNames;
        }

        public CombinedResourceDispatcher(
            [NotNull] string contentType,
            [NotNull] Assembly assembly,
            string baseNamespace,
            Func<string> prefixFactory,
            params string[] resourceNames) : this(contentType, assembly, baseNamespace, resourceNames)
        {
            _prefixFactory = prefixFactory;
        }

        protected override async Task WriteResponse(DashboardResponse response)
        {
            if (_prefixFactory != null)
            {
                var prefixBytes = new UTF8Encoding().GetBytes($"{_prefixFactory()}\n\r");
                await response.Body.WriteAsync(prefixBytes, 0, prefixBytes.Length).ConfigureAwait(false);
            }

            foreach (var resourceName in _resourceNames)
            {
                var nameBytes = new UTF8Encoding().GetBytes($"\n\r/* {resourceName} */\n\r");
                await response.Body.WriteAsync(nameBytes, 0, nameBytes.Length).ConfigureAwait(false);
                await WriteResource(
                    response,
                    _assembly,
                    $"{_baseNamespace}.{resourceName}").ConfigureAwait(false);
            }
        }
    }
}
