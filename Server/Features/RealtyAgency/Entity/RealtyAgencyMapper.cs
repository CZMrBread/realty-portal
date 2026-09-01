using Riok.Mapperly.Abstractions;
using Shared.RealtyAgency.CreateRealtyAgency;
using Shared.RealtyAgency.GetRealtyAgency;
using Shared.RealtyAgency.UpdateRealtyAgency;

namespace Server.Features.RealtyAgency.Entity;

/// <summary>Mapperly mapping between the agency contracts and the entity; a null required field throws.</summary>
[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    ThrowOnPropertyMappingNullMismatch = true)]
public static partial class RealtyAgencyMapper
{
    // --- request -> a new entity (create); the portal assigns the identifier and the service derives the search name ---

    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.Id))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.SearchName))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.Agents))]
    public static partial RealtyAgencyEntity ToEntity(this CreateRealtyAgencyRequest request);

    // --- request -> an existing entity (update) ---

    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.Id))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.SearchName))]
    [MapperIgnoreTarget(nameof(RealtyAgencyEntity.Agents))]
    public static partial void UpdateEntity(this UpdateRealtyAgencyRequest request, RealtyAgencyEntity entity);

    // --- entity -> the response each slice answers with ---

    [MapperIgnoreSource(nameof(RealtyAgencyEntity.CreatedAt))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.UpdatedAt))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.SearchName))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.Agents))]
    public static partial CreateRealtyAgencyResponse ToCreateResponse(this RealtyAgencyEntity entity);

    [MapperIgnoreSource(nameof(RealtyAgencyEntity.CreatedAt))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.UpdatedAt))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.SearchName))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.Agents))]
    public static partial UpdateRealtyAgencyResponse ToUpdateResponse(this RealtyAgencyEntity entity);

    [MapperIgnoreSource(nameof(RealtyAgencyEntity.CreatedAt))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.UpdatedAt))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.SearchName))]
    [MapperIgnoreSource(nameof(RealtyAgencyEntity.Agents))]
    public static partial GetRealtyAgencyResponse ToGetResponse(this RealtyAgencyEntity entity);
}
