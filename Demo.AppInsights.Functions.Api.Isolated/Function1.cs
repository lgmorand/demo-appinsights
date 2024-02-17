using System.Collections.Concurrent;
using System.Net;
using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Demo.AppInsights.Functions.Api.Isolated
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Welcome to Azure Functions Isolad!");

            // read data from Azure table storage
            //var serviceClient = new TableServiceClient(new Uri("https://stodemoappinsights.table.core.windows.net/fakedata"),
            //                                            new TableSharedKeyCredential("stodemoappinsights", "M4TS3SqgZySrqXUcOqlHV9jSqsgTy2vK3PkGIjhER7GkWvrNX1Hm3xnxRAF22JGRlTx2TdIKMS1i+AStSrYzbQ=="));

            //serviceClient.GetTableClient("fakedata").CreateIfNotExists();
           
            return response;
        }
    }
}
