# TODO

## Implementations

### Server – services

| File | Methods |
|---|---|
| `Server/Features/SRealty/Photo/PhotoService.cs` | `GetAdvertPhotosAsync`, `FindPhotoByIdAsync`, `FindPhotoByAdvertIdAndIdAsync`, `AddPhotoAsync`, `UpdatePhotoAsync`, `ReorderPhotosAsync`, `DeletePhotoAsync`, `DeleteAdvertPhotosAsync` |
| `Server/Features/SRealty/Photo/FilePhotoStorage.cs` | `SaveAsync`, `OpenReadAsync`, `DeleteAsync`, root path from configuration |

`RealtyAgencyService`, `RealtyAgentService` and `AdvertService` are implemented.

### Server – endpoints

| File | Handlers |
|---|---|
| `Server/Features/RealtyAgency/CreateRealtyAgency/CreateRealtyAgency.cs` | `CreateRealtyAgencyAsync` |
| `Server/Features/RealtyAgency/GetRealtyAgency/GetRealtyAgency.cs` | `GetRealtyAgencyByIdAsync`, `GetRealtyAgencyByRegistrationNumberAsync`, `Respond` |
| `Server/Features/RealtyAgency/GetRealtyAgencies/GetRealtyAgencies.cs` | `GetRealtyAgenciesAsync` |
| `Server/Features/RealtyAgency/UpdateRealtyAgency/UpdateRealtyAgency.cs` | `UpdateRealtyAgencyAsync` |
| `Server/Features/RealtyAgency/DeleteRealtyAgency/DeleteRealtyAgency.cs` | `DeleteRealtyAgencyAsync` |
| `Server/Features/RealtyAgent/CreateRealtyAgent/CreateRealtyAgent.cs` | `CreateRealtyAgentAsync` |
| `Server/Features/RealtyAgent/GetRealtyAgent/GetRealtyAgent.cs` | `GetRealtyAgentByIdAsync`, `GetRealtyAgentByRkIdAsync`, `Respond` |
| `Server/Features/RealtyAgent/UpdateRealtyAgent/UpdateRealtyAgent.cs` | `UpdateRealtyAgentByIdAsync`, `UpdateRealtyAgentByRkIdAsync`, `UpdateResolvedAsync` |
| `Server/Features/RealtyAgent/DeleteRealtyAgent/DeleteRealtyAgent.cs` | `DeleteRealtyAgentByIdAsync`, `DeleteRealtyAgentByRkIdAsync`, `DeleteResolvedAsync` |
| `Server/Features/SRealty/Photo/UploadPhoto/UploadPhoto.cs` | `UploadPhotoAsync` |
| `Server/Features/SRealty/Photo/EditPhoto/EditPhoto.cs` | handler, line 44 |
| `Server/Features/SRealty/Photo/DeletePhoto/DeletePhoto.cs` | handler, line 40 |

Missing files:

- `Server/Features/SRealty/Photo/GetPhoto/GetPhoto.cs` – `GET …/photo/{photoId}` streaming the image
- DTO ↔ entity mappers for `RealtyAgencyEntity` and `RealtyAgentEntity` (advert has `SrealityAdvertMapper.cs`)

### Client

| File | Work |
|---|---|
| `Client/Features/RealtyAgency/RealtyAgencyApiClient.cs` | `GetAgencyAsync`, `GetAgencyByRegistrationNumberAsync`, `GetAgenciesAsync`, `CreateAgencyAsync`, `UpdateAgencyAsync`, `DeleteAgencyAsync` |
| `Client/Features/RealtyAgent/RealtyAgentApiClient.cs` | `GetAgentAsync`, `GetAgentByRkIdAsync`, `CreateAgentAsync`, `UpdateAgentAsync`, `DeleteAgentAsync` |
| `Client/Features/SRealty/AdvertApiClient.cs` | `GetAdvertAsync`, `CreateAdvertAsync`, `UpdateAdvertAsync`, `DeleteAdvertAsync`, photo upload/edit/delete (only `GetFilteredAdvertsAsync` exists) |
| `Client/Features/RealtyAgency/GetRealtyAgencies/RealtyAgencyListPage.razor` | `LoadAsync`, list markup |
| `Client/Features/RealtyAgency/GetRealtyAgency/RealtyAgencyDetailPage.razor` | `LoadAsync`, `DeleteAsync`, detail markup |
| `Client/Features/RealtyAgency/CreateRealtyAgency/CreateRealtyAgencyPage.razor` | `SubmitAsync`, form |
| `Client/Features/RealtyAgency/UpdateRealtyAgency/UpdateRealtyAgencyPage.razor` | `LoadAsync`, `SubmitAsync`, form |
| `Client/Features/RealtyAgent/CreateRealtyAgent/CreateRealtyAgentPage.razor` | `SubmitAsync`, form |
| `Client/Features/RealtyAgent/GetRealtyAgent/RealtyAgentDetailPage.razor` | `LoadAsync`, `DeleteAsync`, detail markup |
| `Client/Features/RealtyAgent/UpdateRealtyAgent/UpdateRealtyAgentPage.razor` | `LoadAsync`, `SubmitAsync`, form |
| `Client/Features/SRealty/ListAdverts/AdvertListPage.razor` | filter, list, paging via `AdvertApiClient.GetFilteredAdvertsAsync` (page still holds a placeholder comment) |
| `Client/Features/SRealty/GetAdvert/AdvertDetailPage.razor` | load, detail, gallery |
| `Client/Features/SRealty/CreateAdvert/CreateAdvertPage.razor` | submit via API |
| `Client/Features/SRealty/UpdateAdvert/UpdateAdvertPage.razor` | load, submit via API |

`SRealtyFeature.cs` exists and is registered in `Client/Program.cs`.

### RÚIAN

- `Server/Features/Ruian/Data/*.csv` is filled from the ČÚZK code lists (`https://services.cuzk.cz/sestavy/cis/UI_VUSC.zip`, `UI_OKRES.zip`, `UI_OBEC.zip`; Windows-1250, `;`-separated, rows with `PLATI_DO` skipped). Decide whether to script the refresh instead of converting by hand.
- Geocoding (address / GPS → RÚIAN code) so agencies do not have to fill in `LocalityRuian` themselves.

## Refactorings

Ordered by how much future code they affect.

### 1. Caller resolution duplicated

`CreateAdvert`, `GetAdvert`, `UpdateAdvert`, `DeleteAdvert`, `BecomeAgent` repeat:

```csharp
var user = await userService.GetCurrentUserAsync(principal, ct);
if (user is null) return UserErrors.InvalidCredentials.ToResult();
var agent = await realtyAgentService.FindAgentByUserIdAsync(user.Id, ct);
if (agent is null) return AgentErrors.NotAnAgent.ToResult();
if (agent.RealtyAgencyId is null) return AgentErrors.NoAgency.ToResult();
```

The agency and agent endpoints above need the same. The user lookup is a wasted query: `user.Id` equals the `NameIdentifier` claim and the agent shares that primary key.

Proposal: `RealtyAgentService.FindCallingAgentAsync(ClaimsPrincipal, CancellationToken)` – read the id from the claim, call `FindAgentByIdAsync`. Open question: duplicate the claim parse, or extract `UserService.GetCurrentUserId(ClaimsPrincipal)` and call it.

### 2. `CancellationToken` inconsistency

- `RealtyAgentService.FindAgentByIdAsync`, `FindAgentByUserIdAsync`, `FindAgentByRkIdAsync`, `FindAgentWithAgencyAsync`, `GetAgencyAgentsAsync` – token required, other services default it
- `PhotoService` – read methods, `ReorderPhotosAsync`, `Delete*` have no token

Unify on `CancellationToken cancellationToken = default`.

### 3. Route naming

`/realty-agency` vs `/realtyagent`, `/srealty`. Unify (`/realty-agent`), fix `"realtyagent/become"` in `Client/Features/RealtyAgent/RealtyAgentApiClient.cs` and `"srealty/advert"` in `AdvertApiClient.cs`. Consider route constants in `Shared` so server and client read the same string.

### 4. `WithName` inconsistency

User feature and `BecomeAgent`: `WithName(nameof(LoginUserAsync))`. Others: `WithName("CreateAdvert")`. Unify on literal without `Async`.

### 5. Repeated response handling in API clients

Every method repeats `IsSuccessStatusCode ? (ReadFromJsonAsync, null) : (null, ReadMessageAsync)`. `Client/Infrastructure/ApiErrorReader.cs` exists with `ReadMessageAsync`; add `ReadAsync<T>(HttpResponseMessage)` next to it and route the clients through it.

### 6. Small

- `Server/Features/SRealty/Advert/DeleteAdvert/DeleteAdvert.cs:21` – remove obsolete `.WithOpenApi()`
- `RealtyAgentEntity.SRealtyProperties` → `Adverts` (also `SrealityAdvertConfiguration.cs:54`)
- `SrealityAdvertPhoto` → `SrealityAdvertPhotoEntity`
- `Client/_Imports.razor` – add `Client.Features.RealtyAgency`, `Client.Features.RealtyAgent`

### Leave as is

- `UploadPhoto` – four routes, one handler with nullable parameters
- Role seeding in `Server/Program.cs`
