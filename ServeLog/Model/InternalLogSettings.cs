namespace ServeLog.Model
{
    public class InternalLogSettings
    {
        public string Table { get; set; }
        public LogLevelEnum LogLevel { get; set; }

        public string ConnectionString { get; set; }
    }
}