using System.Collections.Generic;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ServeLog.Model
{
    public class CorsSettings
    {
#pragma warning disable IDE0051 // Remove unused private members
        List<string> AllowedOrigin { get; set; } = ["*"];
        public string ActivePolicy { get; set; } = "OpenPolicy";
    }
}
