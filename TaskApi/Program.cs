using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Register IConnectionMultiplexer
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});

// Add Redis cache as a service
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Add CORS to allow the frontend to call the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Endpoints
app.MapGet("/tasks", async ([FromServices] IConnectionMultiplexer redis) =>
{
    var tasks = await TaskHelper.GetTasksAsync(redis);
    return Results.Ok(tasks);
});

app.MapPost("/tasks", async ([FromServices] IConnectionMultiplexer redis, [FromBody] TaskItem task) =>
{
    var db = redis.GetDatabase();
    var json = JsonSerializer.Serialize(task);
    await db.StringSetAsync(task.Id, json);
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id}", async ([FromServices] IConnectionMultiplexer redis, string id, [FromBody] TaskItem updatedTask) =>
{
    var db = redis.GetDatabase();
    var json = JsonSerializer.Serialize(updatedTask);
    await db.StringSetAsync(id, json);
    return Results.Ok(updatedTask);
});

app.Run();

// Task model
public record TaskItem(string Id, string Description, bool IsCompleted);

// Redis helper
public static class TaskHelper
{
    public static async Task<List<TaskItem>> GetTasksAsync(IConnectionMultiplexer redis)
    {
        var db = redis.GetDatabase();
        var keys = redis.GetServer(redis.GetEndPoints()[0]).Keys();
        var tasks = new List<TaskItem>();
        foreach (var key in keys)
        {
            var json = await db.StringGetAsync(key);
            if (!json.IsNullOrEmpty)
            {
                var task = JsonSerializer.Deserialize<TaskItem>(json);
                if (task != null) tasks.Add(task);
            }
        }
        return tasks;
    }
}
