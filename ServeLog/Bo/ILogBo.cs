using ServeLog.Model;
using System.Threading.Tasks;

namespace ServeLog.Bo
{
    /// <summary>
    /// Interfaz for business Object
    /// </summary>
    public interface ILogBo
    {
        /// <summary>
        /// Ue this to post the logger to the service
        /// </summary>
        /// <param name="request">the object to be logged</param>
        /// <returns>a empty task the method is run and forget.</returns>
        Task PostLogEntryAsync(LogRequest request);
    }
}