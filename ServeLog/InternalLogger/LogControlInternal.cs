using ServeLog.Model;
using System;

namespace ServeLog.InternalLogger
{
    /// <summary>
    /// 
    /// </summary>
    public class LogControlInternal : ILogControlInternal
    {
        /// <summary>
        /// Internal data communication for logger
        /// </summary>
        private readonly IDataInternal intData;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly CConfig cconfig;

        private readonly TimeZoneInfo timeZone;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="intData">Data Internal Class to communicate with DB</param>
        /// <param name="xconfig">Configuration settings</param>
        public LogControlInternal(IDataInternal intData, CConfig xconfig)
        {
            this.intData = intData;
            this.cconfig = xconfig;
            string timezoneSetting = xconfig.InternalLogSettings.TimeZone ?? "UTC";
            this.timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneSetting);
        }

        /// <summary>
        /// This write a INFO level information in the logger
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionText"></param>
        public LoggerModel? InternalDebugWriteLog(string message, string exceptionText)
        {
            try
            {
                var level = cconfig.InternalLogSettings.LogLevel;
                if (level == LogLevelEnumeration.DEBUG)
                {
                    LoggerModel model = new()
                    {
                        Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.timeZone),
                        MachineName = Environment.MachineName,
                        Level = "DEBUG",
                        Logger = "Internal Logger",
                        Message = message,
                        Exception = exceptionText
                    };
                    intData.WriteRecordInLog(model);
                    return model;
                }
            }
            catch (Exception ex)
            {
                // Nothing to do this is the log of the log...
                Console.WriteLine($"Fatal Error Happen the log of the log is not able to run. Run Logger Health Check: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Always write in the table when is invoked
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public LoggerModel? InternalAlwaysWriteLog(string message)
        {
            try
            {
                LoggerModel model = new()
                {
                    Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.timeZone),
                    MachineName = Environment.MachineName,
                    Level = "ALWAYS",
                    Logger = "Internal Logger",
                    Message = message,
                    Exception = string.Empty
                };
                intData.WriteRecordInLog(model);
                return model;
            }
            catch (Exception ex)
            {
                // Nothing to do this is the log of the log...
                Console.WriteLine($"Fatal Error Happen the log of the log is not able to run. Run Logger Health Check: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// This write a INFO level information in the logger
        /// </summary>
        public LoggerModel? InternalErrorWriteLog(string message, Exception? exception)
        {
            try
            {
                var level = cconfig.InternalLogSettings.LogLevel;
                if (level == LogLevelEnumeration.ERROR || level == LogLevelEnumeration.DEBUG)
                {
                    string excep = message ?? exception?.Message ?? string.Empty;
                    LoggerModel model = new()
                    {
                        Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.timeZone),
                        MachineName = Environment.MachineName,
                        Level = "ERROR",
                        Logger = "Internal Logger",
                        Message = excep,
                        Exception = exception?.StackTrace ?? string.Empty
                    };
                    intData.WriteRecordInLog(model);
                    return model;
                }
            }
            catch (Exception ex)
            {
                // Nothing to do this is the log of the log...
                Console.WriteLine($"Fatal Error Happen the log of the log is not able to run. Run Logger Health Check: {ex.Message}");
            }
            return null;
        }
    }
}
