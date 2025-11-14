using Microsoft.AspNetCore.Mvc;
using Nest;
using StackExchange.Redis;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDatabase _redisDb;
        private readonly IElasticClient _esClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConnectionMultiplexer redis, IElasticClient esClient)
        {
            _logger = logger;
            _redisDb = redis.GetDatabase();
            _esClient = esClient;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Example: store count in Redis
            var count = (_redisDb.StringIncrement("weather_count")).ToString();

            // Example: index something in Elasticsearch
            var indexResponse = _esClient.IndexDocument(new { Message = $"Weather API hit {count}", Timestamp = DateTime.UtcNow });

            return new string[] { $"Request count: {count}", $"Elasticsearch status: {indexResponse.IsValid}" };
        }
    }
}
