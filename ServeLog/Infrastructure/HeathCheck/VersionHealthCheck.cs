using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Servelog.Infrastructure.HealthCheck
{
    /// <summary>
    /// Checking the assembly version
    /// </summary>
    public class VersionHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Check name
        /// </summary>
        public string Name => "assembly";

        /// <summary>
        /// Helath Check Async
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var version = Assembly.GetEntryAssembly().GetName().FullName;
            var infoVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var data = new Dictionary<string, object>()
            {
                { "AssemblyVersion", version.ToString() },
                { "InformationalVersion", infoVersion}
            };

            var status = HealthStatus.Healthy;
           return Task.FromResult(new HealthCheckResult(
                status,
                description: $"Assembly: {version} Info Version: {infoVersion}",
                exception: null,
                data: data));
        }
    }
}
