namespace Shared.SRealty.Advert.CreateAdvert;

public sealed record CreateAdvertResponse
{
    public CreateAdvertResponse(SrealityAdvertDto advert)
    {
        Advert = advert;
        Status = CreateAdvertResponseStatusEnum.OK;
        StatusMessage = "Advert created successfully.";
    }
    public CreateAdvertResponseStatusEnum Status { get; set; }
    public string StatusMessage { get; set; }
    public SrealityAdvertDto? Advert { get; set; }
}