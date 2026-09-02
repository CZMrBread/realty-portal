# Realty portal
A realty portal that imports adverts in the [Sreality.cz](https://www.sreality.cz/) format.

This file is the user guide: how to run the portal and how to work with it as a visitor, an agent, an agency,
an integrating system or an administrator. Developers start with [docs/Developer.md](docs/Developer.md).

## Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Node.js](https://nodejs.org/en/download/)
- [Docker](https://www.docker.com/products/docker-desktop)

## Configuration
The server settings are in [Server/appsettings.json](Server/appsettings.json). The PostgreSQL and Redis
connection strings are not there: the Aspire AppHost supplies them when it starts the server.

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
`RootPath` is taken from the Server content root when relative. Every uploaded image is re-encoded as JPEG,
scaled down so that its longer side is at most `MaxLongEdge` pixels, and stored under
`RootPath/<advert id>/`. An upload larger than `MaxUploadBytes`, or declaring a side longer than
`MaxSourceEdge`, is refused. Deleting an advert removes its folder.

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

The client reads the address of the API from `Client/wwwroot/appsettings.json`:
```json
{
  "ServerAPI": "http://localhost:5094/api/"
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
If the application is runned for the first time, you need to run RÚIAN address import to fill the rest of the address tables.
You can do it from the Aspire dashboard by clicking the `backend-api` resource and running the "Import RÚIAN addresses" command.

### Where the data live
| Data | Place |
|---|---|
| Accounts, agencies, agents, adverts, RÚIAN register | PostgreSQL in the `realty-portal-data` Docker volume |
| Advert photos | `Server/data/photos/<advert id>/`, or `PhotoStorage:RootPath` |
| Downloaded ČÚZK address zips | `Server/Features/Ruian/Data/Addresses/`, or `Ruian:AddressDataPath` |
| Cached API responses | Redis container, rebuilt on start |
| Sign-in tokens in the browser | `localStorage` of the browser |

## Working with the portal
The user interface of the advert pages is Czech; the account and agency pages are English.

### Roles
| Role | How you get it | What it allows |
|---|---|---|
| Visitor | Nothing, just open the portal | Browse adverts, agencies and agents |
| User | Register an account | Sign in, edit the own profile |
| Agent (makléř) | "Become an agent" on the profile page | Publish and manage own adverts, found an agency |
| Agency admin | Found an agency, or be promoted by its admin | Manage the agency, its agents and all its adverts |
| Portal administrator (SuperAdmin) | Assigned in the database | Reserved; the administration section is a placeholder for now |

### API
Backend api is documented in OpenAPI format.
The UI is presented on `https://localhost:5094/scalar` when the server is running.
Keep in mind some endpoints require authentication and authorization,
so you need to sign in first and use the token in the `Authorization` header like so:
`Authorization: Bearer <token>`

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

The developer guide in [docs/Developer.md](docs/Developer.md) describes the design, the layout of the
projects and how to extend them.
