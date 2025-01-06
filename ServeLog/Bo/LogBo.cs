using ServeLog.Data;
using ServeLog.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServeLog.Bo
{
    /// <summary>
    /// Business object to write logs
    /// </summary>
    public class LogBo : ILogBo, IDisposable
    {
        /// <summary>
        /// Logger Service
        /// </summary>
        private readonly ILogServices service;

        /// <summary>
        /// Cancelation Token Source
        /// </summary>
        private CancellationTokenSource? tokenCancel;

        private readonly CConfig cconfig;

        /// <summary>
        /// Time Zone info
        /// </summary>
        private readonly TimeZoneInfo timeZone;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xconfig">the configuration file</param>
        /// <param name="service"></param>
        public LogBo(CConfig xconfig, ILogServices service)
        {
            this.service = service;
            this.cconfig = xconfig;
            string timezoneSetting = xconfig.LogSettings.TimeZone;
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
            var cancelationMiliseconds = cconfig.LogSettings.TaskCancellationTimeMs;
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
/// <inheritdoc/>

        public void Dispose()
        {
            tokenCancel?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}