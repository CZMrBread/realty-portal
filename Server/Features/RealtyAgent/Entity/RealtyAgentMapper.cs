using Riok.Mapperly.Abstractions;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.Entity;

/// <summary>Mapperly mapping between the agent DTO and the entity.</summary>
[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    ThrowOnPropertyMappingNullMismatch = true)]
public static partial class RealtyAgentMapper
{
    // --- entity -> DTO ---
    // The name comes from the user account behind the agent; when the User navigation is not loaded it stays null.

    [MapProperty("User.UserName", nameof(RealtyAgentDto.UserName))]
    [MapperIgnoreSource(nameof(RealtyAgentEntity.SearchName))]
    [MapperIgnoreSource(nameof(RealtyAgentEntity.RealtyAgency))]
    [MapperIgnoreSource(nameof(RealtyAgentEntity.SRealtyProperties))]
    public static partial RealtyAgentDto ToDto(this RealtyAgentEntity entity);

    // --- DTO -> an existing entity (update) ---
    // The public profile, the role and the agency key move through an update: who the agent is and which agency
    // they work for have their own flows (create, join, leave), so those fields are ignored here.

    [MapperIgnoreTarget(nameof(RealtyAgentEntity.UserId))]
    [MapperIgnoreTarget(nameof(RealtyAgentEntity.User))]
    [MapperIgnoreTarget(nameof(RealtyAgentEntity.SearchName))]
    [MapperIgnoreTarget(nameof(RealtyAgentEntity.RealtyAgencyId))]
    [MapperIgnoreTarget(nameof(RealtyAgentEntity.RealtyAgency))]
    [MapperIgnoreTarget(nameof(RealtyAgentEntity.SRealtyProperties))]
    [MapperIgnoreTarget(nameof(RealtyAgentEntity.RegistrationNumber))]
    [MapperIgnoreSource(nameof(RealtyAgentDto.UserId))]
    [MapperIgnoreSource(nameof(RealtyAgentDto.RealtyAgencyId))]
    [MapperIgnoreSource(nameof(RealtyAgentDto.RegistrationNumber))]
    [MapperIgnoreSource(nameof(RealtyAgentDto.UserName))]
    public static partial void UpdateEntity(this RealtyAgentDto dto, RealtyAgentEntity entity);
}
