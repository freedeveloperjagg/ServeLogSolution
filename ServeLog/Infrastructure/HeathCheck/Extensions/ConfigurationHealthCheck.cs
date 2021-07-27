using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servelog.Infrastructure.HealthCheck.Extensions
{
    /// <summary>
    /// Base class for all HealthCheck that use configuration
    /// </summary>
    public class ConfigurationHealthCheck: IHealthCheck
    {
        /// <summary>
        /// Health Check implementation Name
        /// </summary>
        public string Name = "configuration";

        /// <summary>
        /// Protected configuration
        /// </summary>
        protected IConfiguration config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        public ConfigurationHealthCheck(IConfiguration config )
        {
            this.config = config;
        }

        /// <summary>
        /// This is a base class is not override check if the config is in place
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var status = HealthStatus.Healthy;
            var description = "Config found";
            if (config == null)
            {
                status = HealthStatus.Unhealthy;
                description = "Configuration is null";
            }

            var data = new Dictionary<string, object>()
            {
                { "Config", description }
            };

            var returned = Task.FromResult(new HealthCheckResult(
               status,
               description: description,
               exception: null,
               data: data));

            return returned;

        }
    }
}
