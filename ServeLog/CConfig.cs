using Microsoft.Extensions.Configuration;
using ServeLog.Model;
using System.Collections.Generic;

namespace ServeLog
{
    public class CConfig(IConfiguration config)
    {
        public string Environment { get; } = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public string Version { get;} = System.Reflection.Assembly.GetEntryAssembly().GetName().Version?.ToString();
        public string AllowedHosts { get; } = config["AllowedHosts"] ?? throw new KeyNotFoundException("Key not Found: AllowedHosts");
        public LogSettings LogSettings { get; } = config.GetSection("LoggerService:LogSettings").Get<LogSettings>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:LogSettings");
        public InternalLogSettings InternalLogSettings { get; } = config.GetSection("LoggerService:InternalLogSettings").Get<InternalLogSettings>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:InternalLogSettings");
        public CorsSettings CorsSettings { get; } = config.GetSection("LoggerService:Cors").Get<CorsSettings>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:Cors");
        public Dictionary<string, string> Tables { get; } = config.GetSection("LoggerService:Tables").Get<Dictionary<string, string>>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:Tables");
        public string ApiLoggerDb { get; } = config.GetConnectionString("ApiLoggerDb") ?? throw new KeyNotFoundException("Key not Found: ApiLoggerDb");
        public string InternalLoggerDb { get; } = config.GetConnectionString("InternalLoggerDb") ?? throw new KeyNotFoundException("Key not Found: InternalLoggerDb");
    }
}
