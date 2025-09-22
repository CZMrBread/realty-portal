using System.ComponentModel.DataAnnotations;
using Shared.Dtos.SRealty.RealtyImport;

namespace Server.Endpoints.SRealty;

public static class SRealtyEndpoints
{
    public static void MapSRealtyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/srealty").WithTags("SRealty");

        group.MapPost("create", (SrealityAdvertDto srealityAdvertDto) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(srealityAdvertDto);

            if (!Validator.TryValidateObject(srealityAdvertDto, validationContext, validationResults, true))
            {
                return Results.BadRequest(new {message="Validation failed", erros = validationResults});
            }

            return Results.Ok(srealityAdvertDto);
        }).WithName("CreateSRealty").WithOpenApi();
        // group.MapPut("update/{id:Guid}", SRealtyHandlers.UpdateSRealty).WithName("UpdateSRealty");
        group.MapGet("/get/{id:Guid}", () => Results.Ok("Hello")).WithName("GetSRealtyById").WithOpenApi();
        group.MapGet("/get/rkid/{rkId}", () => Results.Ok("Hello")).WithName("GetSRealtyByRkId").WithOpenApi();
    }
    
}