using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WY.Data.Utility.Caching;

namespace WY.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICacheManager _cacheManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ICacheManager cacheManager
            )
        {
            _cacheManager = cacheManager;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(bool isCache=true)
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            string data = "TestCacheGet001";
            string cacheKey = "testcacheget001";
            var cacheResultData = _cacheManager.Get<WeatherForecast>(cacheKey, () => new WeatherForecast { Date=DateTime.Now, Summary="asdfasdf", TemperatureC=100 });
            return new List<WeatherForecast> { cacheResultData };
        }

       
    }
}
