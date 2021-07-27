using ServeLog.Models;
using System.Threading.Tasks;

namespace ServeLog.Bo
{
    public interface ILogBo
    {
        Task PostLogEntryAsync(LogRequest request);
    }
}