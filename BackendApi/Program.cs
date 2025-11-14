using Nest;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetValue<string>("Redis:Connection") ?? "redis:6379";
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var settings = new ConnectionSettings(new Uri(builder.Configuration.GetValue<string>("Elasticsearch:Uri") ?? "http://es01:9200"))
        .DefaultIndex("backend-logs");
    return new ElasticClient(settings);
});

var app = builder.Build();

app.MapControllers();

app.Run();
