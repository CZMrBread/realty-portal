# Realty portal
A realty portal that imports adverts in the [Sreality.cz](https://www.sreality.cz/) format.

## Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Node.js](https://nodejs.org/en/download/)
- [Docker](https://www.docker.com/products/docker-desktop)

## Configuration
The server settings are in [Server/appsettings.json](Server/appsettings.json).

For uploading photos, you can specify the storage path and other settings in the `PhotoStorage` section of the configuration file:
```json
"PhotoStorage": {
    "RootPath": "data/photos",
    "MaxLongEdge": 1920,
    "JpegQuality": 85,
    "MaxSourceEdge": 12000,
    "MaxUploadBytes": 26214400
},
```

For the RÚIAN address point import, you can specify where the downloaded ČÚZK zips are kept in the `Ruian` section
of the configuration file. A relative path is taken from the Server content root. The `ImportKey` is supplied by the
Aspire AppHost as an environment variable, so it is not in the file:
```json
"Ruian": {
    "AddressDataPath": "Features/Ruian/Data/Addresses"
},
```

For JWT authentication, you can specify the key, issuer, and audience in the `Jwt` section of the configuration file:
> [!WARNING]
> The key should be a strong, random string.
> The issuer and audience should be set to the appropriate values for your application.
```json
"Jwt": {
    "Key": "key-for-jwt-authentication-should-be-strong-and-random",
    "Issuer": "backend-api",
    "Audience": "backend-api"
}
```

## Usage
The solution is a .NET Aspire application. The AppHost project starts everything: PostgreSQL, pgAdmin and Redis as Docker containers, then the Server and the Client.
1. Clone the repository:
   ```bash
   git clone https://github.com/CZMrBread/realty-portal.git
   ```
1. Start Docker.
1. Trust the local HTTPS development certificate (once per machine):
   ```bash
   dotnet dev-certs https --trust
   ```
1. Install the client's Node.js packages (Bootstrap, Bootstrap Icons and Leaflet).
   The Client build copies them from `node_modules` into `wwwroot/lib`, so it fails without this step:
   ```bash
   cd Client
   npm install
   cd ..
   ```
1. Restore the packages and build the solution from the repository root:
   ```bash
   dotnet restore
   dotnet build
   ```
1. Run the AppHost:
   ```bash
   dotnet run --project realty-portal.AppHost
   ```

The Aspire dashboard opens in the browser (https://localhost:17041).
It lists every resource with its state, logs and endpoints.
Open the portal through the `client` endpoint and the API through the `backend-api` endpoint.

On the first run the containers are pulled, the database is created,
migrations are applied and the RÚIAN region,
district and municipality tables are filled from the embedded CSV files.
The database lives in the `realty-portal-data` Docker volume,
so the data are persisted between restarts.

### RÚIAN address import
The regions, districts and municipalities come with the Server. The municipality parts, streets and address points
are much larger, so they are loaded on demand from the ČÚZK monthly export.

In the Aspire dashboard, open the `backend-api` resource and run the "Import RÚIAN addresses" command.
The command sends the `X-Import-Key` header with the generated `ruian-import-key` secret.
The import takes a few minutes. The result lists how many rows of each table were inserted, updated and deleted.

The `ruian-import-key` secret is generated on the first run and kept in the AppHost user secrets.
It protects the import route from unauthorized calls; the route is disabled when no key is configured.

## Structure
The whole project is structured as a solution with multiple projects.
The main projects are:
- [Server](Server/README.md) - Backend code (ASP.NET Core)
- [Client](Client/README.md) - Frontend code (Blazor WebAssembly)
- [Shared](Shared/README.md) - Shared code (DTOs, Enums, etc.)
- [AppHost](realty-portal.AppHost) - The Aspire AppHost that runs the containers and both applications
- [ServiceDefaults](realty-portal.ServiceDefaults) - Telemetry, health checks and resilience shared by the projects Aspire runs

`Server` and `Client` are separate projects and are not coupled together.
For shared data structures, the `Shared` project is used. The `Shared` project is referenced by both the `Server` and `Client` projects.
