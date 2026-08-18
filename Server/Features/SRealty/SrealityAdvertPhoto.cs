using Server.Features.SRealty;

namespace Server.Features.Adverts.Domain;

/// <summary>
/// Fotka inzerátu. Binárka žije v úložišti (IPhotoStorage), tady jen metadata.
/// </summary>
public class SrealityAdvertPhoto
{
    public Guid Id { get; set; }

    public Guid SrealityAdvertId { get; set; }
    public SrealityAdvertEntity Advert { get; set; } = null!;

    /// <summary>Cesta/klíč v úložišti fotek.</summary>
    public required string StoragePath { get; set; }

    /// <summary>Pořadí ve fotogalerii, 0 = úvodní.</summary>
    public int Order { get; set; }

    /// <summary>
    /// room_type z tabulky 2 spec (metoda addPhoto).
    /// tabulka pokračuje na další straně; do té doby int.
    /// </summary>
    public int? RoomType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
