using Riok.Mapperly.Abstractions;
using Shared.Ruian;
using Shared.Ruian.GetAddressPoints;

namespace Server.Features.Ruian.Entity;

/// <summary>Mapperly mapping from the register entities to their DTOs.</summary>
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RuianMapper
{
    [MapperIgnoreSource(nameof(RuianRegionEntity.Districts))]
    public static partial RuianPlaceDto ToDto(this RuianRegionEntity entity);

    [MapperIgnoreSource(nameof(RuianDistrictEntity.RegionCode))]
    [MapperIgnoreSource(nameof(RuianDistrictEntity.Region))]
    [MapperIgnoreSource(nameof(RuianDistrictEntity.Municipalities))]
    public static partial RuianPlaceDto ToDto(this RuianDistrictEntity entity);

    [MapperIgnoreSource(nameof(RuianMunicipalityEntity.SearchName))]
    [MapperIgnoreSource(nameof(RuianMunicipalityEntity.DistrictCode))]
    [MapperIgnoreSource(nameof(RuianMunicipalityEntity.District))]
    public static partial RuianPlaceDto ToDto(this RuianMunicipalityEntity entity);

    [MapperIgnoreSource(nameof(RuianMunicipalityPartEntity.SearchName))]
    [MapperIgnoreSource(nameof(RuianMunicipalityPartEntity.MunicipalityCode))]
    [MapperIgnoreSource(nameof(RuianMunicipalityPartEntity.Municipality))]
    public static partial RuianPlaceDto ToDto(this RuianMunicipalityPartEntity entity);

    [MapperIgnoreSource(nameof(RuianStreetEntity.SearchName))]
    [MapperIgnoreSource(nameof(RuianStreetEntity.MunicipalityCode))]
    [MapperIgnoreSource(nameof(RuianStreetEntity.Municipality))]
    public static partial RuianPlaceDto ToDto(this RuianStreetEntity entity);

    [MapperIgnoreSource(nameof(RuianAddressPointEntity.Municipality))]
    [MapperIgnoreSource(nameof(RuianAddressPointEntity.MunicipalityPart))]
    [MapperIgnoreSource(nameof(RuianAddressPointEntity.Street))]
    [MapperIgnoreSource(nameof(RuianAddressPointEntity.CityDistrictCode))]
    [MapperIgnoreSource(nameof(RuianAddressPointEntity.CityDistrictName))]
    [MapperIgnoreTarget(nameof(RuianAddressPointDto.Label))]
    public static partial RuianAddressPointDto ToDto(this RuianAddressPointEntity entity);
}
