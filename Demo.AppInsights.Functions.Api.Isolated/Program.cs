using Demo.AppInsights.Functions.Api.Isolated;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ApplicationInsightsMiddleware>();
    })
    .ConfigureLogging((context, builder) =>
    {
        // does not work as per https://github.com/Azure/azure-functions-dotnet-worker/issues/423
        //builder.AddApplicationInsights();

        var applicationInsightsConnectionString = context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (!string.IsNullOrEmpty(applicationInsightsConnectionString))
        {
            builder.AddApplicationInsights(configureTelemetryConfiguration =>
            {
                configureTelemetryConfiguration.ConnectionString = applicationInsightsConnectionString;
            }, _ => { });
        }
    })
    //.ConfigureServices(services => {
    //    services.AddApplicationInsightsTelemetryWorkerService();
    //    services.ConfigureFunctionsApplicationInsights();
    //})

    .Build();


host.Run();