﻿using Microsoft.Extensions.Logging;

namespace Demo.AppInsights.Framework
{
    public static class ApplicationInsightsLoggerFactoryExtensions
    {
        public static ILoggerFactory AddApplicationInsights(
           this ILoggerFactory factory,
           Func<string, LogLevel, bool> filter,
           ApplicationInsightsSettings settings)
        {
            factory.AddProvider(new ApplicationInsightsLoggerProvider(filter, settings));
            return factory;
        }

        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory factory,
            ApplicationInsightsSettings settings)
        {
            factory.AddProvider(new ApplicationInsightsLoggerProvider(null, settings));

            return factory;
        }
    }
}