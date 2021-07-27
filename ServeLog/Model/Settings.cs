using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeLog.Model
{
    /// <summary>
    /// Gt the settings 
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Get the tables name and token
        /// </summary>
        public static Dictionary<string,string> LogTables { get; set; }

        /// <summary>
        /// Connection String and Table of the internal Log 
        /// </summary>
        public static InternalLogSettings InternalLog { get; set; }

        /// <summary>
        /// Time Info Zone, if none is given in the settings UTC is default
        /// </summary>
        public static TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Time in miliseconds to cancel a Task.
        /// </summary>
        public static int TaskCancellationTimeMs { get; set; }
    }
}
