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
using ServeLog;
using ServeLog.Bo;
using ServeLog.Data;
using ServeLog.Infrastructure.HealthCheck;
using ServeLog.InternalLogger;
using ServeLog.Model;
using System;
using System.IO;
using System.Text;

LogControlInternal logger;

var builder = WebApplication.CreateBuilder(args);
// Here can go the Azure configuration is needed

// Configuration Set
IConfiguration configuration = builder.Configuration;
CConfig cconfig = new(configuration);

// Inject the CConfig...
builder.Services.AddSingleton(cconfig);

// Add the controllers


// Add Swagger........
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "ServeLog", 
        Version = cconfig.Version, 
        Description = "REST Logger Service. Logger through a unique service.",
        Contact = new OpenApiContact(){
            Name = "Jose Garcia",
            Url = new Uri("http://www.avalon-software.com")
        }
    });
    var xmlfile = $"{System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name}.xml";
    var xmlpath = Path.Combine(AppContext.BaseDirectory, xmlfile) ;
    c.IncludeXmlComments(xmlpath);
});

// Add HealthCheck Infrastructure
builder.Services.AddHealthChecks()
    .AddCheck<MemoryHealthCheck>("Memory")
    .AddCheck<VersionHealthCheck>("Version")
    .AddCheck<DataBaseHealthCheck>("Database");

// Configure Internal Logger
var data = new DataInternal(cconfig);
logger = new LogControlInternal(data, cconfig);
builder.Services.AddSingleton<IDataInternal, DataInternal>();
builder.Services.AddSingleton<ILogControlInternal, LogControlInternal>();

builder.Services.AddControllers()
 .ConfigureApiBehaviorOptions(options =>
  {
      options.InvalidModelStateResponseFactory = context =>
      {
          IErrorManager errorManager = ErrorManager.Factory();
          StringBuilder messageb = new();
          string allErrors = string.Empty;

          foreach (var item in context.ModelState)
          {
              messageb.Append("Validation Failure in");
              messageb.Append(context.ActionDescriptor.DisplayName);
              messageb.Append("Parameter: ");
              messageb.Append(item.Key);
              messageb.Append("/n/r");
              messageb.Append("Error: ");
              foreach (var i in item.Value.Errors)
              {
                  messageb.Append("/n/r");
                  messageb.Append(i.ErrorMessage);
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
          logger.InternalErrorWriteLog(allErrors, null);

          var error = new BadRequestObjectResult(errorManager);
          return error;
      };
  });

// Configure Code Injection
builder.Services.AddScoped<ILogServices, LogServices>();
builder.Services.AddScoped<ILogBo, LogBo>();

var app = builder.Build();

// ========================================================================
// Configure....

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "ServeLog v1");
});

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
        if (!cconfig.Environment.Contains("Prod", StringComparison.CurrentCultureIgnoreCase))
        {
            errorManager.StackTrace = errorFeature?.Error.StackTrace;
            errorManager.Description = errorFeature?.Error.Message;
        }
        else
        {
            // Production
            errorManager.Description = $" {errorManager.DateCreated}: Error in logger";
        }

        // Call the Logger to enter the information
        logger.InternalErrorWriteLog(errorFeature?.Error.Message ?? "Error is missing", errorFeature?.Error);

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

if (cconfig.CorsSettings.ActivePolicy != null)
{
    app.UseCors(cconfig.CorsSettings.ActivePolicy);
}
else
{
    app.UseCors("OpenPolicy");
}

app.UseAuthorization();


app.MapControllers();

app.Run();


//public static IHostBuilder CreateHostBuilder(string[] args) =>
//    Host.CreateDefaultBuilder(args)
//        .ConfigureWebHostDefaults(webBuilder =>
//        {
//            webBuilder.UseStartup<Startup>();
//        });


