namespace Servelog.Infrastructure.HealthCheck.Helpers
{
    /// <summary>
    /// Memory Check Options
    /// </summary>
    public class MemoryCheckOptions
    {
        /// <summary>
        /// Failure Threshold (bytes)
        /// </summary>
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}
