#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ServeLog.Model
{
    public class InternalLogSettings
    {
        public string? Table { get; set; }

        public LogLevelEnumeration LogLevel { get; set; }

        public string? TimeZone { get; set; }

        public string? ConnectionString { get; set; }
    }
}