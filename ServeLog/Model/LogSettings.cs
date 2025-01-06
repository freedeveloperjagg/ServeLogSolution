namespace ServeLog.Model
{
    /// <summary>
    /// Settings to configure the Logger works
    /// </summary>
    public class LogSettings
    {
        /// <summary>
        /// true the logger time is set by itself
        /// false the logger time must comes from the client
        /// </summary>
        public bool UseInternalDateTime { get; set; }

        /// <summary>
        /// The Datetime zone use: Values can be:
        /// UTC, Eastern Standard Time, or other valid zone.
        /// </summary>
        public string TimeZone { get; set; } = "UTC";

        /// <summary>
        /// Time set to cancel a pending operation. Important to avoid freeze the logger
        /// </summary>
        public int TaskCancellationTimeMs { get; set; }

    }
}
