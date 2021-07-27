using Microsoft.Extensions.Configuration;
using Moq;
using ServeLog.InternalLogger;
using System;
using System.IO;
using WapiLogger.Models;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    /// <summary>
    /// Test the internal logger
    /// </summary>
    public class UnitTestModelInternal
    {
        private readonly ITestOutputHelper output;

        public UnitTestModelInternal(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Test with Time zone to UTC
        /// </summary>
        [Fact]
        public void TestUtc()
        {
            // Get the settings for test
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.utc.json", optional: true)
               .Build();
            // Get the mock
            var m = new MockHelper();
            // Setup the mock
            m.MockDataInternal.Setup(x => x.WriteRecordInLog(It.IsAny<LoggerModel>()));

            // Create the object to be test
            LogControlInternal log = new(
                m.MockDataInternal.Object,
                configuration);

            // Run the test
            var now = DateTime.UtcNow;
            var result = log.InternalAlwaysWriteLog("This Always should comming");
            // Get result
            Assert.True(result.Date - now < new TimeSpan(0, 0, 2));
            output.WriteLine($"{result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");
        }

        /// <summary>
        /// Test with Internal Zone to Easter Time
        /// </summary>
        [Fact]
        public void TestEasterTime()
        {
            // Get the settings for test
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.easter.json", optional: true)
               .Build();
            // Get the mock
            var m = new MockHelper();
            // Setup the mock
            m.MockDataInternal.Setup(x => x.WriteRecordInLog(It.IsAny<LoggerModel>()));

            // Create the object to be test
            LogControlInternal log = new(
                m.MockDataInternal.Object,
                configuration);

            // Run the test

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            var result = log.InternalAlwaysWriteLog("This Always should comming");
            // Get result
            Assert.True(result.Date - now < new TimeSpan(0, 0, 2));
            output.WriteLine($"{result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");
        }

        /// <summary>
        /// Test Internal Log Level to DEBUG
        /// </summary>
        [Fact]
        public void TestInternalLogLevelDebug()
        {
            // Get the settings for test
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.utc.json", optional: true)
               .Build();
            // Get the mock
            var m = new MockHelper();
            // Setup the mock
            m.MockDataInternal.Setup(x => x.WriteRecordInLog(It.IsAny<LoggerModel>()));

            // Create the object to be test
            LogControlInternal log = new(
                m.MockDataInternal.Object,
                configuration);

            // Run the test
            var now = DateTime.UtcNow;
            var result = log.InternalAlwaysWriteLog("This Always should comming");
            // Get result
            Assert.True(result.Date - now < new TimeSpan(0, 0, 2));
            Assert.True(result != null);
            Assert.True(result.Level == "ALWAYS");
            output.WriteLine($"Always: {result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");

            result = log.InternalDebugWriteLog("This Only Comming in Debug", null);
            Assert.True(result != null, "Log should return value");
            output.WriteLine($"DEBUG: {result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");
            Assert.True(result.Level == "DEBUG");

            result = log.InternalErrorWriteLog("This Comming in DEBUG & ERROR", null);
            Assert.True(result != null);
            output.WriteLine($"ERROR: {result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");
            Assert.True(result.Level == "ERROR");
        }

        /// <summary>
        /// Test internal server Error Level
        /// </summary>
        [Fact]
        public void TestInternalLogLevelERROR()
        {
            // Get the settings for test
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.easter.json", optional: true)
               .Build();
            // Get the mock
            var m = new MockHelper();
            // Setup the mock
            m.MockDataInternal.Setup(x => x.WriteRecordInLog(It.IsAny<LoggerModel>()));

            // Create the object to be test
            LogControlInternal log = new(
                m.MockDataInternal.Object,
                configuration);

            // Run the test
            var result = log.InternalAlwaysWriteLog("This Always should comming");
            // Get result
            Assert.True(result.Level == "ALWAYS");
            output.WriteLine($"Always: {result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");

            result = log.InternalDebugWriteLog("This Only Comming in Debug", null);
            Assert.True(result == null, "No value should be returned");

            result = log.InternalErrorWriteLog("This Comming in DEBUG & ERROR", null);
            Assert.True(result != null);
            output.WriteLine($"ERROR: {result.Date}, {result.MachineName}, {result.Level}, {result.Logger}, {result.Message}");
            Assert.True(result.Level == "ERROR");
        }

        /// <summary>
        /// Test Exception management
        /// </summary>
        [Fact]
        public void TestInternalLogExceptionManager()
        {
            try
            {
                // Get the settings for test
                IConfiguration configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .AddJsonFile("appsettings.easter.json", optional: true)
                   .Build();
                // Get the mock
                var m = new MockHelper();
                // Setup the mock
                m.MockDataInternal.Setup(x => x.WriteRecordInLog(It.IsAny<LoggerModel>()))
                    .Throws(new Exception());

                // Create the object to be test
                LogControlInternal log = new(
                    m.MockDataInternal.Object,
                    configuration);

                // Run the test
                var result = log.InternalAlwaysWriteLog("This not comes because the exception");
                // Get result
                Assert.False(true, "Should not go to here");

            }
            catch (Exception)
            {
                Assert.True(true, "Should goes to here");
            }
        }
    }
}
