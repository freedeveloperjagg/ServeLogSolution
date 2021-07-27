using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Servelog.Infrastructure.HealthCheck.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Servelog.Infrastructure.HealthCheck
{ 
    public class DataBaseHealthCheck : ConfigurationHealthCheck
    {

        public DataBaseHealthCheck(IConfiguration config) : base(config)
        {
            this.config = config;
            this.Name = "DataBase";
        }

        /// <summary>
        /// Check the configuration of the DB and in the future the network connection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Task<HealthCheckResult> task = Task.Run(() =>
            {
                var status = HealthStatus.Healthy;
                var description = "Config found";
                Exception exception = null;
                var data = new Dictionary<string, object>();
                if (config == null)
                {
                    status = HealthStatus.Unhealthy;
                    description = "Configuration is null";
                    data.Add("ConfigurationFile", "The value is null");
                }
                else
                {
                    // Test the configuration
                    var connLogger = config.GetConnectionString("ApiLoggerDb");
                    SqlConnectionStringBuilder builder = new()
                    {
                        ConnectionString = connLogger
                    };
                    data.Add("ConfigurationApiLoggerDb", $"{builder.DataSource} : {builder.InitialCatalog}");
                    description = $"Connection to: {builder.DataSource} : {builder.InitialCatalog}";

                    // try to connect  with the DB
                    SqlConnection conn = new(connLogger);
                    try
                    {
                        conn.Open();
                        description += " Connectivity Tested OK!";
                        data.Add("Connectivity with ApiLoggerDb", "OK");
                    }
                    catch (Exception ex)
                    {
                        description += " Connectivity with ApiLoggerDb Test: FAIL!";
                        exception = ex;
                        data.Add("Connectivity with ApiLoggerDb", ex.Message);
                        status = HealthStatus.Unhealthy;
                    }
                    finally
                    {
                        conn.Close();
                    }
                    // Test second database
                    connLogger = config.GetConnectionString("InternalLoggerDb");
                    builder = new SqlConnectionStringBuilder
                    {
                        ConnectionString = connLogger
                    };
                    data.Add("Configuration InternalLoggerDb", $"{builder.DataSource} : {builder.InitialCatalog}");
                    description += $" | Connection to: {builder.DataSource} : {builder.InitialCatalog}";

                    // try to connect  with the DB
                    conn = new SqlConnection(connLogger);
                    try
                    {
                        conn.Open();
                        description += " Connectivity Tested OK!";
                        data.Add("Connectivity with InternalLoggerDb", "OK");
                    }
                    catch (Exception ex)
                    {
                        description += " Connectivity with InternalLoggerDb Test: FAIL!";
                        exception = ex;
                        data.Add("Connectivity with InternalLoggerDb", ex.Message);
                        status = HealthStatus.Unhealthy;
                    }
                    finally
                    {
                        conn.Close();
                    }

                }

                return new HealthCheckResult(status, description, exception, data);
            });

            return task;
        }


    }
}
