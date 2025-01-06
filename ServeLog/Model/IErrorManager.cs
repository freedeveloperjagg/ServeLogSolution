using System;
using System.Collections.Generic;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ServeLog.Model
{
    public interface IErrorManager
    {
        DateTime DateCreated { get; set; }
        string? StackTrace { get; set; }
        string? Description { get; set; }
        List<ErrorResponse> Errors { get; set; }
    }
}