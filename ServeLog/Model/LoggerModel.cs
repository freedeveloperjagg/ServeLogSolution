using System;
using System.ComponentModel.DataAnnotations;

namespace WapiLogger.Models
{
    /// <summary>
    /// Logger Model
    /// </summary>
    public class LoggerModel
    {
        /// <summary>
        /// Log Date
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// This is used to enter the Machine name that
        /// create the log.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The register Level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// The name of the logger
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// The message to be show
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The exception to be logged
        /// </summary>
        public string Exception { get; set; }
 
    }
}