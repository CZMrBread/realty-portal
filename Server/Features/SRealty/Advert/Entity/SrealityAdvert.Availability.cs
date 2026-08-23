namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public DateOnly? ReadyDate { get; set; }
    public DateOnly? SaleDate { get; set; }
    public DateTimeOffset? FirstTourDate { get; set; }
    public DateTimeOffset? FirstTourDateTo { get; set; }
}
