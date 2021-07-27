using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;

namespace Servelog.Infrastructure.HealthCheck.Extensions
{
    /// <summary>
    /// Extension to be able to intereact with the Http and the Configuration file
    /// </summary>
    public static class ConfigurationHealthCheckExtensions
    {
        const string DEFAULT_NAME = "configuration_health_check";

        /// <summary>
        /// Health Check Builder allowing to creating descendant that used config file
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hCheck"></param>
        /// <param name="name"></param>
        /// <param name="failureStatus"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddConfigurationHealthCheck(
            this IHealthChecksBuilder builder,
            ConfigurationHealthCheck hCheck,
            string name = default,
            ////string data = "",
            HealthStatus? failureStatus = default,
            IEnumerable<string> tags = default)
        {
            return builder.Add(new HealthCheckRegistration(
                name ?? DEFAULT_NAME,
                sp => hCheck,
                failureStatus,
                tags));
        }
    }
}
