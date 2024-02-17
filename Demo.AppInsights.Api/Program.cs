using Demo.AppInsights.Api.Telemetry;
using Demo.AppInsights.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();
//builder.Services.AddSingleton<ITelemetryInitializer, RequestBodyAndResponseBodyInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRequestBodyLogging();
    app.UseResponseBodyLogging();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();