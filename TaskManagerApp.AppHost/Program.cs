using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add Redis
var redis = builder.AddRedis("redis");
// Add Backend API
var api = builder.AddProject<TaskApi>("taskapi")
    .WithReference(redis);

// Add Frontend
builder.AddProject<TaskWeb>("taskweb") 
    .WithReference(api);

builder.Build().Run();
