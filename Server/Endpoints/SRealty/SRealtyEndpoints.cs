namespace Server.Endpoints.SRealty;

public static class SRealtyEndpoints
{
    public static void MapSRealtyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/srealty").WithTags("SRealty");

        group.MapPost("create", SRealtyHandlers.CreateSRealty).WithName("CreateSRealty");
        group.MapPut("update/{id:Guid}", SRealtyHandlers.UpdateSRealty).WithName("UpdateSRealty");
    }
    
}