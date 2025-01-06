using ServeLog.Model;
using System;

namespace ServeLog.InternalLogger
{
    /// <summary>
    /// Interfaz for Internal Logger.
    /// </summary>
    public interface ILogControlInternal
    {
        /// <summary>
        /// This method always write information in the Tale
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        LoggerModel? InternalAlwaysWriteLog(string message);
        
        /// <summary>
        /// This write if debug is activate in the settings LEVEL
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionText"></param>
        /// <returns></returns>
        LoggerModel? InternalDebugWriteLog(string message, string exceptionText);
        
        /// <summary>
        /// This write in table if LEVEL is ERROR or higher
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        LoggerModel? InternalErrorWriteLog(string message, Exception exception);
    }
}