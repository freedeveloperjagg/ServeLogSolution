using WapiLogger.Models;

namespace ServeLog.InternalLogger
{
    /// <summary>
    /// IDataInternal
    /// </summary>
    public interface IDataInternal
    {
        void WriteRecordInLog(LoggerModel model);
    }
}