# Realty portal

A realty portal that imports adverts in the [Sreality.cz](https://www.sreality.cz/) format.

This file is the user guide: how to run the portal and how to work with it as a visitor, an agent, an agency,
an integrating system or an administrator. Developers start with [docs/Developer.md](docs/Developer.md).

## Quick start

You need [Docker](https://www.docker.com/products/docker-desktop) (running), the
[.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) and
[Node.js](https://nodejs.org/en/download/). Then one command starts the whole portal:

```bash
git clone https://github.com/CZMrBread/realty-portal.git
cd realty-portal
dotnet run --project realty-portal.AppHost
```

That one command restores the NuGet packages, installs the client's Node.js packages (Bootstrap, Bootstrap
Icons, Leaflet) on the first build, builds the solution, starts the containers and both applications.

If the browser complains about the HTTPS certificate, trust the .NET development certificate once per
machine and start again:

```bash
dotnet dev-certs https --trust
```

The solution is a .NET Aspire application. The AppHost starts PostgreSQL, pgAdmin and Redis as Docker
containers, then the Server and the Client, and opens the Aspire dashboard at https://localhost:17041.
The dashboard lists every resource with its state, logs and endpoints:

- `client` – the portal
- `backend-api` – the API and its OpenAPI UI at `/scalar`

On the first run the containers are pulled, the database is created, migrations are applied and the RÚIAN
region, district and municipality tables are filled from the embedded CSV files. The database lives in the
`realty-portal-data` Docker volume, so the data survive restarts.

### Import the RÚIAN addresses (required once)

The address point tables (streets, address points) are not filled automatically because the ČÚZK data are
downloaded from the internet and the import takes a while. Without them, adverts cannot be placed on an
address, so run the import once after the first start, in development as well as in production:

1. Open the Aspire dashboard.
2. Click the `backend-api` resource.
3. Run the **Import RÚIAN addresses** command (command name `import-ruian-addresses`).

The import downloads the ČÚZK zips into `Ruian:AddressDataPath` (see [Configuration](#configuration)) and
fills the remaining address tables. Run the same command again whenever you want to refresh the data.

## Configuration

The server settings are in [Server/appsettings.json](Server/appsettings.json). The PostgreSQL and Redis
connection strings and the RÚIAN `ImportKey` are not there: the Aspire AppHost supplies them to the server
as environment variables when it starts it.

### Photo storage

```json
"PhotoStorage": {
    "RootPath": "data/photos",
    "MaxLongEdge": 1920,
    "JpegQuality": 85,
    "MaxSourceEdge": 12000,
    "MaxUploadBytes": 26214400
}
```

`RootPath` is taken from the Server content root when relative. Every uploaded image is re-encoded as JPEG,
scaled down so that its longer side is at most `MaxLongEdge` pixels, and stored under
`RootPath/<advert id>/`. An upload larger than `MaxUploadBytes`, or declaring a side longer than
`MaxSourceEdge`, is refused. Deleting an advert removes its folder.

### RÚIAN

```json
"Ruian": {
    "AddressDataPath": "Features/Ruian/Data/Addresses"
}
```

`AddressDataPath` is where the downloaded ČÚZK zips are kept. A relative path is taken from the Server
content root.

### JWT authentication

```json
"Jwt": {
    "Key": "key-for-jwt-authentication-should-be-strong-and-random",
    "Issuer": "backend-api",
    "Audience": "backend-api"
}
```

> [!WARNING]
> Replace the key with a strong, random string before deploying. Set the issuer and audience to the values
> appropriate for your deployment.

### Client

The client reads the address of the API from `Client/wwwroot/appsettings.json`:

```json
{
  "ServerAPI": "http://localhost:5094/api/"
}
```

Change it if the API runs on a different address or port.

## Where the data live

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

The backend API is documented in the OpenAPI format. The interactive UI is at `https://localhost:5094/scalar`
while the server is running. Some endpoints require authentication: sign in first and send the token in the
`Authorization` header:

```
Authorization: Bearer <token>
```

## Structure

The solution consists of these projects:

- [Server](Server/README.md) – backend (ASP.NET Core)
- [Client](Client/README.md) – frontend (Blazor WebAssembly)
- [Shared](Shared/README.md) – DTOs, enums and other code shared by Server and Client
- [AppHost](realty-portal.AppHost) – the Aspire AppHost that runs the containers and both applications
- [ServiceDefaults](realty-portal.ServiceDefaults) – telemetry, health checks and resilience shared by the projects Aspire runs

`Server` and `Client` are not coupled; they share data structures only through the `Shared` project.

The developer guide in [docs/Developer.md](docs/Developer.md) describes the design, the layout of the
projects and how to extend them.
