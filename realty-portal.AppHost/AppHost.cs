using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

// 1. Define external resources (Aspire will run these in Docker automatically)
var redis = builder.AddRedis("cache");
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin() // Adds a UI for your DB
    .WithDataVolume("realty-portal-data"); // Persists data across restarts
var db = postgres.AddDatabase("sqldata");

// secret the RUIAN address point import route checks; generated once and kept in user secrets
var ruianImportKey = builder.AddParameter("ruian-import-key", new GenerateParameterDefault { MinLength = 32 },
    secret: true, persist: true);

// 2. Define your Server (API)
var api = builder.AddProject<Projects.Server>("backend-api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(redis)
    .WithEnvironment("Ruian__ImportKey", ruianImportKey)
    .WithExternalHttpEndpoints();

// dashboard command that loads the ČÚZK address points; header and route mirror Server.Features.Ruian
api.WithHttpCommand("/api/ruian/address-points", "Import RÚIAN addresses",
    endpointName: "http",
    commandName: "import-ruian-addresses",
    commandOptions: new HttpCommandOptions
    {
        Method = HttpMethod.Put,
        Description = "Loads the ČÚZK address points into the RÚIAN register, downloading this month's zip when none is on disk.",
        IconName = "ArrowDownload",
        Arguments =
        [
            new InteractionInput
            {
                Name = "force",
                Label = "Force download",
                InputType = InputType.Boolean,
                Description = "Download this month's zip even when one is already on disk."
            }
        ],
        PrepareRequest = async context =>
        {
            context.Request.Headers.Add("X-Import-Key", await ruianImportKey.Resource.GetValueAsync(context.CancellationToken));
            if (context.Arguments.TryGetByName("force", out var force) && bool.TryParse(force.Value, out var forced) && forced)
            {
                context.Request.RequestUri = new UriBuilder(context.Request.RequestUri!) { Query = "force=true" }.Uri;
            }
        },
        ResultMode = HttpCommandResultMode.Text,
        Progress = new CommandProgressOptions { Title = "RÚIAN import", Message = "Importing address points, this takes a few minutes." }
    });

// 3. Define your Blazor WASM Client
builder.AddProject<Projects.Client>("client")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
