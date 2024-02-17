using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace Demo.AppInsights.Framework
{
    public static class LoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception exception, string errorId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var telemetryClient = GetTelemetryClient(logger);

            properties = FillProperties(properties, null, errorId, component);

            telemetryClient.TrackException(exception, properties);
        }

        public static void LogTrace(this ILogger logger, string message, Severity severity, string eventId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var telemetryClient = GetTelemetryClient(logger);
            var sev = default(SeverityLevel);

            properties = FillProperties(properties, eventId, null, component);

            switch (severity)
            {
                case Severity.Critical:
                    sev = SeverityLevel.Critical;
                    break;

                case Severity.Error:
                    sev = SeverityLevel.Error;
                    break;

                case Severity.Warning:
                    sev = SeverityLevel.Warning;
                    break;

                default:
                    sev = SeverityLevel.Information;
                    break;
            }

            telemetryClient.TrackTrace(message, sev, properties);
        }

        public static void LogInformation(this ILogger logger, string message, string eventId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var severity = Severity.Information;

            LogTrace(logger, message, severity, eventId, component, properties);
        }

        public static void LogWarning(this ILogger logger, string message, string eventId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var severity = Severity.Warning;

            LogTrace(logger, message, severity, eventId, component, properties);
        }

        public static void LogError(this ILogger logger, string message, string eventId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var severity = Severity.Error;

            LogTrace(logger, message, severity, eventId, component, properties);
        }

        public static void LogCritical(this ILogger logger, string message, string eventId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var severity = Severity.Critical;

            LogTrace(logger, message, severity, eventId, component, properties);
        }

        public static void LogMetric(this ILogger logger, string name, double value, string component = null, Dictionary<string, string> properties = null)
        {
            var telemetryClient = GetTelemetryClient(logger);

            properties = FillProperties(properties, null, null, component);

            telemetryClient.TrackMetric(name, value, properties);
        }

        public static void LogDependency(this ILogger logger, string dependencyName, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            var telemetryClient = GetTelemetryClient(logger);

            telemetryClient.TrackDependency(dependencyName, commandName, startTime, duration, success);
        }

        public static void LogDependency(this ILogger logger, string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
        {
            var telemetryClient = GetTelemetryClient(logger);

            telemetryClient.TrackDependency(dependencyTypeName, target, dependencyName, data, startTime, duration, resultCode, success);
        }

        public static void LogPageView(this ILogger logger, string name)
        {
            var telemetryClient = GetTelemetryClient(logger);

            telemetryClient.TrackPageView(name);
        }

        public static void LogRequest(this ILogger logger, string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            var telemetryClient = GetTelemetryClient(logger);

            telemetryClient.TrackRequest(name, startTime, duration, responseCode, success);
        }

        public static void LogEvent(this ILogger logger, string name, Dictionary<string, double> metrics, string eventId = null, string component = null, Dictionary<string, string> properties = null)
        {
            var telemetryClient = GetTelemetryClient(logger);

            properties = FillProperties(properties, eventId, null, component);

            telemetryClient.TrackEvent(name, properties, metrics);
        }

        private static Dictionary<string, string> FillProperties(Dictionary<string, string> properties, string eventId, string errorId, string component)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            if (!string.IsNullOrEmpty(eventId))
            {
                properties.Add(nameof(eventId), eventId);
            }

            if (!string.IsNullOrEmpty(errorId))
            {
                properties.Add(nameof(errorId), errorId);
            }

            if (!string.IsNullOrEmpty(component))
            {
                properties.Add(nameof(component), component);
            }

            return properties;
        }

        private static TelemetryClient GetTelemetryClient(ILogger logger)
        {
            if (logger is ApplicationInsightsLogger ail)
            {
                return ail.TelemetryClient;
            }
            else
            {
                return new TelemetryClient();
            }
        }
    }
}