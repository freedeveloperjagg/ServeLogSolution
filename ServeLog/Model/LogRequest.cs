using System;
using System.ComponentModel.DataAnnotations;

namespace ServeLog.Models
{
    /// <summary>
    /// The Log Request Class
    /// </summary>
    public class LogRequest
    {
        /// <summary>
        /// Used to link the error to the user with the log 
        /// logged
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// The name of the machine where log is emitted
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 255, MinimumLength = 5)]
        public string MachineName { get; set; }

        /// <summary>
        /// The register Level
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Level { get; set; }

        /// <summary>
        /// The name of the logger
        /// Don't touch this!! this name is used to localize the log table.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 255, MinimumLength = 5)]
        public string Logger { get; set; }

        /// <summary>
        /// The message to be show
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string Message { get; set; }

        /// <summary>
        /// The exception Message to be logged
        /// </summary>
        public string Exception { get; set; }
    }
}