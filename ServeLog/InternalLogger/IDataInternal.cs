using ServeLog.Model;

namespace ServeLog.InternalLogger
{
    /// <summary>
    /// IDataInternal
    /// </summary>
    public interface IDataInternal
    {
        /// <summary>
        /// Write the log information in the table
        /// </summary>
        /// <param name="model"></param>
        void WriteRecordInLog(LoggerModel model);
    }
}