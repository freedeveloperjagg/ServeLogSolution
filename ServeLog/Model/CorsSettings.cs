using System.Collections.Generic;

namespace ServeLog.Model
{
    public class CorsSettings
    {
        List<string> AllowedOrigin { get; set; }
        public string ActivePolicy { get; set; }
    }
}
