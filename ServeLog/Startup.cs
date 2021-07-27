using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Servelog.Infrastructure.HealthCheck;
using ServeLog.Bo;
using ServeLog.Data;
using ServeLog.InternalLogger;
using ServeLog.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServeLog
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private IDataInternal data;
        private ILogControlInternal logger;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            // Resolving the 400 Validation Error Type
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    IErrorManager errorManager = ErrorManager.Factory();
                    StringBuilder messageb = new();
                    string allErrors = string.Empty;

                    foreach (var item in context.ModelState)
                    {
                        messageb.Append($"Validation Failure in { context.ActionDescriptor.DisplayName} Parameter: { item.Key} Error: ");

                        foreach (var i in item.Value.Errors)
                        {
                            messageb.Append($" {i.ErrorMessage} - ");
                        }

                        errorManager.Errors.Add(new ErrorResponse()
                        {
                            Code = "400",
                            Description = messageb.ToString()
                        });
                        allErrors += $" {messageb} |";
                    }
                    // Enter the data time
                    DateTime errorTime = DateTime.UtcNow;
                    errorManager.DateCreated = errorTime;
                    errorManager.Description = $"{errorTime}: Validation Error in NetCoreDemo";

                    // Call the Logger to enter the information
                    this.logger.InternalErrorWriteLog(allErrors, null);

                    var error = new BadRequestObjectResult(errorManager);
                    return error;
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServeLog", Version = "v1" });
            });

            // Get Settings
            this.GetSettings();
            services.AddSingleton(configuration);

            // Add HealthCheck Infrastructure
            services.AddHealthChecks()
                .AddCheck<MemoryHealthCheck>("Memory")
                .AddCheck<VersionHealthCheck>("Version")
                .AddCheck<DataBaseHealthCheck>("Database");

            // Configure Internal Logger
            this.data = new DataInternal(configuration);
            this.logger = new LogControlInternal(this.data, configuration);
            services.AddSingleton<IDataInternal, DataInternal>();
            services.AddSingleton<ILogControlInternal, LogControlInternal>();

            // Configure Code Injection
            services.AddScoped<ILogServices, LogServices>();
            services.AddScoped<ILogBo, LogBo>();
        }

        /// <summary>
        /// This method gets called by the runtime. 
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("iis"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "ServeLog v1");
                });
            }

            // General Manager for Exceptions
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    DateTime errorReportedtime = DateTime.UtcNow;
                    ErrorManager errorManager = new()
                    {
                        DateCreated = errorReportedtime,
                    };
                    if (!env.IsProduction())
                    {
                        errorManager.StackTrace = errorFeature.Error.StackTrace;
                        errorManager.Description = errorFeature.Error.Message;
                    }
                    else
                    {
                        // Production
                        errorManager.Description = $" {errorManager.DateCreated}: Error in logger";
                    }

                    // Call the Logger to enter the information
                    this.logger.InternalErrorWriteLog(errorFeature.Error.Message, errorFeature.Error);

                    // Send the response back
                    string content = System.Text.Json.JsonSerializer.Serialize(errorManager);
                    context.Response.StatusCode = 500; // 500 reserved for exceptions in app.
                    await context.Response.WriteAsync(content);
                });
            });


            app.UseHealthChecks("/health"
               , new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
               {
                   Predicate = _ => true,
                   ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
               }
            );


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Get application Settings
        /// </summary>
        private void GetSettings()
        {
            // Get settings..................................................................
            Settings.LogTables = configuration.GetSection("Tables").Get<Dictionary<string, string>>();
            Settings.InternalLog = new InternalLogSettings
            {
                Table = configuration["InternalLogSettings:Table"]
            };
            bool valid = Enum.TryParse(configuration["InternalLogSettings:LogLevel"], true, out LogLevelEnum result);
            Settings.InternalLog.LogLevel = valid ? result : LogLevelEnum.INFO;

            // Get Log Zone Time to be used
            var timeZone = configuration["TimeZone"];
            Settings.TimeZone = timeZone == null
                ? TimeZoneInfo.Utc
                : TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            // Get the cancellation await token
            Settings.TaskCancellationTimeMs = Convert.ToInt32(configuration["TaskCancellationTimeMs"]);

            // End Settings ..................................................................
        }
    }
}
