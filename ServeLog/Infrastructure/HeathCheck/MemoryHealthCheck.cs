using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Servelog.Infrastructure.HealthCheck.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servelog.Infrastructure.HealthCheck
{
    /// <summary>
    /// MemoryHealthCheck custom Check
    /// </summary>
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryCheckOptions> options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Check name
        /// </summary>
        public string Name => "memory_check";

        /// <summary>
        /// This talk check the Health of the memory in the applicatiob
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var options = this.options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
            {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
            };
            var status = (allocated < options.Threshold) ?
                HealthStatus.Healthy : context.Registration.FailureStatus;

            return Task.FromResult(new HealthCheckResult(
                status,
                description: "Reports degraded status if allocated bytes " +
                    $">= {options.Threshold} bytes.",
                exception: null,
                data: data));
        }
    }

}
