using WapiLogger.Models;

namespace ServeLog
{
    public static class SatinizeLoggerModel
    {
        /// <summary>
        /// Sanitize the logger model
        /// </summary>
        /// <param name="model">The model to be sanitize</param>
        /// <returns>Model sanitized</returns>
        public static LoggerModel Execute(LoggerModel model)
        {
            model.Message = model.Message.Length > 1999 ? model.Message.Remove(1998) : model.Message;
            model.Exception = (model.Exception != null)
                ? model.Exception.Length > 3999 ? model.Exception.Remove(3998) : model.Exception
                : string.Empty;
            model.MachineName = model.MachineName.Length > 254 ? model.MachineName.Remove(253) : model.MachineName;
            model.Level = model.Level.Length > 49 ? model.Level.Remove(48) : model.Level;
            model.Logger = model.Logger.Length > 254 ? model.Logger.Remove(253) : model.Logger;
            return model;
        }
    }
}
