using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WapiLogger.Models;

namespace ServeLog.InternalLogger
{
    /// <summary>
    /// Call the necessary services to Manage the log table
    /// </summary>
    public class DataInternal : IDataInternal
    {
        private readonly string connString;
        private readonly string table;

        public DataInternal(IConfiguration configuration)
        {
            this.connString = configuration.GetConnectionString("InternalLoggerDb");
            this.table = configuration["InternalLogSettings:Table"];
        }

        /// <summary>
        /// Write the log in the table. The table is selected by the
        /// value of the appConfig that match with request.Logger
        /// </summary>
        /// <param name="model">
        /// Logger: Must match the AppSetting with the name of the table to  log
        /// or the method fail.
        /// </param>
        public void WriteRecordInLog(LoggerModel model)
        {
            // Sanitarize Model
            model = SatinizeLoggerModel.Execute(model);

            // Prepare query
            var sql = $@"INSERT INTO {this.table} 
                        ([Date],[MachineName],[Level],[Logger],[Message],[Exception])
                        VALUES
                        (
                        @Date,
                        @MachineName,
                        @Level,
                        @Logger,
                        @Message,
                        @Exception
                        )";
            var conn = new SqlConnection(connString);
            var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@Date", model.Date);
            command.Parameters.AddWithValue("@MachineName", model.MachineName);
            command.Parameters.AddWithValue("@Level", model.Level);
            command.Parameters.AddWithValue("@Logger", model.Logger);
            command.Parameters.AddWithValue("@Message", model.Message);
            command.Parameters.AddWithValue("@Exception", model.Exception);
            // Open The connexion...
            conn.Open();
            try
            {
                var result = command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

    }
}