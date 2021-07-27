using Microsoft.Extensions.Configuration;
using ServeLog.Data;
using ServeLog.Model;
using ServeLog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using WapiLogger.Models;

namespace ServeLog.Bo
{
    /// <summary>
    /// Business object to write logs
    /// </summary>
    public class LogBo : ILogBo
    {
        /// <summary>
        /// Logger Service
        /// </summary>
        private readonly ILogServices service;

        /// <summary>
        /// Cancelation Token Source
        /// </summary>
        private CancellationTokenSource tokenCancel;

        /// <summary>
        /// Time Zone info
        /// </summary>
        private readonly TimeZoneInfo timeZone;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public LogBo(IConfiguration config, ILogServices service)
        {
            this.service = service;
            string timezoneSetting = config["TimeZone"];
            this.timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneSetting);
        }

        /// <summary>
        /// Post the log entry. Operation must be completed in less than 60 second
        /// </summary>
        /// <param name="request"></param>
        /// <remark>
        /// Cancellation token guarantee that a connection pool does not get trapped forever
        /// </remark>
        public Task PostLogEntryAsync(LogRequest request)
        {
            var cancelationMiliseconds = Settings.TaskCancellationTimeMs;
            this.tokenCancel = new CancellationTokenSource(cancelationMiliseconds);
            var token = tokenCancel.Token;
            var t = Task.Run(() =>
             {
                 // Take the value entered or give one by default now UTC
                 request.CreatedDate ??= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.timeZone);
                 LoggerModel model = new()
                 {
                     Date = request.CreatedDate.GetValueOrDefault(),
                     Exception = request.Exception,
                     Level = request.Level,
                     Logger = request.Logger,
                     Message = request.Message,
                     MachineName = request.MachineName ?? string.Empty,
                 };

                 this.service.WriteRecordInLog(model);
             }, token);

            return t;
        }
    }
}