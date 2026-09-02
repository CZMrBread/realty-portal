# Developer guide

This guide is for a developer who has not seen the project before and wants to change it. It explains how
the portal is put together, where things are, which algorithms it relies on, and how to add to it. How to
run it is in the [README](../README.md).

## 1. What the portal is
A real estate portal. Agencies and their agents publish adverts, either through the web client or by pushing
JSON in the format of Sreality.cz over the API. Visitors search the adverts. Every address is tied to the
Czech address register RÚIAN, which the portal keeps in its own database.

## 2. Solution layout
| Project | Kind | Role |
|---|---|---|
| `Server` | ASP.NET Core minimal API | Endpoints, business rules, database access, photo storage, RÚIAN import |
| `Client` | Blazor WebAssembly | The user interface, running in the browser and calling the API over HTTP |
| `Shared` | Class library | Contracts both sides agree on: request and response records, enums, error codes, validation attributes |
| `realty-portal.AppHost` | .NET Aspire AppHost | Starts PostgreSQL, Redis and pgAdmin in Docker, then the Server and the Client, and injects connection strings and secrets |
| `realty-portal.ServiceDefaults` | Class library | OpenTelemetry, health checks and resilience handlers every hosted project calls in its `Program.cs` |
| `Tests/Server.Tests` | xUnit | Integration tests running the real API pipeline on in-memory SQLite |

`Shared` has no dependency on ASP.NET Core or Entity Framework Core, so it can be referenced by the browser
client. The Server and the Client never reference each other.

## 3. Architecture

```
browser ── Blazor WASM (Client) ── HTTPS/JSON ── Server API ── EF Core ── PostgreSQL
                                                    │
                                                    ├── Redis (output cache of whole responses)
                                                    └── file system (photos, ČÚZK zips)
```

### Vertical slices
Both applications are organised by feature, not by technical layer.
A feature folder holds everything of one domain area inside it, one folder per endpoint holds one operation.
This is a logical organisation, meant to answer "where do I look for X", not a rule of isolation: features call each other's services where they need to.

Server feature folder:
```
Features/<Feature>/
    <Feature>Endpoints.cs        route group: prefix, OpenAPI tag, registration of every endpoint
    <Feature>Service.cs          reads and writes the feature's entities; the only place that touches DbContext for them
    Entity/<X>Entity.cs          EF Core entity
    Entity/<X>Mapper.cs          Mapperly mapper between the entity and the Shared contracts
    <Endpoint>/<Endpoint>.cs     one static class: a Map<Endpoint>() extension and the handler next to it
```

Client feature folder:
```
Features/<Feature>/
    <Feature>ApiClient.cs        typed HttpClient wrapper for the feature's routes
    <Feature>Feature.cs          Add<Feature>Feature() registering the API client in DI
    <Endpoint>/<Endpoint>Page.razor
    Components/                  components reused within the feature or by other features
```

Shared feature folder:
```
<Feature>/
    <Endpoint>/<Endpoint>Request.cs, <Endpoint>Response.cs
    <Feature>Errors.cs           the ApiError values the feature answers with
    Enums/                       enums with LocalizedDisplayName attributes
```

The features are `User`, `RealtyAgent`, `RealtyAgency`, `SRealty` (adverts and photos) and `Ruian`.
Technical code no feature owns is in `Server/Infrastructure` (authentication, database context and entity configurations,
the `ApiError` to HTTP result conversion) and `Client/Infrastructure` (Bootstrap form components, error reading).

### Request flow on the server
1. `Program.cs` maps every feature's route group under `api`.
   Routes follows REST principles: the HTTP method expresses the action, the path expresses the resource.
   Every route has a prefix and an OpenAPI tag per feature.
2. Minimal API validation (`AddValidation`) checks the request record against its data annotation attributes
   and its `IValidatableObject.Validate` before the handler runs. A failure is a 400 problem document with
   the fields under `errors`.
3. The handler resolves the caller through `UserService.GetCurrentUserAsync` or
   `RealtyAgentService.FindCallingAgentAsync`, checks ownership, and calls the feature service.
4. Business refusals are `ApiError` values from `Shared`, turned into problem documents by
   `ApiErrorResults.ToResult()`; the stable code travels in the `errorCode` extension so the client can map
   it to wording.
5. Entities are mapped to response records with Mapperly, called as extension methods (`entity.ToDto()`).

### Authentication and authorisation
- ASP.NET Core Identity stores accounts (`ApplicationUser`, GUID keys). Passwords need six characters, a
  digit, lower and upper case.
- `AccessTokenService` issues a JWT access token (15 minutes) and a refresh token (30 days, 
  stored in `RefreshTokens`, single use).
  The access token carries the user id, name, roles and, for agents, the claims from
  `AgentClaimTypes`: agency id, the agency's key of the agent, and the agent role.
- Policies: `AgentPolicies.AgentOnly` and `AgencyAdminOnly` (from the agent role claim),
  `UserPolicies.SuperAdminOnly` (from the Identity role `SuperAdmin`).
  Because membership travels in the token, 
  the client forces a refresh after becoming an agent or founding an agency.
- On the client, `TokenStore` keeps both tokens in `localStorage`,
  `BearerTokenHandler` attaches the access token to every request and refreshes it when it is about to expire, 
  and `AuthStateService` exposes the parsed claims to components.
  `RequireRole` and `RequireAgentRole` hide UI the caller may not use, the server is the authority.

### Data model
```
ApplicationUser 1──0..1 RealtyAgentEntity *──0..1 RealtyAgencyEntity
                              │ SellerId                    │ RealtyAgencyId
                              └──────── SrealityAdvertEntity ┘
                                              │ 1──* SrealityAdvertPhotoEntity
                                              │ 0..1 RuianMunicipality / RuianDistrict / RuianAddressPoint
RuianRegion 1──* RuianDistrict 1──* RuianMunicipality 1──* RuianMunicipalityPart, RuianStreet
RuianAddressPoint *──1 RuianMunicipality, RuianMunicipalityPart, 0..1 RuianStreet
```
- An agent shares its primary key with its user account. An advert belongs to its selling agent; the agency is
  denormalised onto it so agency-wide listings and the agency-scoped keys (`AdvertRkId`, `SellerRkId`,
  `RealtyAgentRkId`) work. Deleting an agency detaches its agents and adverts.
- `SrealityAdvertEntity` mirrors `SrealityAdvertDto` field for field, both split into partial files by topic
  (core, price, areas, building, ...). The DTO's JSON names are the Sreality names.
- Entity identifiers are version 7 GUIDs created by the entities; `AppDbContext` stamps `CreatedAt` and
  `UpdatedAt` and refuses any other GUID kind on insert. Enums are stored as their integer values.
- Entity configurations live in `Server/Infrastructure/Database/Configuration`, one class per entity.
  PostgreSQL-only parts (`pg_trgm`, `unaccent`, the tsvector column) are applied only when the provider is
  Npgsql, so the same model runs on SQLite in the tests.

### Caching
Only whole HTTP responses are cached, through ASP.NET Core output caching backed by Redis.
`AdvertOutputCachePolicy` caches `GET /api/srealty/advert/{id}` for five minutes under a tag per advert;
`AdvertService` evicts the tag on update and delete. Services do not cache values.

## 4. Algorithms worth knowing

### Name search of agencies and agents
`SearchStringExtensions.ToSearchKey` lower-cases a name, strips accents, replaces punctuation with spaces
and drops trailing legal forms ("s.r.o.", "a.s.", "GmbH", ...). The result is stored in `SearchName` with a GIN trigram index.
A search compares the key of the query with `EF.Functions.TrigramsAreWordSimilar` and
orders by `TrigramsWordSimilarity`, so a fragment or a misspelling still finds the name.

### Full-text search of adverts
PostgreSQL generates a `tsvector` over the description and the address fields with the `unaccent` text
search configuration (`SrealityAdvertConfiguration.ConfigureSearchVector`).
The `Search` filter turns the query into a `plainto_tsquery` in the same configuration,
so every word must appear and accents do not matter.

### Locality resolution (`AdvertService.ResolveLocalityAsync`)
Runs on create and update, before saving, and returns validation errors keyed by JSON field name:
1. A RÚIAN code is authoritative. It is looked up in the table its level names (district, municipality, street, address point).
   Unknown code: 400 on `locality_ruian`. Level "Building" (stavební objekt) is not imported and is refused on `locality_ruian_level`.
2. The town the sender wrote must match the resolved municipality after `RuianNames.Normalize`, otherwise 400 on `locality_city`.
   This catches a code copied from another advert.
3. The register overwrites the names.
   An address point fills city, part, street, čp, čo and the coordinates when the sender gave none. Sender coordinates are kept.
4. With a coarser code, or no code, the names are used to go deeper: municipality by name, then čp with street or part to an address point,
   or a street name to a street.
   The deepest hit sets the code and level. 
   An unknown town is accepted unplaced; it then misses the region and district filters.

### RÚIAN import
- On start-up `RuianImporter` adds the regions, districts and municipalities missing from the database,
  read from CSV files embedded in the Server assembly (`Features/Ruian/Data`).
  Nothing is deleted, because adverts reference these rows.
- On demand `RuianAddressImporter` loads the ČÚZK monthly zip (`RuianAddressDownloader` fetches
  `https://vdp.cuzk.gov.cz/vymenny_format/csv/<yyyyMMdd>_OB_ADR_csv.zip` when none is on disk). Each CSV row
  is written with binary COPY into temporary staging tables (`RuianTableSync`), then one `INSERT ... ON
  CONFLICT DO UPDATE` per table applies the difference and rows absent from the zip are deleted, all in one transaction.
  Re-running on the same zip changes nothing.
- Coordinates come as S-JTSK (EPSG 5514) and are converted to WGS84 with ProjNet using the seven-parameter
  datum shift ČÚZK publishes, the CSV lists Y and X as positive numbers, EPSG 5514 expects both negated.

### Photos
`FilePhotoStorage` (behind `IPhotoStorage`, so another store can be plugged in) buffers the upload up to the
configured size, checks the declared dimensions before decoding to defuse decompression bombs, decodes with
ImageMagick (JPEG, PNG, WebP, HEIC), scales the longer side down to `MaxLongEdge`,
and writes a JPEG under `<RootPath>/<advert id>/`. 
The database keeps the relative path, order, kind, room type and alt text.

### Advert list filtering
`AdvertService.ApplyFilter` composes the `AdvertFilter` criteria into one query: expired adverts are always
left out, area means the estate area for land and the usable area otherwise, region and district filters
go through the resolved RÚIAN codes, agency and seller filters serve the agency and agent pages. 
Sorting is in `ApplySort`, newest first among equals.

## 5. The client
- `Program.cs` reads `ServerAPI` from `wwwroot/appsettings.json` and calls one `Add<Feature>Feature` per feature.
  Every API client gets the same `HttpClient` with the bearer handler.
- Pages are thin: they call the API client, hold the response, and render.
  Result tuples `(Response, Error)` carry either the payload or the message `ApiErrorReader` extracted from the problem document.
- Forms use the components in `Client/Infrastructure/Components/Forms` (`BootstrapEditForm`, `FloatingText`,
  `FloatingNumber`, `FloatingSelect`, `EnumSelect`, `EnumCheckBoxGroup`, `CheckBoxRevealField`) which wrap
  the Blazor input components with Bootstrap floating labels and validation feedback.
- The advert form (`Features/SRealty/Advert/Components/AdvertForm.razor`) is one card per DTO partial,
  `AdvertFieldVisibility` decides which fields apply to the chosen category.
  Display components (`AdvertParameters`, `AdvertAmenities`, `AdvertUtilities`) render the same partials on the detail page.
- `RuianAddressPicker` (`Features/Ruian/Components`) is the cascading address selector,
  it reports a `RuianAddressSelection` and the location section copies it into the DTO.
- Maps use Leaflet through `AdvertMap.razor` and its JavaScript module.
- User-facing text on the advert pages is Czech, enum labels come from `LocalizedDisplayName` through `GetLocalizedDisplayName()` so the culture is one switch.

## 6. How to extend

### Add an endpoint to an existing feature
1. Add the request and response records under `Shared/<Feature>/<Endpoint>/`, with data annotation attributes for validation.
   Add new refusal codes to `<Feature>Errors`.
2. Add `Server/Features/<Feature>/<Endpoint>/<Endpoint>.cs` with a `Map<Endpoint>(this IEndpointRouteBuilder)`
   extension and the handler.
   Pick the HTTP method by meaning; do not put verbs in the path. 
   Require the right policy.
3. Put any query or write into the feature's service, return whole entities from it.
4. Register the map method in `<Feature>Endpoints.cs`.
5. Add a method to the client's `<Feature>ApiClient`, then a page or component that uses it.

### Add a field to the advert
1. Add the property to the matching `SrealityAdvertDto.<Topic>.cs` with `JsonPropertyName` in the Sreality
   spelling and the validation attributes, add it to `SrealityAdvert.<Topic>.cs` on the entity.
2. Mapperly maps it by name.
   A portal-only field is ignored on the DTO-to-entity mappings with `MapperIgnoreTarget`,
   a derived output field is ignored the other way with `MapperIgnoreSource`.
3. Add a migration (below).
   Add the field to the form section and, if shown, to the display component.

### Add a migration
Entity changes need a migration. From the repository root, with the Server not running:
```bash
dotnet ef migrations add <Name> --project Server
```
Migrations apply automatically on start-up outside the Testing environment.

### Add a feature
Create the three feature folders (Server, Client, Shared), an `<Feature>Endpoints` class with its own prefix
and tag, an `<Feature>Service`, an `<Feature>ApiClient` and `<Feature>Feature`, and register both in the two
`Program.cs` files. Model the layout on `RealtyAgency`, the smallest complete feature.

## 7. Tests
`Tests/Server.Tests` boots the real API through `WebApplicationFactory<Program>` on an in-memory SQLite
database per test class. PostgreSQL-only features (trigram and full-text search) are left out of the model
there and are not covered. The RÚIAN register is empty under SQLite, so tests seed the rows they need
through `AppDbContext`. The suite currently covers advert creation: access rules, the agency-key route,
attribute and cross-field validation, and locality resolution.
```bash
dotnet test
```
