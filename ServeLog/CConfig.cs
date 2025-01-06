using Microsoft.Extensions.Configuration;
using ServeLog.Model;
using System.Collections.Generic;

namespace ServeLog
{
    /// <summary>
    /// Program Settings
    /// This program check for each setting and store it in memeory
    /// </summary>
    /// <param name="config">The program config</param>
    public class CConfig(IConfiguration config)
    {
        /// <summary>
        /// NET Environment
        /// </summary>
        public string Environment { get; } = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "NOT FOUND";
        
        /// <summary>
        /// Program Version
        /// </summary>
        public string Version { get; } = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "NOT FOUND";
        
        /// <summary>
        /// Allowed Hosts
        /// </summary>
        public string AllowedHosts { get; } = config["AllowedHosts"] ?? throw new KeyNotFoundException("Key not Found: AllowedHosts");
        
        /// <summary>
        /// Settings of the logger program
        /// </summary>
        public LogSettings LogSettings { get; } = config.GetSection("LoggerService:LogSettings").Get<LogSettings>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:LogSettings");
        
        /// <summary>
        /// Settings for the internal Logger. the internal logger log errors that happen inside 
        /// the logger itself, for example bad requests, false token or tables.
        /// </summary>
        public InternalLogSettings InternalLogSettings { get; } = config.GetSection("LoggerService:InternalLogSettings").Get<InternalLogSettings>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:InternalLogSettings");
        
        /// <summary>
        /// The Cors Settings
        /// </summary>
        public CorsSettings CorsSettings { get; } = config.GetSection("LoggerService:Cors").Get<CorsSettings>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:Cors");
        
        /// <summary>
        /// Dictionary holding the token and the table that match with the token
        /// </summary>
        public Dictionary<string, string> Tables { get; } = config.GetSection("LoggerService:Tables").Get<Dictionary<string, string>>() ?? throw new KeyNotFoundException("Key not Found: LoggerService:Tables");
        
        /// <summary>
        /// The Connection string to logger the information
        /// </summary>
        public string ApiLoggerDb { get; } = config.GetConnectionString("ApiLoggerDb") ?? throw new KeyNotFoundException("Key not Found: ApiLoggerDb");
        
        /// <summary>
        /// The connection string for the internal logger. Can be the same that the ApiLoggerDB
        /// </summary>
        public string InternalLoggerDb { get; } = config.GetConnectionString("InternalLoggerDb") ?? throw new KeyNotFoundException("Key not Found: InternalLoggerDb");
    }
}
