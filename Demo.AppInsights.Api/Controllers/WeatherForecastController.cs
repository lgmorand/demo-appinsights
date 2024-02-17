using Microsoft.AspNetCore.Mvc;

namespace Demo.AppInsights.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[TypeFilter(typeof(HideActionFilter))]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            // call a rest api GET method
            var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:7234/api/Function1");
            var content = response.Content.ReadAsStringAsync();
            _logger.LogInformation("GET method called");
            return null;

            var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = rng.Next(-20, 0),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }

        [TypeFilter(typeof(HideActionFilter))]
        [HttpPost]
        public void CreateUser([FromBody]User usr)
        {
            _logger.LogInformation("User created");
        }
    }
}