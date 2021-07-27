using Microsoft.Extensions.Configuration;
using ServeLog.InternalLogger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using WapiLogger.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestIntegration
{
    public class TestDataBase
    {
        private readonly ITestOutputHelper output;

        public TestDataBase(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestInternalDbProcedure()
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.Test.json", optional: true)
               .Build();

            // Create the class
            IDataInternal data = new DataInternal(configuration);

            // Create the data
            var date = DateTime.UtcNow;
            LoggerModel model = new()
            {
                Date = date,
                Exception = string.Empty,
                Level = "ALWAYS",
                Logger = "Internal Test",
                MachineName = Environment.MachineName,
                Message = "This is a test"
            };

            // Call the test
            data.WriteRecordInLog(model);

            // Assert
            string strConn = configuration.GetConnectionString("InternalLoggerDb");
            string sql = $"SELECT * FROM ServeLog WHERE Date = @Param";
            SqlConnection conn = new(strConn);
            SqlCommand command = new(sql, conn);
            command.Parameters.AddWithValue("@Param", model.Date);
            conn.Open();
            try
            {
                List<LoggerModel> modelList = new();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var readm = new LoggerModel()
                    {
                        Date = Convert.ToDateTime(reader["Date"]),
                        Exception = reader["Exception"].ToString(),
                        Level = reader["Level"].ToString(),
                        Logger = reader["Logger"].ToString(),
                        MachineName = reader["MachineName"].ToString(),
                        Message = reader["Message"].ToString()
                    };
                    modelList.Add(readm);
                }

                Assert.True(modelList.Count == 1, $"Return result: {modelList.Count}");
                var m = modelList[0];
                Assert.True(m.Message == model.Message, $"Message not equal: {m.Message}");
                Assert.True(m.Exception == model.Exception);
                Assert.True(m.MachineName == model.MachineName);
                Assert.True(m.Level == model.Level);
                Assert.True(m.Logger == model.Logger);
                output.WriteLine("Object Successful");
            }
            catch (Exception ex)
            {
                Assert.True(false, $"A exception happen {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
