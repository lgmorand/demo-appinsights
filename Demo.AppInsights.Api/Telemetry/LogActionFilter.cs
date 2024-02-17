using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Demo.AppInsights.Api
{
    public class HideActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<HideActionFilter> _logger;

        public HideActionFilter(
            ILogger<HideActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext is null)
            {
                await next();
                return;
            }

            try
            {
                string fromBodyParameter = context.ActionDescriptor.Parameters
                    .Where(item => item.BindingInfo.BindingSource == BindingSource.Body)
                    .FirstOrDefault()
                    ?.Name.ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(fromBodyParameter))
                    return;

                KeyValuePair<string, object> arguments = context.ActionArguments
                    .First(x => x.Key.ToUpperInvariant() == fromBodyParameter);

                string argument = SerializeDataUsingObfuscator(arguments.Value);

                RequestTelemetry request = context.HttpContext.Features.Get<RequestTelemetry>();
                request.Properties.Add("RequestBody", argument);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error executing the response logger task");
            }
            finally
            {
                await next();
            }
        }

        private static string SerializeDataUsingObfuscator(object value) =>
       JsonConvert.SerializeObject(value,
           new JsonSerializerSettings
           {
               ContractResolver = new ObfuscatorContractResolver()
           });

    }
}
