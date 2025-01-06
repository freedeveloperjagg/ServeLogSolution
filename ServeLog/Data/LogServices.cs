using Microsoft.Data.SqlClient;
using ServeLog.Model;
using System.Collections.Generic;

namespace ServeLog.Data
{
    /// <summary>
    /// Call the necessary services to Manage the log table
    /// </summary>
    public class LogServices(CConfig xconfig) : ILogServices
    {
        private readonly CConfig cconfig = xconfig;

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
            var connString = cconfig.ApiLoggerDb;

            // Get the specific table to be logged
            if (model.Logger == null)
            {
                throw new KeyNotFoundException($"The used token is not a valid token: {model.Logger}");
            }
            var validToken = cconfig.Tables.TryGetValue(model.Logger, out string? table);
            if (!validToken)
            {
                throw new KeyNotFoundException($"The used token is not a valid token: {model.Logger}");
            }
            if (table == null)
            {
                throw new KeyNotFoundException($"The retrieved table is null");
            }
            InsertingLogRecord(model, connString, table);
        }

        /// <summary>
        /// Inserting the record
        /// </summary>
        /// <param name="model">The information to be logged</param>
        /// <param name="connString">The connection string to DB</param>
        /// <param name="table">The table to be used for log</param>
        internal static void InsertingLogRecord(LoggerModel model, string connString, string table)
        {
            // Sanitarize Model
            model = SanitizeLoggerModel.Execute(model);

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