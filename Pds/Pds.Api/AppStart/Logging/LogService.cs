using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using Pds.Data;
using Pds.Data.Entities;
using static System.DateTime;

namespace Pds.Api.AppStart.Logging
{
    [Target("LogService")]
    public class LogService : AsyncTaskTarget
    {
        private IServiceScope scope;
        private IUnitOfWork unitOfWork;

        [DefaultValue("${message}")] public Layout Message { get; set; } = "${message}";

        [DefaultValue("${exception:format=tostring}")]
        public Layout Exception { get; set; } = "${exception:format=tostring}";

        [DefaultValue("${callsite}")] public Layout CallSite { get; set; } = "${callsite}";

        [DefaultValue("${logger}")] public Layout Logger { get; set; } = "${logger}";


        [DefaultValue("${level}")] public Layout Level { get; set; } = "${level}";

        [DefaultValue(@"${date:universalTime=true:format=yyyy-MM-ddTHH\:mm\:ss.fff}")]
        public Layout Time { get; set; } = @"${date:universalTime=true:format=yyyy-MM-ddTHH\:mm\:ss.fff}";

        public void SetServiceProvider(IServiceProvider value)
        {
            scope = value.CreateScope();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        }

        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            await unitOfWork.Logs.InsertAsync(
                    new LogRecord
                    {
                        Message = Message.Render(logEvent),
                        Exception = Exception.Render(logEvent),
                        CallSite = CallSite.Render(logEvent),
                        Logger = Logger.Render(logEvent),
                        Level = Level.Render(logEvent),
                        CreatedAt = TryParse(Time.Render(logEvent), out var created)
                            ? created
                            : UtcNow
                    });
        }

        protected override async Task WriteAsyncTask(IList<LogEventInfo> logEvents, CancellationToken cancellationToken)
        {
            await unitOfWork.Logs.InsertRangeAsync(
                    logEvents.Select(logEvent =>
                        new LogRecord
                        {
                            Message = Message.Render(logEvent),
                            Exception = Exception.Render(logEvent),
                            CallSite = CallSite.Render(logEvent),
                            Logger = Logger.Render(logEvent),
                            Level = Level.Render(logEvent),
                            CreatedAt = TryParse(Time.Render(logEvent), out var created)
                                ? created
                                : UtcNow
                        }).ToList());

        }

        protected override void CloseTarget()
        {
            unitOfWork?.Dispose();
            scope?.Dispose();
        }
    }
}