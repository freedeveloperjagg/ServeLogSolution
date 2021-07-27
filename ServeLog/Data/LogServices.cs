using Microsoft.Extensions.Configuration;
using ServeLog.Model;
using System;
using System.Data.SqlClient;
using WapiLogger.Models;

namespace ServeLog.Data
{
    /// <summary>
    /// Call the necessary services to Manage the log table
    /// </summary>
    public class LogServices : ILogServices
    {
        private readonly IConfiguration configuration;

        public LogServices(IConfiguration configuration)
        {
            this.configuration = configuration;
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
            // Call conection string
            var connString = configuration.GetConnectionString("ApiLoggerDb");

            // Get the specific table to be logged
            var validToken = Settings.LogTables.TryGetValue(model.Logger, out string table);
            if (!validToken)
            {
                throw new ApplicationException($"The used token is not a valid token: {model.Logger}");
            }

            this.InsertingLogRecord(model, connString, table);
        }

        /// <summary>
        /// Inserting the record
        /// </summary>
        /// <param name="model">The information to be logged</param>
        /// <param name="connString">The connection string to DB</param>
        /// <param name="table">The table to be used for log</param>
        public void InsertingLogRecord(LoggerModel model, string connString, string table)
        {
            // Sanitarize Model
            model = SatinizeLoggerModel.Execute(model);
            
            // Prepare query
            var sql = $@"INSERT INTO {table} 
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