namespace Serilog.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Serilog.Configuration;
    using Serilog.Core;
    using Serilog.Exceptions.Core;
    using Serilog.Exceptions.Destructurers;
    using Serilog.Exceptions.Filters;

    public static class LoggerEnrichmentConfigurationExtensions
    {
        public static Serilog.LoggerConfiguration WithExceptionDetails(
            this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration)
        {
            return loggerEnrichmentConfiguration.With(new ExceptionEnricher());
        }

        public static Serilog.LoggerConfiguration WithExceptionDetails(
            this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration,
            params IExceptionDestructurer[] destructurers)
        {
            return loggerEnrichmentConfiguration.With(new ExceptionEnricher(destructurers));
        }

        public static Serilog.LoggerConfiguration WithExceptionDetails(
            this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration,
            IEnumerable<IExceptionDestructurer> destructurers,
            IExceptionPropertyFilter filter = null,
            IDestructuringOptions destructuringOptions = null)
        {
            ILogEventEnricher enricher = new ExceptionEnricher(
                destructurers,
                filter: filter,
                destructuringOptions: destructuringOptions);
            return loggerEnrichmentConfiguration.With(enricher);
        }

        public static Serilog.LoggerConfiguration WithProperties(
            this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration,
            IDictionary<string, object> properties)
        {
            if (loggerEnrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerEnrichmentConfiguration));
            }

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (properties.Count == 0)
            {
                throw new ArgumentException("Logger properties count cannot be zero.", nameof(properties));
            }

            Serilog.LoggerConfiguration loggerConfiguration = null;

            foreach (var property in properties)
            {
                loggerConfiguration = loggerEnrichmentConfiguration.WithProperty(property.Key, property.Value);
            }

            return loggerConfiguration;
        }

        public static Serilog.LoggerConfiguration WithLazyProperties(
            this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration,
            IDictionary<string, Func<object>> properties)
        {
            if (loggerEnrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerEnrichmentConfiguration));
            }

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (properties.Count == 0)
            {
                throw new ArgumentException("Logger properties count cannot be zero.", nameof(properties));
            }

            Serilog.LoggerConfiguration loggerConfiguration = null;

            foreach (var property in properties)
            {
                loggerConfiguration = loggerEnrichmentConfiguration.WithProperty(property.Key, property.Value());
            }

            return loggerConfiguration;
        }
    }
}
