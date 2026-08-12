var builder = DistributedApplication.CreateBuilder(args);

// 1. Define external resources (Aspire will run these in Docker automatically)
var redis = builder.AddRedis("cache");
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin() // Adds a UI for your DB
    .WithDataVolume("realty-portal-data"); // Persists data across restarts
var db = postgres.AddDatabase("sqldata");

// 2. Define your Server (API)
var api = builder.AddProject<Projects.Server>("backend-api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(redis)
    .WithExternalHttpEndpoints();

// 3. Define your Blazor WASM Client
builder.AddProject<Projects.Client>("client")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();