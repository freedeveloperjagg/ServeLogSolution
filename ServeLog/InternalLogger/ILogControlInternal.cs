using System;
using WapiLogger.Models;

namespace ServeLog.InternalLogger
{
    public interface ILogControlInternal
    {
        LoggerModel InternalAlwaysWriteLog(string message);
        LoggerModel InternalDebugWriteLog(string message, string exceptionText);
        LoggerModel InternalErrorWriteLog(string message, Exception exception);
    }
}