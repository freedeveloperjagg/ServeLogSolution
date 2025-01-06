using ServeLog.Model;

namespace ServeLog.Data
{
    /// <summary>
    /// Services available to log program
    /// </summary>
    public interface ILogServices
    {
        /// <summary>
        /// Write record in Logger
        /// </summary>
        /// <param name="model"></param>
        void WriteRecordInLog(LoggerModel model);
    }
}