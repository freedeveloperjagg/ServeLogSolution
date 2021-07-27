using WapiLogger.Models;

namespace ServeLog.Data
{
    public interface ILogServices
    {
        void WriteRecordInLog(LoggerModel model);
        void InsertingLogRecord(LoggerModel model, string connString, string table);
    }
}