namespace Shared.SRealtyRealty.ValueObjects;

public record ShareDetails
{
    public int? ShareNumerator { get; init; }

    public int? ShareDenominator { get; init; }

    public int? CommonAreaShareNumerator { get; init; }

    public int? CommonAreaShareDenominator { get; init; }
}