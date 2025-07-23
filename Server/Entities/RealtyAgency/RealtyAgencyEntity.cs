namespace Server.Entities.RealtyAgency;

public class RealtyAgencyEntity: ITimeStampedEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
}