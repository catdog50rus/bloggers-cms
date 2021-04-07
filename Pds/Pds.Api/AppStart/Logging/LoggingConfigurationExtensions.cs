using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Pds.Api.Logging.NLogTargets;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Pds.Api.AppStart.Logging
{
    public static class LoggingConfigurationExtensions
    {
        private const string LoggingSectionName = "CustomLogging";

        public static IHostBuilder ConfigureCustomLoggingLogic(this IHostBuilder builder)
        {
            return builder.ConfigureLogging(ConfigureLogging);
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            var configuration = context.Configuration;
            var environment = context.HostingEnvironment;
            builder.ClearProviders();
            builder.SetMinimumLevel(
                environment.IsProduction() ? LogLevel.Information : LogLevel.Trace);
            builder.AddNLog(BuildLogFactory);
        }

        private static LogFactory BuildLogFactory(IServiceProvider provider)
        {
            var loggingConfiguration = new LoggingConfiguration();
            var configuration = provider
                .GetRequiredService<IConfiguration>();
            configuration.GetSection(LoggingSectionName)
                .Bind(loggingConfiguration);
            Target.Register<RepositoryLogTarget>(nameof(RepositoryLogTarget));

            var logFactory = new LogFactory
            {
                Configuration =
                    new XmlLoggingConfiguration(loggingConfiguration.NLogConfig),
                ThrowExceptions = true
            };

            foreach (var serviceProviderDynamicLink in logFactory.Configuration.AllTargets
                .OfType<IServiceProviderDynamicLink>()) serviceProviderDynamicLink.SetServiceProvider(provider);

            return logFactory;
        }


        private class LoggingConfiguration
        {
            /// <summary>
            ///     NLog config file path
            /// </summary>
            public string NLogConfig { get; set; }
        }
    }
}