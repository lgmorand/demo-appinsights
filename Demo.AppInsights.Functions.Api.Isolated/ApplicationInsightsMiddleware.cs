using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Demo.AppInsights.Functions.Api.Isolated
{
    public class ApplicationInsightsMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsMiddleware()
        {
            var telemetryConfiguration = new TelemetryConfiguration
            {
                InstrumentationKey = "<your_instrumentation_key>"
            };
            _telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            //HttpContext httpContext = context.GetHttpContext()
            //   ?? throw new InvalidOperationException($"{nameof(context)} has no http context associated with it.");

            //context.GetHttpContext();

            //if (context.InvocationFeatures.Get<IHttpRequestFeature>() is IHttpRequestFeature httpRequestFeature)
            //{

            //    var reader = new StreamReader(httpRequestFeature.Body);
            //    var requestBody = await reader.ReadToEndAsync();
            //    var requestTelemetry = context.Features.Get<RequestTelemetry>();
            //    requestTelemetry?.Properties.Add("RequestBody", requestBody);

            //    // Reset the stream so it's readable again
            //    httpRequestFeature.Body.Position = 0;
            //}

            await next(context);
        }
    }
}
