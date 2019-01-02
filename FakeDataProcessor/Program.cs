using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.AwsCloudWatch;

namespace FakeDataProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            InitialiseLogging();
            Log.Information("FakeDataProcessor Starting!");

            DoSomeFakeDataProcessingWork();

            Log.Information("FakeDataProcessor Closing!");
        }

        private static void DoSomeFakeDataProcessingWork()
        {
            foreach (var r in Enumerable.Range(1, 10))
            {
                Log.Information($"Wait noobs, I'm processing file {r}");
                Thread.Sleep(200);
                Log.Information($"File {r} processed OK.");
            }

            Log.Information("All files processed.");
        }

        private static void InitialiseLogging()
        {
            // name of the log group
            var logGroupName = "awiddowson/ContainerTest";

// customer formatter
            //var formatter = new MyCustomTextFormatter();

// options for the sink defaults in https://github.com/Cimpress-MCP/serilog-sinks-awscloudwatch/blob/master/src/Serilog.Sinks.AwsCloudWatch/CloudWatchSinkOptions.cs
            var options = new CloudWatchSinkOptions
            {
                // the name of the CloudWatch Log group for logging
                LogGroupName = logGroupName,

                //// the main formatter of the log event
                //TextFormatter = formatter,
  
                // other defaults defaults
                MinimumLogEventLevel = LogEventLevel.Information,
                BatchSizeLimit = 100,
                QueueSizeLimit = 10000,
                Period = TimeSpan.FromSeconds(10),
                CreateLogGroup = true,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 5
            };

// setup AWS CloudWatch client

//            var client = new AmazonCloudWatchLogsClient();

// Attach the sink to the logger configuration
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
               // .WriteTo.AmazonCloudWatch(options, client)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
