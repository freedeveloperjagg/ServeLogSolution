using System;
using System.Collections.Generic;

namespace ServeLog.Model
{
    public interface IErrorManager
    {
        DateTime DateCreated { get; set; }
        string StackTrace { get; set; }
        string Description { get; set; }
        List<ErrorResponse> Errors { get; set; }
    }
}