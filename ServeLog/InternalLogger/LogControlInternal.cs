using Microsoft.Extensions.Configuration;
using ServeLog.Model;
using System;
using WapiLogger.Models;

namespace ServeLog.InternalLogger
{
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
        /// <param name="config">Configuration settings</param>
        public LogControlInternal(IDataInternal intData, CConfig xconfig)
        {
            this.intData = intData;
            this.cconfig = xconfig;
            string timezoneSetting = xconfig.InternalLogSettings.TimeZone;
            this.timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneSetting);
        }

        /// <summary>
        /// This write a INFO level information in the logger
        /// </summary>
        /// <param name="model">
        /// The information to be stored.
        /// </param>
        public LoggerModel InternalDebugWriteLog(string message, string exceptionText)
        {
            try
            {
                var level = cconfig.InternalLogSettings.LogLevel;
                if (level == LogLevelEnum.DEBUG)
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

        public LoggerModel InternalAlwaysWriteLog(string message)
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
        /// <param name="model">
        /// The information to be stored.
        /// </param>
        public LoggerModel InternalErrorWriteLog(string message, Exception exception)
        {
            try
            {
                var level = cconfig.InternalLogSettings.LogLevel;
                if (level == LogLevelEnum.ERROR || level == LogLevelEnum.DEBUG)
                {
                    string excep = message ?? exception?.Message;
                    LoggerModel model = new()
                    {
                        Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.timeZone),
                        MachineName = Environment.MachineName,
                        Level = "ERROR",
                        Logger = "Internal Logger",
                        Message = excep,
                        Exception = exception?.StackTrace
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
