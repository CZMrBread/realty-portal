# Realty Portal Build Plan

Written 2026-08-28 against the working tree as of migration `20260827211830_Init`.
Twelve ordered phases, each written as the exact edits to make: which file, which
member, what it becomes, and what proves the phase is done. Seven design decisions
are already taken and folded in. Companion to `docs/TODO.md`, which lists the
stubs; this document says in which order and how to fill them.

Baseline: `dotnet test Tests/Server.Tests` passes 24/24. 54 `NotImplementedException`
stubs. 21 untracked files, including the only migration.

## Where the project stands

**Working:** registration, login, refresh-token rotation, logout;
`POST /realtyagent/become`; the advert slice (create, get, update, delete by id and
by agency key, filtered paged list) with Mapperly mapping and output caching on
`GET /srealty/advert/{advertId}`; the RÚIAN register seeded from embedded CSVs; the
`RealtyAgencyService` / `RealtyAgentService` method surfaces; client auth state,
bearer handler, role gates, layouts, Bootstrap form components and the sectioned
`AdvertForm`.

**Stubbed:** ten agency/agent handlers, eight `PhotoService` and four
`FilePhotoStorage` methods, eleven client API methods, seven agency/agent pages and
the four SRealty page bodies — exactly what `docs/TODO.md` lists.

**Not in the TODO:** full-text search is switched off in the model and would fail
at query time, agency list/search cannot execute, paging metadata is wrong in every
caller, the client can never read an advert id, and the test host cannot run any
cached route. Phases 1 and 2 clear these before any handler is written.

## Order of work

| # | Phase | Why here |
|---|---|---|
| 0 | Repository hygiene | no design input |
| 1 | Schema consolidation, regenerate Init | migration still uncommitted |
| 2 | Foundation bug fixes | tiny diffs, few callers |
| 3 | Decisions | 7 taken, 4 open |
| 4 | Wide shape refactorings | 15 files now vs 60 later |
| 5 | Shared contracts | errors, DTOs, mappers |
| 6 | Server: RealtyAgency slice | agents need an agency |
| 7 | Server: RealtyAgent slice | depends on 5 and 6 |
| 8 | Server: Photo slice | largest untested area |
| 9 | Client API clients | response shapes final |
| 10 | Client pages | last consumer |
| 11 | Cleanup sweep and deferred items | no shape changes |

## Decisions

### Taken (2026-08-28)

1. **Who may write agencies and agents:** an AgencyAdmin of that agency, checked
   from the DB row (`agent.RealtyAgencyId == agency.Id && agent.AgentRole == AgencyAdmin`).
   Routes keep `AgentOnly` as the cheap gate; the token-based `AgencyAdminOnly`
   policy stays unused.
2. **Creating an agency:** the founder joins automatically as AgencyAdmin and must
   not already belong to an agency (409). Response is the bare `RealtyAgencyDto`
   with 201; the client then forces a token refresh.
3. **Deleting an agency:** detach in the handler, nothing is destroyed. An advert
   belongs to the agent who sells it; the agency is a grouping. Every agent leaves
   (agency link and agency key cleared, as `LeaveAgencyAsync` does), every advert is
   detached the same way, then the agency row goes. One transaction.
4. **Photos on the client:** a separate anonymous list route
   `GET /srealty/advert/{advertId}/photo` returning `PhotoDto[]`, plus the anonymous
   image route `GET .../photo/{photoId}`. `SrealityAdvertDto` stays the untouched
   Sreality import contract.
5. **Routes:** rename the prefix `/realtyagent` → `/realty-agent`; `POST /become`
   stays. Route strings move to Shared-owned per-feature constants.
6. **"Token names no account":** new `UserErrors.Unauthenticated`
   (401, `user.unauthenticated`) answered through `ToResult()` wherever a user is
   resolved; a missing agent row stays `AgentErrors.NotAnAgent` (403).
7. **Creating an agent:** the AgencyAdmin looks the account up by e-mail through a
   new `GET /user?email=` returning a filled `GetUserProfileResponse`; the page then
   posts the `UserId`.

### Still open — the phases below use the recommendation

| Question | Recommendation |
|---|---|
| Deleting an agent who still sells adverts | Decision 3 makes the agent the owner of their adverts, so removing the profile would orphan them (the Seller FK is `SetNull`). Refuse with 409 `AgentErrors.HasAdverts` while the agent is named on any advert; they delete or hand the adverts over first. Alternative: delete the adverts and their photos together with the profile. |
| Existing agent added by an AgencyAdmin | The stub doc says "already has a profile → 409", but `BecomeAgent` deliberately creates agency-less profiles and "joining one is a separate step". Recommended: `CreateRealtyAgent` adopts an existing agency-less agent (`JoinAgencyAsync` + role from the request); an agent already in another agency is 409 `AlreadyInAgency`; no profile at all is created. |
| Photo limits and wording | Placeholders used below: 10 MB, JPEG/PNG/WebP, 50 photos per advert; Czech error texts and the `AgentRoleEnum` labels ("Makléř", "Správce kanceláře") are drafts. Change the constants in `Shared/SRealty/Photo/PhotoLimits.cs` and the strings in `ApiErrorMessages` as you like. |
| Agent detail visibility | Keep `GET /realty-agent/{agentId}` behind `AgentOnly` like the rk route: the DTO exposes agency membership and the agency key, which are agency-internal. A public agent profile, if ever wanted, belongs to `GetUserProfile`. |

## Verified blockers the TODO does not list

Checked against the working tree on 2026-08-28. The fix for each is a numbered step
in phases 1, 2 or 4.

1. **Full-text search is switched off and would fail at query time.**
   `AppDbContext.cs:42,44` have `HasPostgresExtension("unaccent")` and
   `ConfigureSearchVector(...)` commented out, so the regenerated migration creates
   `SearchVector` as a plain nullable `tsvector` that nothing writes and no GIN index
   covers. `SrealityAdvertConfiguration.TextSearchConfiguration` is `"unaccent"`,
   which is a dictionary, not a text-search configuration, so
   `AdvertService.ApplyFilter`'s `PlainToTsQuery("unaccent", ...)` throws in Postgres
   as soon as anyone sends `?search=`. → phase 1, steps 1–2 and 7.
2. **Agency list and search throw at runtime.** `RealtyAgencyEntity.SearchName` is
   `[NotMapped]`, yet `GetAgenciesAsync` orders by it and `SearchAgenciesAsync`
   passes it to `EF.Functions.TrigramsAreWordSimilar`. EF Core cannot translate it.
   → phase 1, step 3.
3. **`ToPagedResultAsync` counts after Skip/Take.**
   `Shared/Shared/Extensions/PageResultExtensions.cs` lists and counts the same
   query; all five callers apply `Skip/Take` first, so `TotalCount == Items.Count`.
   It also enumerates synchronously twice (Shared has no EF reference).
   → phase 2, step 1.
4. **The client can never read an advert id.** `SrealityAdvertDto.AdvertId` is
   `[JsonIgnore(Condition = WhenReading)]`, which applies to the Blazor client too;
   `GetFilteredAdvertsTests` already reads `advert_id` from raw JSON. The
   server-side purpose is already served by `[MapperIgnoreSource]`. → phase 2, step 2.
5. **Tests cannot touch any cached or evicting route.** `TestWebApplicationFactory`
   swaps only the DbContext; the Redis `IOutputCacheStore` stays with no connection
   string. The 24 passing tests avoid `GetAdvertById`, `UpdateAdvert`,
   `DeleteAdvert`; the tracked `res.trx` shows the failure a new test produces.
   → phase 2, step 5.
6. **Photo model does not match the routes already mapped.** Six photo routes
   address a photo by `photoRkId` and both requests carry
   `photo_rkid`/`photo_kind`/`alt`/`main`, but `SrealityAdvertPhoto` has no
   `PhotoRkId`, `PhotoKind`, `Alt`, `ContentType`; `RoomType` is `int?`; no DbSet
   (table `SrealityAdvertPhoto`). `UpdatePhotoAsync` takes `UploadPhotoRequest` +
   required `Stream` while `EditPhoto` binds `EditPhotoRequest` + `IFormFile?`.
   `FindPhotoByAdvertIdAndIdAsync` is non-nullable. → phase 1 step 4, phase 4 step 2.
7. **Photo storage, cleanup and the public image route are unspecified.** No
   configuration key, no constructor on `FilePhotoStorage`, no limits;
   `DeleteAdvert` never removes files; `PhotoService` has no cache eviction; the
   photo group is `AgentOnly` so an anonymous `GetPhoto` cannot live there; no photo
   list or DTO. → phase 8.
8. **Keyed create route has no key; PUT can re-key silently.** `CreateAdvert.cs:23`
   maps `MapPost("/rk", ...)`; every sibling uses `/rk/{...RkId}`. `UpdateEntity`
   copies `AdvertRkId` from the body with no 409 check. → phase 2, step 3.
9. **Create advert is silently rejected on the client.** `SellerId` carries
   `[RequiredIfValue(nameof(SellerRkId), [null])]`, `AdvertForm` has no control for
   it and no `ValidationSummary`, so `OnValidSubmit` never fires. `AdvertFilter` is
   `init`-only and array-typed, so the list page cannot bind it.
   → phase 5 step 6, phase 10 steps 7–8.
10. **Working tree is not committable as it is.** 21 untracked files (RÚIAN
    feature, the migration, `GetFilteredAdverts` + test, `AdvertSortEnum`, client
    `SRealtyFeature`/`AdvertApiClient`, `docs/TODO.md`) while the staged copies of
    `AppDbContext`, `Program.cs`, `AdvertService`, `SRealtyEndpoints` are older
    snapshots. `Tests/Server.Tests/TestResults/res.trx` is tracked. All three
    migration files start with a BOM. → phase 0.
11. **Username sign-in unreachable; logout revokes other sessions on reuse.**
    `LoginUserRequest.Email` is `[EmailAddress]` and validation runs first, so
    `?? FindByNameAsync` never executes. `RefreshAsync` treats any revoked token as
    a replay and revokes every session, including one revoked by logout;
    `ReplacedByHash` is never read. → phase 11, step 2.
12. **Authorization half-wired.** `AgencyAdminOnly` and `SuperAdminOnly` applied
    nowhere; every agency/agent write is `AgentOnly`; no ownership helper on
    agency/agent entities; nothing creates a SuperAdmin although the client gates
    on it. → phase 6 step 1, phase 10 step 2.
13. **Five navigation links, zero pages; new pages have no layout or gate.**
    `NavMenu → admin`, `SuperAdminLayout → admin/agencies, admin/users`,
    `AgencyLayout → agency/agents, agency/settings` match no `@page`. The seven
    agency/agent pages declare no `@layout`, show Edit/Delete to everyone, and none
    injects `AuthStateService` for the forced refresh. → phase 10, steps 1–2.
14. **Error catalogue and Czech wording stop short of the new slices.** No
    `ApiError` for "agent key taken" or "agent belongs to another agency";
    `NoAgency` is worded for adverts; `ApiErrorMessages` lacks every `agency.*`,
    `agent.not_found`, `advert.not_owned`; the 401 fallback is "Wrong email or
    password." for every signed-out write. → phase 5, steps 1 and 7.

## Phases

Each phase changes signatures or strings the next one calls, so keep the order.
Phases 0–2 need no further input. Line numbers refer to the tree as of 2026-08-28.

### Phase 0 — Repository hygiene (small)

Goal: make the working tree committable so every later phase starts from a
coherent baseline.

1. Stage the 21 untracked files:

   ```sh
   git add docs/TODO.md Shared/SRealty/Advert/ListAdverts/AdvertSortEnum.cs \
     Client/Features/SRealty/AdvertApiClient.cs Client/Features/SRealty/SRealtyFeature.cs \
     Server/Features/Ruian Server/Infrastructure/Database/Configuration/Ruian*Configuration.cs \
     Server/Features/SRealty/Advert/GetFilteredAdverts/GetFilteredAdverts.cs \
     Tests/Server.Tests/Features/SRealty/Advert/GetFilteredAdverts/GetFilteredAdvertsTests.cs \
     Server/Migrations
   ```

   then `git add -A` so the staged copies of `AppDbContext.cs`, `Program.cs`,
   `AdvertService.cs`, `SRealtyEndpoints.cs` match the working tree (they are stale
   snapshots today).
2. `.gitignore` — append a line `TestResults/`. Then
   `git rm --cached Tests/Server.Tests/TestResults/res.trx`.
3. Strip the BOM from the three migration files (they start with `EF BB BF`); from
   Git Bash:

   ```sh
   for f in Server/Migrations/*.cs; do sed -i '1s/^\xEF\xBB\xBF//' "$f"; done
   ```

   Repeat this after every `dotnet ef migrations add` — the tool writes the BOM
   every time.
4. `dotnet test Tests/Server.Tests` → 24 passed. Commit.

Done when `git status` is clean, `git ls-files | grep -i testresults` prints
nothing, `head -c3 Server/Migrations/*.cs | xxd` shows no `efbbbf`.

### Phase 1 — Schema consolidation, then regenerate Init once (medium)

Goal: every schema change the later phases need, folded into the still-uncommitted
Init migration.

1. `Server/Infrastructure/Database/Configuration/SrealityAdvertConfiguration.cs:17`
   — `TextSearchConfiguration = "czech_unaccent"`. (`unaccent` is a dictionary; the
   configuration that uses it is created in step 7.)
2. `Server/Infrastructure/Database/AppDbContext.cs:42,44` — uncomment both lines so
   the Npgsql branch reads:

   ```csharp
   builder.HasPostgresExtension("unaccent");
   builder.HasPostgresExtension("pg_trgm");
   SrealityAdvertConfiguration.ConfigureSearchVector(builder.Entity<SrealityAdvertEntity>());
   ```

3. Persisted agency search name.
   - `Server/Features/RealtyAgency/Entity/RealtyAgencyEntity.cs:23-24` — replace
     the `[NotMapped]` getter with
     `[MaxLength(200)] public string SearchName { get; set; } = string.Empty;`.
     Drop the unused usings (`System.Text.RegularExpressions`,
     `Microsoft.AspNetCore.Components`, `System.ComponentModel.DataAnnotations.Schema`).
   - `Server/Features/RealtyAgency/RealtyAgencyService.cs` — first line of
     `CreateAgencyAsync` and `UpdateAgencyAsync`:
     `agency.SearchName = agency.Name.ToSearchKey();`
   - `Server/Infrastructure/Database/Configuration/RealtyAgencyConfiguration.cs` —
     add a static `ConfigureSearchName(EntityTypeBuilder<RealtyAgencyEntity> builder)`
     containing
     `builder.HasIndex(a => a.SearchName).HasMethod("gin").HasOperators("gin_trgm_ops");`
     and call it from the same Npgsql-only block in `AppDbContext` as step 2 (the
     trigram operator class exists only in Postgres).
4. Photo entity.
   - Rename `Server/Features/SRealty/Photo/Entity/SrealityAdvertPhoto.cs` →
     `SrealityAdvertPhotoEntity.cs`, class `SrealityAdvertPhotoEntity`; update
     `SrealityAdvertEntity.Photos`, `PhotoService`, and the configuration.
   - Members become:

     ```csharp
     public Guid Id { get; set; } = Guid.CreateVersion7();
     public Guid SrealityAdvertId { get; set; }
     public SrealityAdvertEntity Advert { get; set; } = null!;
     public required string StoragePath { get; set; }
     [MaxLength(100)] public required string ContentType { get; set; }
     public int Order { get; set; }
     [MaxLength(64)] public string? PhotoRkId { get; set; }
     public PhotoKindEnum PhotoKind { get; set; } = PhotoKindEnum.Photo;
     public PhotoRoomTypeEnum? RoomType { get; set; }
     [MaxLength(500)] public string? Alt { get; set; }
     public DateTimeOffset CreatedAt { get; set; }
     public DateTimeOffset UpdatedAt { get; set; }
     ```

   - `Server/Infrastructure/Database/Configuration/SrealtyAdvertPhotosConfiguration.cs`
     — rename the file to `SrealityAdvertPhotoConfiguration.cs` (the class already
     has that name); add
     `builder.HasIndex(p => new { p.SrealityAdvertId, p.PhotoRkId }).IsUnique().HasFilter("\"PhotoRkId\" IS NOT NULL");`
     next to the existing Order index.
   - `Server/Infrastructure/Database/AppDbContext.cs:27` — add
     `public DbSet<SrealityAdvertPhotoEntity> SrealityAdvertPhotos { get; set; }`;
     the table becomes `SrealityAdvertPhotos`.
5. `Server/Features/RealtyAgent/Entity/RealtyAgentEntity.cs:37` —
   `SRealtyProperties` → `Adverts`; `SrealityAdvertConfiguration.cs:54` —
   `.WithMany(s => s.Adverts)`.
6. Delete `Server/Migrations/20260827211830_Init.cs`, `...Init.Designer.cs`,
   `AppDbContextModelSnapshot.cs`; regenerate with the same command you used for
   the current one (`dotnet ef migrations add Init --project Server`). Confirm the
   new `Up` contains `Annotation("Npgsql:PostgresExtension:unaccent", ...)`, the
   `SearchVector` column with `computedColumnSql` and `stored: true`,
   `IX_SrealityAdverts_SearchVector` with `Npgsql:IndexMethod = gin`,
   `IX_RealtyAgencies_SearchName`, and table `SrealityAdvertPhotos` with
   `PhotoRkId`/`PhotoKind`/`Alt`/`ContentType`.
7. Edit the generated `Server/Migrations/<stamp>_Init.cs`: in `Up`, after the
   `AlterDatabase()` call and before `CreateTable(name: "SrealityAdverts", ...)`,
   insert

   ```csharp
   migrationBuilder.Sql("""
       CREATE TEXT SEARCH CONFIGURATION czech_unaccent (COPY = simple);
       ALTER TEXT SEARCH CONFIGURATION czech_unaccent
           ALTER MAPPING FOR asciiword, asciihword, hword_asciipart, word, hword, hword_part
           WITH unaccent, simple;
       """);
   ```

   and in `Down`, after `DropTable(name: "SrealityAdverts")`:
   `migrationBuilder.Sql("DROP TEXT SEARCH CONFIGURATION IF EXISTS czech_unaccent;");`.
   Postgres resolves the configuration name when the generated column is created,
   so the SQL has to run before that table.
8. Strip the BOM again (phase 0, step 3). Start the AppHost against a fresh
   Postgres: the server has to come up (this is what the old Init could not do),
   and `GET /api/srealty/advert?search=byt` has to answer 200. `dotnet test` still
   24/24 (SQLite ignores `SearchVector`). Commit.

Why here: done now it is one migration; done after the photo and agency handlers
exist it is four migrations and a second pass over the handlers.

### Phase 2 — Foundation bug fixes (small)

Goal: five fixes with two to five callers each; every later phase depends on them.

1. Paging.
   - Delete `Shared/Shared/Extensions/PageResultExtensions.cs`.
   - `RealtyAgencyService.cs` — delete `GetAgenciesAsync` (52-58); in
     `SearchAgenciesAsync` replace line 80 with

     ```csharp
     var totalCount = await query.CountAsync(cancellationToken);
     var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
     return new PagedResult<RealtyAgencyEntity>(items, page, pageSize, totalCount);
     ```

   - `RealtyAgentService.cs:62-68` — same shape in `GetAgencyAgentsPageAsync`:
     filter, `CountAsync`, then
     `OrderBy(RealtyAgentRkId).ThenBy(UserId).Skip/Take/ToListAsync`.
   - `AdvertService.cs:56-73` — delete `GetAgencyAdvertsAsync` and
     `GetSellerAdvertsAsync` (no callers). Remove `using Shared.Shared.Extensions;`
     from `AdvertService` and `RealtyAgentService`.
2. Advert id on the client.
   - `Shared/SRealty/Advert/SrealityAdvertDto.Core.cs:18` — delete the
     `[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]` line; doc:
     "Assigned by the portal; ignored on create and update."
     (`SrealityAdvertMapper` already `MapperIgnoreSource`s it.)
   - `Tests/.../GetFilteredAdvertsTests.cs` — `ReadAsync` returns
     `PagedResult<SrealityAdvertDto>`; `Id(SrealityAdvertDto a) => a.AdvertId!.Value`;
     `CreateAdvertAsync` reads `CreateAdvertResponse` and returns
     `created.Advert!.AdvertId!.Value`; delete the `JsonElement` plumbing and the
     doc comment at line 156.
3. Advert key.
   - `CreateAdvert.cs:23` — `group.MapPost("/rk/{advertRkId}", CreateAdvertAsync)`.
   - `SrealityAdvertMapper.cs` — on `UpdateEntity` add
     `[MapperIgnoreTarget(nameof(SrealityAdvertEntity.AdvertRkId))]` and
     `[MapperIgnoreSource(nameof(SrealityAdvertDto.AdvertRkId))]` (RMG020 is an
     error in `Server.csproj`, so the source side must be ignored too). The key is
     set once by the create route and never re-keyed by PUT.
4. Seller always known (needed by decision 3 so a detached advert keeps its owner).
   - `CreateAdvert.cs:87` — after `var advert = request.ToEntity(...)` add
     `advert.SellerId = agent.UserId;`
   - `UpdateAdvert.cs:119` — after `request.UpdateEntity(advert)` add
     `advert.SellerId = agent.UserId;` (the seller check two lines above already
     guarantees the caller is the seller).
5. Test host.
   - `Tests/Server.Tests/TestWebApplicationFactory.cs:59` — after
     `services.AddDbContext...` add

     ```csharp
     services.RemoveAll<IOutputCacheStore>();
     services.AddOutputCache();            // TryAdd puts the in-memory store back
     services.RemoveAll<IPhotoStorage>();
     services.AddSingleton<IPhotoStorage, InMemoryPhotoStorage>();
     ```

     (`using Microsoft.AspNetCore.OutputCaching;`
     `using Microsoft.Extensions.DependencyInjection.Extensions;`
     `using Server.Features.SRealty.Photo;`).
   - New `Tests/Server.Tests/TestDoubles/InMemoryPhotoStorage.cs` — `IPhotoStorage`
     over a `ConcurrentDictionary<string, byte[]>`; stub every method with
     `NotImplementedException` for now, phase 8 fills it.
   - New `Tests/Server.Tests/Features/SRealty/Advert/GetAdvert/GetAdvertTests.cs`:
     create an advert as an agent, anonymous `GET api/srealty/advert/{id}` → 200
     and `AdvertId` equal; `PUT` a changed description, `GET` again → the new
     description (proves the in-memory store and eviction). **Verify:** if the GET
     still fails with "No endpoints specified... ConnectionStrings:cache", the
     Redis store survived the `RemoveAll`; register the in-memory store explicitly
     instead.

Done when `dotnet test` passes with the new `GetAdvertTests`, and
`POST api/srealty/advert/rk/ABC` creates an advert with `advert_rkid = "ABC"`.

### Phase 3 — Decisions (7 taken)

Everything the handlers depend on is decided (see the top of this document). The
four open questions only affect `DeleteRealtyAgent`, `CreateRealtyAgent`'s adoption
path, photo limits and the agent GET; the phases below use the recommendations.

### Phase 4 — Wide shape refactorings, before any new body (medium)

Goal: signatures and strings the ~45 new bodies will call. About 15 files today,
about 60 after phases 6–10.

1. **Caller resolution (TODO §1).**
   - New `Server/Features/User/UserClaims.cs`:

     ```csharp
     public static class UserClaims
     {
         /// <summary>Account identifier carried in the token, or null when the token names none.</summary>
         public static Guid? GetUserId(this ClaimsPrincipal principal)
             => Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;
     }
     ```

   - `UserService.cs:19-29` — `GetCurrentUserAsync` body becomes
     `var userId = principal.GetUserId(); return userId is null ? null : await FindUserByIdAsync(userId.Value, cancellationToken);`
   - `RealtyAgentService.cs` — add

     ```csharp
     /// <summary>Agent profile of the caller, or null when the token names no account or the account is not an agent.</summary>
     public async Task<RealtyAgentEntity?> FindCallingAgentAsync(ClaimsPrincipal principal,
         CancellationToken cancellationToken = default)
     {
         var userId = principal.GetUserId();
         return userId is null ? null : await FindAgentByIdAsync(userId.Value, cancellationToken);
     }
     ```

     Delete `FindAgentByUserIdAsync` (an alias of `FindAgentByIdAsync`); its one
     other caller is `AccessTokenService.cs:149`.
   - New `Shared/User/UserErrors.cs` entry:
     `Unauthenticated = new("user.unauthenticated", HttpStatusCode.Unauthorized, "The token names no account.")`.
   - Replace the user+agent block in `GetAdvert.cs:52-62`,
     `UpdateAdvert.cs:38-48, 67-77`, `DeleteAdvert.cs:38-48, 66-76`,
     `CreateAdvert.cs:48-58` with

     ```csharp
     var agent = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
     if (agent is null)
     {
         return AgentErrors.NotAnAgent.ToResult();
     }
     ```

     and remove the `UserService userService` parameter plus
     `using Server.Features.User;` / `using Shared.User;` from those four files. The
     `agent.RealtyAgencyId is null → NoAgency` check stays where it is today (rk
     routes and keyed create only).
   - `BecomeAgent.cs:33` — `return UserErrors.Unauthenticated.ToResult();` (it must
     keep the user lookup: it creates the profile for that account).
     `GetCurrentUser.cs:24` — same replacement for `Results.Unauthorized()`; drop
     the unused `UserManager` parameter.
   - Remove `UserService` from the signatures of the ten agency/agent stubs; they
     will use `FindCallingAgentAsync` in phases 6–7.
2. **CancellationToken (TODO §2).**
   - `RealtyAgentService.cs` — `FindAgentByIdAsync`, `FindAgentByRkIdAsync`,
     `FindAgentWithAgencyAsync`, `GetAgencyAgentsAsync`:
     `CancellationToken cancellationToken = default`.
   - `PhotoService.cs` — every method gets the defaulted token; signatures become

     ```csharp
     Task<List<SrealityAdvertPhotoEntity>> GetAdvertPhotosAsync(Guid advertId, CancellationToken cancellationToken = default);
     Task<SrealityAdvertPhotoEntity?> FindPhotoByIdAsync(Guid photoId, CancellationToken cancellationToken = default);
     Task<SrealityAdvertPhotoEntity?> FindPhotoByAdvertIdAndIdAsync(Guid advertId, Guid photoId, CancellationToken cancellationToken = default);
     Task<SrealityAdvertPhotoEntity?> FindPhotoByRkIdAsync(Guid advertId, string photoRkId, CancellationToken cancellationToken = default);   // new
     Task<SrealityAdvertPhotoEntity> AddPhotoAsync(SrealityAdvertEntity advert, Stream content, string contentType, PhotoRequest request, CancellationToken cancellationToken = default);
     Task<SrealityAdvertPhotoEntity> UpdatePhotoAsync(SrealityAdvertPhotoEntity photo, Stream? content, string? contentType, PhotoRequest request, CancellationToken cancellationToken = default);
     Task<List<SrealityAdvertPhotoEntity>> ReorderPhotosAsync(Guid advertId, List<Guid> orderedPhotoIds, CancellationToken cancellationToken = default);
     Task DeletePhotoAsync(SrealityAdvertPhotoEntity photo, CancellationToken cancellationToken = default);
     Task DeleteAdvertPhotosAsync(Guid advertId, CancellationToken cancellationToken = default);
     ```

     (`PhotoRequest` is the merged request record from phase 5, step 3; until then
     keep `UploadPhotoRequest`.)
   - `IPhotoStorage.cs` / `FilePhotoStorage.cs` — `SaveAsync`, `OpenReadAsync`,
     `DeleteAsync` gain `CancellationToken cancellationToken = default`; delete
     `ValidateAsync` (phase 8 replaces it with a static sniffer, so storage stays
     storage).
   - `Server/Infrastructure/Authentication/AccessTokenService.cs` —
     `CreateAuthenticationAsync(user, ct = default)` returning `Task<TokenResponse>`
     (never null), `RefreshAsync(token, ct = default)`, `RevokeAsync(token, ct = default)`,
     `RevokeAllForUserAsync(userId, ct = default)`, private
     `CreateAccessTokenAsync(user, ct)`; forward the token to `SaveChangesAsync`,
     `FirstOrDefaultAsync`, `ExecuteUpdateAsync` and line 149
     (`FindAgentByIdAsync(user.Id, cancellationToken)`).
   - Callers: `Login.cs:40`, `Register.cs:47`, `Refresh.cs:23` pass
     `cancellationToken`; delete the dead `if (token == null)` branches at
     `Login.cs:41-44` and `Register.cs:48-51`. `Logout.cs:23` gains a
     `CancellationToken` parameter and passes it. `GetUserProfile.cs:20` takes
     `UserService` + token and calls `FindUserByIdAsync` instead of `UserManager`.
3. **Route constants and the rename (TODO §3).** Constants are path segments without
   slashes; ASP.NET Core joins group and route templates either way, and the client
   composes `$"{Prefix}/{Segment}"` relative to its `ServerAPI` base (which ends in
   `/api/`).
   - New `Shared/Shared/ApiRoutes.cs`: `Prefix = "api"`,
     `ErrorCodeExtension = "errorCode"`, `SortQuery = "sort"`, `PageQuery = "page"`,
     `PageSizeQuery = "pageSize"`. Use `ErrorCodeExtension` in
     `ApiErrorResults.cs:17` and `ApiErrorReader.cs:57`; the query keys in
     `AdvertApiClient.cs:59-61`.
   - New `Shared/User/UserRoutes.cs`: `Prefix = "user"`, `Login`, `Register`,
     `Refresh`, `Logout`, `Me = "me"`. New `Shared/RealtyAgency/RealtyAgencyRoutes.cs`:
     `Prefix = "realty-agency"`, `Registration = "registration"`. New
     `Shared/RealtyAgent/RealtyAgentRoutes.cs`: `Prefix = "realty-agent"`,
     `Become = "become"`, `Rk = "rk"`. New `Shared/SRealty/SRealtyRoutes.cs`:
     `Prefix = "srealty"`, `Advert = "advert"`, `Rk = "rk"`, `Photo = "photo"`,
     `AdvertIdRouteValue = "advertId"` (used by `AdvertOutputCachePolicy.cs:18` and
     the templates).
   - Server: `RealtyAgentEndpoints.cs:13` → `Prefix = RealtyAgentRoutes.Prefix`
     (this is the rename); the same swap in `UserEndpoints`, `RealtyAgencyEndpoints`,
     `SRealtyEndpoints`; `Program.cs:127` → `MapGroup(ApiRoutes.Prefix)`. Route
     literals that are plain segments (`"/become"`, `"/login"`, `"/me"`) become the
     constants; templates with parameters stay literal but use the segment
     constants, e.g. `$"/{RealtyAgentRoutes.Rk}/{{agentRkId}}"`.
   - Client: `RealtyAgentApiClient.cs:18` →
     `$"{RealtyAgentRoutes.Prefix}/{RealtyAgentRoutes.Become}"`;
     `UserApiClient.cs:19,28,37,52` and `AdvertApiClient.cs:17` likewise.
   - Tests keep literal URLs (they pin the public contract) but must follow the
     rename: `BecomeAgentTests.cs:16,28,45,47,62`, `GetFilteredAdvertsTests.cs:121`,
     `RequestValidationTests.cs:108` → `api/realty-agent/become`. The client tests'
     `srealty/advert` literals are unaffected.
4. **WithName (TODO §4).** `BecomeAgent.cs:17` → `"BecomeAgent"`; `Login.cs:16`
   `"Login"`, `Register.cs:15` `"Register"`, `GetUserProfile.cs:16`
   `"GetUserProfile"`, `GetCurrentUser.cs:14` `"GetCurrentUser"`, `Refresh.cs:15`
   `"Refresh"`, `Logout.cs:13` `"Logout"`. Delete `DeleteAdvert.cs:21`
   `.WithOpenApi()`.

Done when the solution builds with no reference to `FindAgentByUserIdAsync`,
`GetCurrentUserAsync` is called only from `BecomeAgent` and `GetCurrentUser`,
`grep -rn '"/realtyagent\|realtyagent/'` over Server/Client/Tests is empty, and all
tests pass.

### Phase 5 — Shared contracts (medium)

Goal: errors, DTOs, mappers and the client reader the handlers and pages compile
against — written once, before any body.

1. `Shared/RealtyAgent/AgentErrors.cs` — add (Czech client text in step 7):

   ```csharp
   RkIdTaken       = new("agent.rkid_taken",       HttpStatusCode.Conflict,  "This agency already has an agent under that key.");
   NotOwned        = new("agent.not_owned",        HttpStatusCode.Forbidden, "This agent belongs to another agency.");
   AlreadyInAgency = new("agent.already_in_agency", HttpStatusCode.Conflict,  "This agent already belongs to an agency.");
   NotAgencyAdmin  = new("agent.not_agency_admin", HttpStatusCode.Forbidden, "Only an agency administrator may do that.");
   HasAdverts      = new("agent.has_adverts",      HttpStatusCode.Conflict,  "The agent is still named on adverts.");
   ```

   and reword `NoAgency`'s detail to "This agent belongs to no agency."
2. New `Shared/SRealty/Photo/PhotoErrors.cs`: `NotFound` (`photo.not_found`, 404),
   `RkIdTaken` (`photo.rkid_taken`, 409), `TooLarge` (`photo.too_large`, 413),
   `UnsupportedFormat` (`photo.unsupported_format`, 415), `LimitReached`
   (`photo.limit_reached`, 409).
3. Photo DTOs.
   - New `Shared/SRealty/Photo/PhotoRequest.cs` = the body of today's
     `UploadPhotoRequest` with `PhotoRkid` renamed `PhotoRkId`; delete
     `UploadPhoto/UploadPhotoRequest.cs`, `EditPhoto/EditPhotoRequest.cs`,
     `UploadPhoto/UploadPhotoResponse.cs` and the empty `DeletePhoto` folder item
     in `Shared/Shared.csproj:10`.
   - New `Shared/SRealty/Photo/PhotoDto.cs`: `Guid Id`, `Guid AdvertId`,
     `string? PhotoRkId` (`photo_rkid`), `int Order`, `PhotoKindEnum PhotoKind`
     (`photo_kind`), `PhotoRoomTypeEnum? RoomType` (`room_type`), `string? Alt`,
     `string ContentType` (`content_type`). No URL: the client builds
     `{ServerAPI}srealty/advert/{AdvertId}/photo/{Id}`.
   - New `Shared/SRealty/Photo/PhotoLimits.cs`: `MaxBytes = 10 * 1024 * 1024`,
     `MaxPerAdvert = 50`, `AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"]`
     — placeholders to confirm.
4. `Shared/User/GetUserProfile/GetUserProfileResponse.cs` — `Guid Id`,
   `string UserName`. (Used by `GET /user/{id}` and the new `GET /user?email=`.)
5. `Shared/RealtyAgent/AgentRoleEnum.cs` —
   `[LocalizedDisplayName(DisplayNameCz = "Makléř", DisplayNameEn = "Agent")]` and
   `[LocalizedDisplayName(DisplayNameCz = "Správce kanceláře", DisplayNameEn = "Agency administrator")]`.
   `Shared/RealtyAgent/RealtyAgentDto.cs:11` —
   `[Required] public AgentRoleEnum? AgentRole { get; set; }` so `EnumSelect` can
   bind it.
6. `Shared/SRealty/Advert/ListAdverts/AdvertFilter.cs` — every `init` → `set`;
   `AdvertSubtypeEnum[]?` → `List<AdvertSubtypeEnum>?`, `BuildingConditionEnum[]?` →
   `List<BuildingConditionEnum>?`; `AdvertService.cs:124,173` `{ Length: > 0 }` →
   `{ Count: > 0 }`. **Verify:** run
   `GetFilteredAdverts_NarrowsByLayoutAreaPriceAndCondition`: if `[AsParameters]`
   does not bind `List<T>` from repeated query values, keep the arrays and convert
   in the list page instead.
7. Mappers, same attributes as `SrealityAdvertMapper`:
   - New `Server/Features/RealtyAgency/RealtyAgencyMapper.cs`: `ToEntity(dto)` and
     `UpdateEntity(dto, entity)` ignoring target `Id`, `CreatedAt`, `UpdatedAt`,
     `SearchName`, `Agents` and source `Id`; `ToDto(entity)` ignoring source
     `CreatedAt`, `UpdatedAt`, `SearchName`, `Agents`.
   - New `Server/Features/RealtyAgent/RealtyAgentMapper.cs`: `ToDto(entity)`
     ignoring source `User`, `RealtyAgency`, `Adverts`; `UpdateEntity(dto, entity)`
     mapping only `AgentRole` and `RealtyAgentRkId` (ignore target `UserId`,
     `RealtyAgencyId`, `User`, `RealtyAgency`, `Adverts`; ignore source `UserId`,
     `RealtyAgencyId`) — decision: no agency moves through PUT.
8. Client.
   - `Client/Infrastructure/ApiErrorReader.cs` — add

     ```csharp
     public static async Task<(T? Response, string? Error)> ReadAsync<T>(HttpResponseMessage response,
         CancellationToken cancellationToken = default)
         => response.IsSuccessStatusCode
             ? (await response.Content.ReadFromJsonAsync<T>(cancellationToken), null)
             : (default, await ReadMessageAsync(response));

     public static async Task<string?> ReadErrorAsync(HttpResponseMessage response)
         => response.IsSuccessStatusCode ? null : await ReadMessageAsync(response);
     ```

     Route `UserApiClient.LoginAsync/RegisterAsync`,
     `RealtyAgentApiClient.BecomeAgentAsync`, `AdvertApiClient.GetFilteredAdvertsAsync`
     through `ReadAsync<T>`. `StatusFallback`: 401 → "Přihlaste se prosím.",
     403 → "K této akci nemáte oprávnění.", default →
     "Server požadavek odmítl ({code}).".
   - `Client/Infrastructure/ApiErrorMessages.cs` — add entries for
     `UserErrors.Unauthenticated`, `AgencyErrors.NotFound/RegistrationNumberTaken/NotOwned`,
     `AgentErrors.NotFound/RkIdTaken/NotOwned/AlreadyInAgency/NotAgencyAdmin/HasAdverts`,
     `AdvertErrors.NotOwned`, every `PhotoErrors` member. Draft wording:
     - `user.unauthenticated` — "Přihlášení je neplatné, přihlaste se prosím znovu."
     - `agency.not_found` — "Taková realitní kancelář neexistuje."
     - `agency.registration_number_taken` — "Kancelář s tímto IČO už existuje."
     - `agency.not_owned` — "Tato kancelář není vaše."
     - `agent.not_found` — "Takový makléř neexistuje."
     - `agent.rkid_taken` — "Kancelář už má makléře s tímto RK ID."
     - `agent.not_owned` — "Tento makléř patří jiné kanceláři."
     - `agent.already_in_agency` — "Makléř už patří do jiné kanceláře."
     - `agent.not_agency_admin` — "Tuto akci může provést jen správce kanceláře."
     - `agent.has_adverts` — "Makléř má stále inzeráty."
     - `advert.not_owned` — "Tento inzerát patří jiné kanceláři nebo makléři."
     - `photo.not_found` — "Taková fotografie neexistuje."
     - `photo.rkid_taken` — "Inzerát už má fotografii s tímto RK ID."
     - `photo.too_large` — "Soubor je příliš velký."
     - `photo.unsupported_format` — "Nepodporovaný formát obrázku."
     - `photo.limit_reached` — "Inzerát už má nejvyšší povolený počet fotografií."
   - `Client/_Imports.razor` — add `@using Client.Features.RealtyAgency`,
     `Client.Features.RealtyAgent`, `Client.Features.SRealty`,
     `Shared.RealtyAgency`, `Shared.Shared`, `Shared.SRealty.Advert.ListAdverts`,
     `Shared.SRealty.Photo`.

Done when the solution builds and every `ApiError` code in Shared has an
`ApiErrorMessages` entry (a quick test: reflect over the `*Errors` classes and
assert `Resolve(code) is not null`).

### Phase 6 — Server: RealtyAgency slice (medium)

Goal: five handlers, two service additions, tests. Agencies before agents because
`CreateRealtyAgent` needs an agency to attach to.

1. `Server/Features/RealtyAgent/Entity/RealtyAgentEntity.cs` — add the ownership
   helper decision 1 rests on:

   ```csharp
   /// <summary>Whether the agent administers the given agency. Read from the row, not the token, which may be stale.</summary>
   public bool IsAdminOf(Guid agencyId) => RealtyAgencyId == agencyId && AgentRole == AgentRoleEnum.AgencyAdmin;
   ```

2. Services for decision 3.
   - `AdvertService.cs` — new method, called inside the agency delete transaction:

     ```csharp
     /// <summary>Releases every advert of an agency to its seller: the agency link and both agency-scoped keys go, the advert stays.</summary>
     public async Task DetachAgencyAdvertsAsync(Guid realtyAgencyId, CancellationToken cancellationToken = default)
     {
         var ids = await appDbContext.SrealityAdverts.Where(a => a.RealtyAgencyId == realtyAgencyId)
             .Select(a => a.Id).ToListAsync(cancellationToken);
         await appDbContext.SrealityAdverts.Where(a => a.RealtyAgencyId == realtyAgencyId)
             .ExecuteUpdateAsync(s => s
                 .SetProperty(a => a.RealtyAgencyId, (Guid?)null)
                 .SetProperty(a => a.AdvertRkId, (string?)null)
                 .SetProperty(a => a.SellerRkId, (string?)null)
                 .SetProperty(a => a.UpdatedAt, DateTimeOffset.UtcNow), cancellationToken);
         foreach (var id in ids)
         {
             await outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(id), cancellationToken);
         }
     }
     ```

   - `RealtyAgentService.cs` — `DetachAgencyAgentsAsync(Guid agencyId, ct)`:
     `ExecuteUpdateAsync` setting `RealtyAgencyId` and `RealtyAgentRkId` to null for
     `RealtyAgencyId == agencyId` (the same two fields `LeaveAgencyAsync` clears; the
     role is left alone, as there).
   - `RealtyAgencyService.cs` — constructor becomes
     `(AppDbContext appDbContext, RealtyAgentService realtyAgentService, AdvertService advertService)`;
     `DeleteAgencyAsync` becomes

     ```csharp
     await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
     await realtyAgentService.DetachAgencyAgentsAsync(agency.Id, cancellationToken);
     await advertService.DetachAgencyAdvertsAsync(agency.Id, cancellationToken);
     appDbContext.RealtyAgencies.Remove(agency);
     await appDbContext.SaveChangesAsync(cancellationToken);
     await transaction.CommitAsync(cancellationToken);
     ```

     All three services share the scoped `AppDbContext`, so one transaction covers
     them. Delete `FindAgencyByAgentIdAsync` and `FindAgencyWithAgentsAsync`
     (duplicates of `RealtyAgentService.FindAgentWithAgencyAsync` /
     `GetAgencyAgentsAsync`, no callers).
3. `GetRealtyAgency.cs` — both handlers:
   `Respond(await realtyAgencyService.FindAgencyBy...Async(..., cancellationToken))`;
   `Respond`: null → `AgencyErrors.NotFound.ToResult()`, else
   `TypedResults.Ok(agency.ToDto())`. Correct the service doc (lines 9-12): nothing
   is cached; the portal client sends a bearer token so it never hits the output
   cache anyway.
4. `GetRealtyAgencies.cs` — signature
   `(string? name, RealtyAgencyService realtyAgencyService, CancellationToken cancellationToken, int page = 1, int pageSize = DefaultPageSize)`
   with `DefaultPageSize = 20`, `MaxPageSize = 100` and the `ValidationProblem`
   guard copied from `GetFilteredAdverts.cs:39-58`; then
   `SearchAgenciesAsync(name, page, pageSize, ct)` → `PagedResult<RealtyAgencyDto>`.
5. `CreateRealtyAgency.cs` — flow: `FindCallingAgentAsync` → null: `NotAnAgent` ·
   `agent.RealtyAgencyId is not null`: `AgentErrors.AlreadyInAgency` ·
   `FindAgencyByRegistrationNumberAsync` not null: `RegistrationNumberTaken` ·
   `var agency = await CreateAgencyAsync(request.ToEntity())` ·
   `await realtyAgentService.JoinAgencyAsync(agent, agency, agent.RealtyAgentRkId)`
   and `SetAgentRoleAsync(agent, AgencyAdmin)` (or set both fields and one
   `UpdateAgentAsync`) ·
   `TypedResults.CreatedAtRoute(agency.ToDto(), "GetRealtyAgencyById", new { agencyId = agency.Id })`.
   Inject `RealtyAgentService` and `RealtyAgencyService` only.
6. `UpdateRealtyAgency.cs` — caller → `NotAnAgent` · `FindAgencyByIdAsync` null →
   `NotFound` · `!agent.IsAdminOf(agency.Id)` → `AgencyErrors.NotOwned` ·
   registration number changed and taken by another id → `RegistrationNumberTaken` ·
   `request.UpdateEntity(agency)`, `UpdateAgencyAsync` · `Ok(agency.ToDto())`.
7. `DeleteRealtyAgency.cs` — caller → `NotAnAgent` · agency null → `NotFound` ·
   `!agent.IsAdminOf(agency.Id)` → `NotOwned` · `DeleteAgencyAsync` · `NoContent`.
   The caller's own row is detached too, so the client refreshes its token
   afterwards (phase 10).
8. Tests.
   - `Tests/Server.Tests/TestAccounts.cs` — add
     `RegisterAgentAsync(HttpClient client, string userName)`: register →
     `POST api/realty-agent/become` → `POST api/user/refresh` → sets the bearer and
     returns `(RegisterUserResponse Account, RealtyAgentDto Agent)`. Replace the
     copies in `GetFilteredAdvertsTests.cs:116-133` (drop the `X-Test-Seller`
     header: `SellerId` comes from the returned agent) and
     `RequestValidationTests.cs:102-119`. Add
     `CreateAgencyAsync(client, name, registrationNumber)` that posts and
     re-refreshes the token.
   - New `Tests/Server.Tests/Features/RealtyAgency/RealtyAgencyTests.cs`: create →
     201 with id, founder's next token carries `agency_id` and
     `agent_role = AgencyAdmin`; create twice with one registration number → 409;
     second agency by an agent already in one → 409; get by id and by registration
     number anonymously → 200, unknown → 404; list with `name=` → filtered (SQLite
     cannot run the trigram branch, so test the no-name path and assert the search
     path only on Postgres if you add Testcontainers later); update by a plain agent
     of the agency → 403, by its admin → 200; delete with two agents and two adverts
     → 204, both agents now have null agency, both adverts still exist with null
     `RealtyAgencyId`.

Done when the seven agency tests pass and
`GET api/realty-agency?page=1&pageSize=20` returns a `TotalCount` larger than the
page when more agencies exist.

### Phase 7 — Server: RealtyAgent slice (medium)

Goal: seven handler bodies, the account lookup, the agents list, and `BecomeAgent`
returning the shared DTO.

1. Account lookup for decision 7 — new `Server/Features/User/FindUser/FindUser.cs`:
   `group.MapGet("", FindUserAsync).WithName("FindUser").RequireAuthorization()` with
   `(string email, UserService userService, CancellationToken ct)` →
   `FindUserByEmailAsync` → 404 `UserErrors.NotFound` or
   `Ok(new GetUserProfileResponse { Id, UserName })`. Map it in `UserEndpoints`.
   `GetUserProfile.cs` fills the same two fields.
2. `GetRealtyAgent.cs` — put `RequireAuthorization(AgentPolicies.AgentOnly)` on the
   by-id route too (open question 4). By id: `FindAgentByIdAsync` → `Respond`. By
   rk: caller → `NotAnAgent`; `RealtyAgencyId is null` → `NoAgency`;
   `FindAgentByRkIdAsync(caller.RealtyAgencyId.Value, agentRkId)` → `Respond`.
   `Respond`: null → `AgentErrors.NotFound`, else `Ok(agent.ToDto())`.
3. `CreateRealtyAgent.cs` — caller → `NotAnAgent` · `caller.RealtyAgencyId is null`
   → `NoAgency` · `caller.AgentRole != AgencyAdmin` → `NotAgencyAdmin` ·
   `request.RealtyAgencyId` given and different → `AgencyErrors.NotOwned` ·
   `userService.FindUserByIdAsync(request.UserId)` null → `UserErrors.NotFound` ·
   rk-id given and `FindAgentByRkIdAsync` hits → `RkIdTaken` · existing agent row:
   in another agency → `AlreadyInAgency`, agency-less → `JoinAgencyAsync` +
   `SetAgentRoleAsync` (open question 2), none →
   `CreateAgentAsync(new RealtyAgentEntity { UserId, AgentRole = request.AgentRole!.Value, RealtyAgencyId = caller.RealtyAgencyId, RealtyAgentRkId = request.RealtyAgentRkId })`
   · `CreatedAtRoute(agent.ToDto(), "GetRealtyAgentById", new { agentId = agent.UserId })`.
   Keep `UserService` in this handler's signature (it is the one agent handler that
   needs an account).
4. `UpdateRealtyAgent.cs` — by id: `FindAgentByIdAsync`; by rk: caller's agency +
   `FindAgentByRkIdAsync`; `UpdateResolvedAsync(agent, request, caller, service, ct)`:
   null → `NotFound` · `agent.RealtyAgencyId != caller.RealtyAgencyId` →
   `AgentErrors.NotOwned` · `!caller.IsAdminOf(agent.RealtyAgencyId!.Value)` →
   `NotAgencyAdmin` · rk-id changed and `FindAgentByRkIdAsync` hits another agent →
   `RkIdTaken` · `request.UpdateEntity(agent)`, `UpdateAgentAsync` ·
   `Ok(agent.ToDto())`. (An admin demoting themselves is allowed; guarding "the
   last admin" is a later nicety.)
5. `DeleteRealtyAgent.cs` — same resolution; `DeleteResolvedAsync`: null →
   `NotFound` · allowed when `caller.UserId == agent.UserId` or
   `caller.IsAdminOf(agent.RealtyAgencyId ?? Guid.Empty)`, else `NotOwned` · open
   question 1: `appDbContext.SrealityAdverts.AnyAsync(a => a.SellerId == agent.UserId)`
   (expose as `AdvertService.HasSellerAdvertsAsync`) → `HasAdverts` ·
   `DeleteAgentAsync` · `NoContent`.
6. Agents list — new `Server/Features/RealtyAgent/GetRealtyAgents/GetRealtyAgents.cs`:
   `GET /realty-agent?agencyId=&page=&pageSize=`, `AgentOnly`; caller must belong to
   `agencyId` (else `AgencyErrors.NotOwned`); `GetAgencyAgentsPageAsync` →
   `PagedResult<RealtyAgentDto>`; same paging guard as phase 6 step 4. This backs
   the `agency/agents` page.
7. `BecomeAgent.cs:48-54` — return `TypedResults.Ok(agent.ToDto())`; delete
   `Shared/RealtyAgent/BecomeAgent/BecomeAgentResponse.cs`; update
   `Client/Features/RealtyAgent/RealtyAgentApiClient.cs:15-20` and
   `BecomeAgentTests.cs:31` to `RealtyAgentDto`.
8. New `Tests/Server.Tests/Features/RealtyAgent/RealtyAgentTests.cs`: admin adds a
   registered account by id → 201, the account's next token carries the agency;
   adding the same account twice → 409 (or 200 adoption if agency-less, per your
   answer to open question 2); rk-id reuse → 409; plain agent adding someone → 403;
   update role by admin → 200, by plain agent → 403; get by rk from another agency
   → 404; delete own profile → 204, colleague's by admin → 204, agent with adverts
   → 409; agents list → only that agency's agents, correct `TotalCount`.

Done when the agent tests pass and `grep -rn NotImplementedException Server` lists
only the photo slice.

### Phase 8 — Server: Photo slice (large)

Goal: storage, service, five handlers (three existing, two new anonymous ones),
cleanup on advert delete, tests against the in-memory store.

1. Configuration — new `Server/Features/SRealty/Photo/PhotoStorageOptions.cs`
   (`SectionName = "PhotoStorage"`, `string RootPath = "photos"`);
   `Server/Program.cs:91` →
   `builder.Services.Configure<PhotoStorageOptions>(builder.Configuration.GetSection(PhotoStorageOptions.SectionName));`
   before the storage registration; `Server/appsettings.json` →
   `"PhotoStorage": { "RootPath": "photos" }` (relative paths resolve under
   `IHostEnvironment.ContentRootPath`). Add `photos/` to `.gitignore`.
2. Image sniffing — new `Server/Features/SRealty/Photo/PhotoContentType.cs`:
   `static string? Detect(Stream stream)` reads the first 12 bytes and returns
   `image/jpeg` (`FF D8 FF`), `image/png` (`89 50 4E 47 0D 0A 1A 0A`), `image/webp`
   (`RIFF....WEBP`) or null, then seeks back to 0. This replaces
   `IPhotoStorage.ValidateAsync`; the client-declared content type is never trusted.
3. `FilePhotoStorage.cs` — constructor
   `(IOptions<PhotoStorageOptions> options, IHostEnvironment environment)`;
   root = `Path.GetFullPath(Path.Combine(environment.ContentRootPath, options.Value.RootPath))`.
   - `SaveAsync`: relative path `{advertId:N}/{photoId:N}{ext}` with `ext` from the
     content type (`.jpg`/`.png`/`.webp`); `Directory.CreateDirectory`;
     `FileMode.CreateNew`; return the relative path.
   - `OpenReadAsync`:
     `var full = Path.GetFullPath(Path.Combine(root, storagePath)); if (!full.StartsWith(root + Path.DirectorySeparatorChar)) return null;`
     then `File.Exists ? File.OpenRead(full) : null`.
   - `DeleteAsync`: same guard, `File.Delete` if present, remove the advert
     directory when empty.
4. `PhotoService.cs` — constructor
   `(AppDbContext appDbContext, IPhotoStorage photoStorage, IOutputCacheStore outputCache)`;
   every write ends with
   `outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(advertId), ct)` because
   the photo list route (step 7) is cached under the advert tag.
   - Reads: `GetAdvertPhotosAsync` =
     `Where(SrealityAdvertId == advertId).OrderBy(Order).ToListAsync`; the three
     finders are single `FirstOrDefaultAsync` calls.
   - Ordering rule (used by add, update, delete): compute the target list of ids,
     then call `ReorderPhotosAsync`, which assigns `Order = index` in two passes to
     respect the unique `(SrealityAdvertId, Order)` index — Postgres checks it row by
     row, so a plain `+1` shift collides: first
     `ExecuteUpdateAsync(Order = -(Order + 1))` for the advert's photos, then set
     each photo's final `Order` and save. Position: `request.Main == 1` → index 0;
     `request.Order` given → clamped to `[0, count]`; otherwise append.
   - `AddPhotoAsync`:
     `new SrealityAdvertPhotoEntity { SrealityAdvertId = advert.Id, ContentType = contentType, StoragePath = "" }`
     → `StoragePath = await photoStorage.SaveAsync(advert.Id, photo.Id, content, contentType, ct)`
     → metadata from the request → insert at the computed position → save → evict.
   - `UpdatePhotoAsync`: when `content` is not null, `DeleteAsync(old)` then
     `SaveAsync` and set `ContentType`; copy `PhotoRkId`, `PhotoKind` (default
     `Photo` when null), `RoomType`, `Alt`; if `Order`/`Main` given, move; save;
     evict.
   - `DeletePhotoAsync`: remove the row, close the gap through `ReorderPhotosAsync`,
     `photoStorage.DeleteAsync`, evict. `DeleteAdvertPhotosAsync`: load, delete
     every file, `RemoveRange`, save (no eviction needed — the advert itself is
     going).
5. `AdvertService.cs:98-103` — inject `PhotoService` and call
   `photoService.DeleteAdvertPhotosAsync(advert.Id, ct)` before `Remove(advert)`.
   Register order in `Program.cs` is irrelevant (both scoped, no cycle:
   `PhotoService` does not depend on `AdvertService`).
6. Handlers. Shared resolution in a new
   `Server/Features/SRealty/Photo/PhotoHandlerSupport.cs`:
   `ResolveAdvertAsync(Guid? advertId, string? advertRkId, RealtyAgentEntity agent, AdvertService, ct)`
   → `(SrealityAdvertEntity? advert, IResult? refusal)` covering `NoAgency` on rk,
   `AdvertErrors.NotFound`, `AdvertErrors.NotOwned` (`advert.IsOwnedBy(agent)`); and
   `ResolvePhotoAsync(advert, Guid? photoId, string? photoRkId, PhotoService, ct)` →
   `PhotoErrors.NotFound`.
   - **Verify** multipart field names. Minimal-API `[FromForm]` binding of a record
     does not read `[JsonPropertyName]`. Before writing the handlers, post
     `photo_rkid=x` and `PhotoRkId=x` to the existing `UploadPhoto` stub with a
     one-line body that echoes `request`. If only PascalCase binds, replace
     `[FromForm] PhotoRequest request` with `IFormCollection form` and a small
     `PhotoRequest ReadForm(IFormCollection)` in `PhotoHandlerSupport` that reads
     the snake_case keys explicitly — deterministic and free of binding magic.
   - `UploadPhoto.cs`: caller → resolve advert → `file.Length > PhotoLimits.MaxBytes`
     → `TooLarge` → `PhotoContentType.Detect` null or not in `AllowedContentTypes` →
     `UnsupportedFormat` → `GetAdvertPhotosAsync(advert.Id).Count >= MaxPerAdvert` →
     `LimitReached` → `photoRkId ?? request.PhotoRkId` taken → `PhotoErrors.RkIdTaken`
     → `AddPhotoAsync` →
     `CreatedAtRoute(photo.ToDto(), "GetPhoto", new { advertId = advert.Id, photoId = photo.Id })`.
   - `EditPhoto.cs`: caller → advert → photo → optional file through the same
     size/format checks → rk-id uniqueness when changed →
     `UpdatePhotoAsync(photo, stream, contentType, request)` → `Ok(photo.ToDto())`.
   - `DeletePhoto.cs`: caller → advert → photo → `DeletePhotoAsync` → `NoContent`.
     Give it an `[AsParameters] DeletePhotoRoute` like `EditPhotoRoute` so the two
     match (one shared `PhotoRoute` record works for both).
   - New `Server/Features/SRealty/Photo/PhotoMapper.cs`: Mapperly `ToDto` mapping
     `SrealityAdvertId` → `AdvertId`, ignoring source `Advert`, `StoragePath`,
     `CreatedAt`, `UpdatedAt`.
7. Anonymous routes — `SRealtyEndpoints.cs`: add a third group
   `var publicPhotoGroup = group.MapGroup(AdvertPrefix).WithTags(PhotoTag);` (no
   authorization) and map:
   - New `Server/Features/SRealty/Photo/GetPhotos/GetPhotos.cs`:
     `GET /{advertId:guid}/photo` → `GetAdvertPhotosAsync` → `Ok(List<PhotoDto>)`,
     `.CacheOutput(p => p.AddPolicy<AdvertOutputCachePolicy>().Expire(TimeSpan.FromMinutes(5)))`
     so photo writes evict it with the advert tag.
   - New `Server/Features/SRealty/Photo/GetPhoto/GetPhoto.cs`:
     `GET /{advertId:guid}/photo/{photoId:guid}` named `"GetPhoto"` →
     `FindPhotoByAdvertIdAndIdAsync` → null → `PhotoErrors.NotFound` →
     `photoStorage.OpenReadAsync(photo.StoragePath)` → null → `NotFound` →
     `TypedResults.File(stream, photo.ContentType, lastModified: photo.UpdatedAt, enableRangeProcessing: true)`.
     Not output-cached (bodies up to 10 MB); the browser caches by `Last-Modified`.
8. Tests — fill `InMemoryPhotoStorage`; new
   `Tests/Server.Tests/Features/SRealty/Photo/PhotoTests.cs`: upload a 1×1 PNG
   (hard-code the bytes) → 201 with `Order 0`; upload a second with `main=1` → it
   takes order 0 and the first moves to 1; anonymous list → two entries in order;
   anonymous GET image → 200 with `image/png`; upload a text file → 415; upload by
   another agency's agent → 403; edit metadata only → 200; delete first → the
   remaining photo is order 0; delete the advert → the store is empty; rk-id
   routes: upload with `rk/P1` then edit via `photo/rk/P1`.

Done when `grep -rn NotImplementedException Server` is empty and the photo tests
pass.

### Phase 9 — Client API clients (medium)

Goal: every method is one HTTP call plus `ApiErrorReader.ReadAsync<T>` /
`ReadErrorAsync`; paths from the Shared constants.

1. `RealtyAgencyApiClient.cs` — `GetAgencyAsync`: `GET {Prefix}/{agencyId}` ·
   `GetAgencyByRegistrationNumberAsync`:
   `GET {Prefix}/{Registration}/{Uri.EscapeDataString(number)}` ·
   `GetAgenciesAsync`: `GET {Prefix}?page=&pageSize=` plus `&name=` when given
   (always send page and pageSize) · `CreateAgencyAsync`: `PostAsJsonAsync` ·
   `UpdateAgencyAsync`: `PutAsJsonAsync` · `DeleteAgencyAsync`: `DeleteAsync` →
   `ReadErrorAsync`. Fix the doc comment (`/realtyagency` → `/realty-agency`).
2. `RealtyAgentApiClient.cs` — `BecomeAgentAsync` → `RealtyAgentDto`;
   `GetAgentAsync`, `GetAgentByRkIdAsync` (`{Prefix}/{Rk}/{rkId}`),
   `CreateAgentAsync`, `UpdateAgentAsync`, `DeleteAgentAsync`; new
   `GetAgencyAgentsAsync(Guid agencyId, int page, int pageSize)` →
   `PagedResult<RealtyAgentDto>`.
3. `UserApiClient.cs` — new `FindUserByEmailAsync(string email)` →
   `GET user?email=` → `GetUserProfileResponse`. It needs the bearer client, but
   `UserApiClient` is deliberately built on the plain one (`UserFeature.cs:16`);
   put the method on a new `UserProfileApiClient(HttpClient)` registered with
   `AddScoped` in `UserFeature` so it receives the bearer client.
4. `AdvertApiClient.cs` — `GetFilteredAdvertsAsync` must actually append
   `ToQueryString(...)` (today it sends the bare path); add `GetAdvertAsync(Guid)`,
   `CreateAdvertAsync(SrealityAdvertDto)` → `CreateAdvertResponse`,
   `UpdateAdvertAsync(Guid, dto)` → `SrealityAdvertDto`, `DeleteAdvertAsync(Guid)`,
   `GetPhotosAsync(Guid advertId)` → `List<PhotoDto>`,
   `UploadPhotoAsync(Guid advertId, IBrowserFile file, PhotoRequest request)`,
   `EditPhotoAsync(Guid advertId, Guid photoId, IBrowserFile? file, PhotoRequest request)`,
   `DeletePhotoAsync(Guid advertId, Guid photoId)`, and
   `PhotoUrl(Guid advertId, Guid photoId)` =
   `new Uri(httpClient.BaseAddress!, $"{SRealtyRoutes.Prefix}/{SRealtyRoutes.Advert}/{advertId}/{SRealtyRoutes.Photo}/{photoId}")`
   for `<img src>`. Multipart: `MultipartFormDataContent` with
   `StreamContent(file.OpenReadStream(PhotoLimits.MaxBytes))` named `"file"` (file
   name preserved) and one `StringContent` per non-null request field under the
   snake_case names.
5. Client tests — new
   `Tests/Client.Tests/Features/RealtyAgency/RealtyAgencyApiClientTests.cs` etc.
   with `StubHttpHandler`: assert the request path and method for each call and
   that a problem response with `errorCode` maps to the Czech text.

Done when `grep -rn NotImplementedException Client` lists only the pages.

### Phase 10 — Client pages (large)

Goal: layouts and gates first, then each page body. Czech text throughout; enum
labels through `GetLocalizedDisplayName`.

1. Layouts and links.
   - `@layout AgencyLayout` on `CreateRealtyAgencyPage`, `UpdateRealtyAgencyPage`,
     `CreateRealtyAgentPage`, `UpdateRealtyAgentPage` and the new agents page;
     `MainLayout` (default) stays for the public agency list and detail.
   - `Client/Layout/AgencyLayout.razor:28-29` — `href="agency/agents"` stays (page
     below); `href="agency/settings"` → `href="agency/@AuthState.AgencyId"`.
   - `Client/Layout/NavMenu.razor:25-29` — delete the `RequireRole` block (no admin
     area exists); leave `SuperAdminLayout` in place, unused, until one does.
   - New `Client/Features/RealtyAgent/GetRealtyAgents/AgencyAgentsPage.razor`:
     `@page "/agency/agents"`, `@layout AgencyLayout`; lists
     `GetAgencyAgentsAsync(AuthState.AgencyId!.Value, page, 20)` in a table with
     role label and rk-id, "Přidat makléře" → `agent/create` for
     `RequireAgentRole Role="AgentRoleEnum.AgencyAdmin"`.
2. Agency pages.
   - `RealtyAgencyListPage`: inject nothing new; `LoadAsync` =
     `GetAgenciesAsync(name, page, PageSize)`; markup: search box (`FloatingText` +
     "Hledat"), a table (name, IČO, e-mail) linking to `agency/{Id}`, previous/next
     from `TotalPages`; show "Založit kancelář" only inside `<RequireAgentRole>`
     when `AuthState.AgencyId is null`.
   - `RealtyAgencyDetailPage`: inject `AuthStateService`; `LoadAsync` =
     `GetAgencyAsync(Id)`; show Edit/Delete only when
     `AuthState.IsAgencyAdmin && AuthState.AgencyId == Id`; `DeleteAsync` →
     `DeleteAgencyAsync(Id)` → `await AuthState.TryRefreshAsync(force: true)` (the
     caller was detached) → navigate to `agency`.
   - `CreateRealtyAgencyPage`: inject `AuthStateService`; form = `BootstrapEditForm`
     + `DataAnnotationsValidator` + `ValidationSummary` + `FloatingText` for `Name`,
     `RegistrationNumber` ("IČO"), `Email` (`Type="email"`); `SubmitAsync` →
     `CreateAgencyAsync(agency)` → `TryRefreshAsync(force: true)` → navigate to
     `agency/{response.Id}`. Title "Založení realitní kanceláře".
   - `UpdateRealtyAgencyPage`: `LoadAsync` → `GetAgencyAsync(Id)`; same form;
     `SubmitAsync` → `UpdateAgencyAsync(Id, agency)` → navigate to detail.
3. Agent pages.
   - `CreateRealtyAgentPage`: inject `UserProfileApiClient`; step 1: e-mail field +
     "Vyhledat" → `FindUserByEmailAsync` → show the user name and set
     `agent.UserId`; step 2: `EnumSelect TEnum="AgentRoleEnum"` for `AgentRole` and
     `FloatingText` for `RealtyAgentRkId`; `SubmitAsync` → `CreateAgentAsync(agent)`
     → navigate to `agency/agents`.
   - `RealtyAgentDetailPage`: inject `AuthStateService`; shows role label, rk-id,
     agency link; Edit/Delete when
     `AuthState.IsAgencyAdmin && AuthState.AgencyId == agent.RealtyAgencyId` or
     `agent.UserId == AuthState.Claims!.UserId`; `DeleteAsync` →
     `DeleteAgentAsync(Id)` → if self: `TryRefreshAsync(force: true)` and navigate
     home, else `agency/agents`.
   - `UpdateRealtyAgentPage`: load, role + rk-id form, `UpdateAgentAsync(Id, agent)`;
     if editing self, `TryRefreshAsync(force: true)`.
4. `Client/Features/SRealty/Components/AdvertForm.razor` — add
   `<ValidationSummary class="alert alert-danger"/>` above the buttons;
   `AdvertCoreSection.razor:43-46` — delete the free-text `SellerRkId` control. The
   pages set the seller from the token before rendering:
   `advert.SellerId = AuthState.Claims!.UserId` (never `SellerRkId`; the server
   stamps `SellerId` anyway).
5. `CreateAdvertPage.razor` — inject `AdvertApiClient`, `AuthStateService`,
   `NavigationManager`;
   `<AdvertForm Model="advert" OnValidSubmit="SubmitAsync" Busy="busy" Error="error" SubmitLabel="Vytvořit inzerát"/>`;
   `SubmitAsync` → `CreateAdvertAsync(advert)` → navigate to
   `srealty/{response.Advert!.AdvertId}`. Delete the stale TODO comment (line 9).
6. `UpdateAdvertPage.razor` — `OnParametersSetAsync` → `GetAdvertAsync(Id)` into a
   nullable `advert` (render the form only when loaded); `SubmitAsync` →
   `UpdateAdvertAsync(Id, advert)` → navigate to detail. Delete the comment at
   line 9.
7. `ListAdverts/AdvertListPage.razor` — a filter card bound to an `AdvertFilter`
   instance (now settable): `EnumSelect` for function/type, `EnumCheckBoxGroup` for
   subtypes and building conditions, `FloatingText` for city and search,
   `FloatingNumber` for price/area bounds, an `EnumSelect TEnum="AdvertSortEnum"`;
   results as Bootstrap cards linking to `srealty/{AdvertId}`, paging from
   `TotalPages`. Delete the comment at line 8.
8. `GetAdvert/AdvertDetailPage.razor` — load `GetAdvertAsync(Id)` and
   `GetPhotosAsync(Id)`; gallery of
   `<img src="@Adverts.PhotoUrl(Id, photo.Id)" alt="@photo.Alt">` in `Order`; a
   description/price/location block; Edit/Delete for `RequireAgentRole` when the
   advert's seller is the caller (`advert.SellerId == AuthState.Claims!.UserId`) —
   the server enforces ownership anyway. Delete the comment at line 8.
9. Czech `PageTitle`s and headings on all eleven pages; the existing English pages
   (Login, Register, NavMenu, layouts, BecomeAgent, NotFound, Home) are converted in
   phase 11.

Done when `grep -rn NotImplementedException Client Server Shared` is empty and each
page round-trips against the running AppHost.

### Phase 11 — Cleanup sweep and deferred items (medium)

Goal: nothing above depends on these; doing them earlier only churns files phases
4–10 rewrite.

1. Mechanical pass: final newlines (about 85 files lack the one `.editorconfig`
   requires — `dotnet format` fixes them), unused usings, delete
   `CertificateTypeEnum`, `CooperativeTransferEnum`, `FlatTypeEnum`,
   `HeatingTypeEnum` (referenced nowhere), the empty
   `Server/Infrastructure/DependencyInjection` folder and the `<Folder>` items in
   `Server/Server.csproj:33-43`, the unused packages
   (`Microsoft.Extensions.Caching.Hybrid`, `Aspire.StackExchange.Redis.DistributedCaching`,
   `Npgsql...NetTopologySuite`, `Swashbuckle.AspNetCore`, `NSubstitute`), the
   `SRealtyProperties`-era comment in `Program.cs:38`.
2. Auth polish: `Login.cs:27-28` keep e-mail only (drop `?? FindByNameAsync` and
   the doc at 19-21) — or drop `[EmailAddress]` if you want user-name sign-in;
   `AccessTokenService.cs:79-83` revoke all sessions only when
   `token.ReplacedByHash is not null` (a rotated token replayed), a logout-revoked
   token just returns null; `GetCurrentUser.cs:14` add `.RequireAuthorization()`;
   `Logout.cs` verify the refresh token belongs to the caller
   (`token.UserId == principal.GetUserId()`) before revoking; move
   `AccessTokenService` and `JwtSigningKey` into `Server/Features/User/`; move each
   `IEntityTypeConfiguration` next to its entity (`ApplyConfigurationsFromAssembly`
   keeps finding them); move `SearchStringExtensions` to
   `Server/Features/RealtyAgency/`.
3. Configuration beyond Development: `Client/wwwroot/appsettings.json` with a
   production `ServerAPI`; `Jwt:*` in the server's non-Development configuration
   (user secrets or environment); `Server/Program.cs` call
   `app.MapDefaultEndpoints()` so `/health` and `/alive` exist for Aspire;
   `Client/wwwroot/index.html:7,30` remove the template title and the
   `aspnetcore-browser-refresh.js` script; delete `wwwroot/sample-data/weather.json`.
4. Contracts: labels for the 15 unlabeled `HeatingSourceEnum` members;
   `SrealityAdvertDto.Auction.cs:36-48` make the `RequiredIfValue` on
   `PriceMinimumBid`/`PriceExpertReport` match their docs; one language for
   validation messages (Czech in `RequiredIfValueAttribute`, `EnumValueAttribute`
   and the paging guards, since the same attributes run in the Blazor forms);
   `UserErrors.EmailTaken`/`UserNameTaken` → 409 like every other "already exists"
   (a client test pins 400 today — update it).
5. UI language: convert Login, Register, NavMenu, AgencyLayout/SuperAdminLayout,
   AccessDeniedPanel, BecomeAgent, NotFound and Home to Czech;
   `AuthStateService.cs:62,75` fallbacks too.
6. RÚIAN: a refresh script (PowerShell or a small console project in the solution)
   that downloads the three ČÚZK zips, converts Windows-1250 semicolon files to
   comma-separated UTF-8 without BOM, drops rows with `PLATI_DO`, and overwrites
   `Server/Features/Ruian/Data/*.csv`; then extend
   `AdvertService.ResolveLocalityAsync` for Street/Building/Address-level codes once
   the register carries them; geocoding as its own piece of work afterwards.

## Corrections to TODO.md

- **§1 (caller resolution)** — the block is verbatim only in the three by-rk
  handlers. `UpdateAdvertById`/`DeleteAdvertById` deliberately stop after the agent
  check, `CreateAdvert` applies `NoAgency` only when a key is given, `BecomeAgent`
  inverts the test. The extraction replaces the user+agent lookup only; `NoAgency`
  stays per route. The `ClaimsPrincipal` extension (`UserClaims.GetUserId`) avoids
  the scalar-returning service method the TODO worried about.
- **§2 (cancellation tokens)** — also `AccessTokenService` (passes
  `CancellationToken.None` at line 149), `IPhotoStorage`/`FilePhotoStorage`, and
  the `GetUserProfile`/`Logout` handlers.
- **§3 (route strings)** — `realtyagent/become` and `srealty/advert` in the client
  match the server today; only the first changes, together with the prefix rename
  and the seven server-test sites. `AdvertApiClient`'s real defect is that it never
  appends its query string.
- **"RealtyAgencyService is implemented"** — its list and search methods cannot
  execute (NotMapped `SearchName`).
- **RÚIAN CSV note** — describes the ČÚZK source, not the repository's files, which
  are comma-separated UTF-8 as the importer requires.
- **Missing rows** — `GetUserProfile` returns `{}`; no agents list endpoint although
  `AgencyLayout` links one; no photo list route or DTO; the search-vector
  configuration is disabled in the model.
- **"Leave as is: UploadPhoto four routes"** — `EditPhoto` and `DeletePhoto` map four
  routes each too; the plan keeps all three that way.

## Deliberately later

- Geocoding (address or GPS → RÚIAN code): the register already stores GPS and
  resolves by unique municipality name, so the question is narrower than the TODO
  states.
- Testcontainers Postgres + Redis so trigram, tsvector and `czech_unaccent` paths
  get real coverage; today's SQLite host cannot run them.
- SuperAdmin area (`admin/agencies`, `admin/users`) and the bootstrap of the first
  SuperAdmin account.
- HybridCache read-through for `RuianService` if the package stays.
