using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Azure.Core;

namespace Demo.AppInsights.Functions.Api.Isolated
{
    public class ApplicationInsightsMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ApplicationInsightsMiddleware> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsMiddleware(ILogger<ApplicationInsightsMiddleware> logger)
        {
            _logger = logger;
            var telemetryConfiguration = new TelemetryConfiguration
            {
                ConnectionString = "InstrumentationKey=35d75450-c663-4b81-9a31-dac81bb02625;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/"
            };
            _telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var logger =  context.GetLogger(context.FunctionDefinition.Name);
            try
            {
                var requestData = await context.GetHttpRequestDataAsync();
                var body = await new StreamReader(requestData.Body).ReadToEndAsync();

                // une trace avec le logger
                logger.LogInformation("Request Body: {0} using logger", body);
               
                // une trace
                _telemetryClient.Context.GlobalProperties["RequestBody"] = body;
                _telemetryClient.TrackTrace("Request Body using tele2: ");

                // une request
                RequestTelemetry requestTelemetry = new RequestTelemetry();
                requestTelemetry.Name = context.FunctionDefinition.Name;
                requestTelemetry.Url = requestData.Url;
                requestTelemetry.Timestamp = DateTimeOffset.UtcNow;
                _telemetryClient.TrackRequest(requestTelemetry);


                    await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError("Unexpected Error in {0}: {1}", context.FunctionDefinition.Name, ex.Message);
            }

        }
    }
}
