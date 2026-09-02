# Server
Server is an ASP.NET Core minimal API that the Client and the real estate agencies talk to.
It stores the data in PostgreSQL through Entity Framework Core, caches some responses in Redis
and issues JWT tokens for authentication.

## Configuration
The configuration is located in `Server/appsettings.json`; the sections are described in the [root README](../README.md).
The connection strings for PostgreSQL (`sqldata`) and Redis (`cache`) are injected by the Aspire AppHost,
so they are not in the file.

When the server runs in the `Development` environment, the OpenAPI document is served at `/openapi/v1.json`
and an interactive reference at `/scalar`.

## Structure
The Server project follows vertical slice architecture, with each feature organized into its own folder.
The vertical slices are not strictly separated, but they are organized in a way that makes it easy to find and modify code related to a specific feature.
The main folders in the Server project are:
- [Features](Features) - Contains the vertical slices for each feature of the application.
    - [User](Features/User) - Accounts, login, refresh tokens and roles, built on ASP.NET Core Identity.
    - [RealtyAgent](Features/RealtyAgent) - Agent profiles, their roles within an agency and the agent policies.
    - [RealtyAgency](Features/RealtyAgency) - Agencies and their agents.
    - [SRealty](Features/SRealty) - Adverts in the Sreality format and their photos.
    - [Ruian](Features/Ruian) - The RÚIAN address register: regions, districts, municipalities and the address points import.
- [Infrastructure](Infrastructure) - Contains the technical code that no feature owns.
    - [Authentication](Infrastructure/Authentication) - Access token creation and the JWT signing key.
    - [Database](Infrastructure/Database) - `AppDbContext`, the entity configurations and the timestamp interface.
    - [Http](Infrastructure/Http) - Turns an `ApiError` into an HTTP result.
- [Migrations](Migrations) - Entity Framework Core migrations, applied automatically on start-up.

### Inside a feature
Every feature has the same shape:
- `<Feature>Endpoints.cs` - The route group of the feature with its prefix and OpenAPI tag; every route hangs under `/api`.
- `<Feature>Service.cs` - Reads and writes the feature's entities; other features call it instead of the `DbContext`.
- `Entity/` - The database entity and its Mapperly mapper between the entity and the contracts from `Shared`.
    - Mapperly generates the mapping code at compile time, so there is no runtime reflection overhead.
- One folder per endpoint, such as `CreateAdvert/` or `GetRealtyAgencies/` - A static class with a `Map...` method that registers the route and the handler next to it.

Endpoints follow REST principles and are distinguished by HTTP method, not by action words in the path.
Handlers resolve the caller through the services (`UserService.GetCurrentUserAsync`, `RealtyAgentService.FindCallingAgentAsync`)
and answer errors with the `ApiError` values defined in `Shared`, so the Client gets the same error codes for every feature.

## Database
Entity identifiers are version 7 GUIDs created by the entities themselves; `AppDbContext` refuses to save any other kind.
Enums from `Shared` are stored directly.
PostgreSQL extensions `pg_trgm` and `unaccent` back the name search of agencies and agents and the full-text search of adverts.

To add a migration after changing an entity, run from the repository root:
```bash
dotnet ef migrations add <Name> --project Server
```
