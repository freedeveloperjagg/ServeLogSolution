
using System;
using System.Collections.Generic;
using System.Net;

namespace ServeLog.Model
{
    /// <summary>
    /// Error Message
    /// </summary>
    public class ErrorManager : IErrorManager
    {
        /// <summary>
        /// This link the log record with the error
        /// message
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// List of Validations or internal Errors
        /// Used to reported error conditions that does
        /// not raise exceptions. Normally Error 400 Type
        /// </summary>
        public List<ErrorResponse> Errors { get; set; }

        /// <summary>
        /// Friendly description, Reserved for
        /// Exceptions raised Errors type 500
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// This is the technological Error, should only be
        /// used in non production environment.
        /// Must be empty in production
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Return a initializated error Manager
        /// </summary>
        /// <returns></returns>
        public static IErrorManager Factory()
        {
            IErrorManager manager = new ErrorManager
            {
                Errors = new List<ErrorResponse>()
            };
            return manager;
        }
    }
    /// <summary>
    /// Hold 400 type errors
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Can be a numeric code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the error, can be a
        /// portion of the stack trace.
        /// </summary>
        public string Description { get; set; }
    }
}
