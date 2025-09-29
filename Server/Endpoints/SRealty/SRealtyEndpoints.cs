using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Server.Filters;
using Server.Services;
using Shared.Dtos.SRealty.RealtyImport;

namespace Server.Endpoints.SRealty;

public static class SRealtyEndpoints
{
    public static void MapSRealtyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/srealty").WithTags("SRealty");

        group.MapPost("create",
                async ([FromBody] SRealityAdvertDto sRealityAdvertDto, SRealtyHandlers service) =>
                {
                    var createdEntity = await service.CreateFromDtoAsync(sRealityAdvertDto);
                    return Results.Ok(createdEntity);
                })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("CreateSRealty")
            .WithOpenApi();
        group.MapPut("update/{id:Guid}",
            async (Guid id, SRealityAdvertDto sRealityAdvertDto, SRealtyHandlers service) =>
            {
                return Results.Ok(sRealityAdvertDto);
            });
        group.MapPut("update/rkid/{rkid}",
            async (string rkid, SRealityAdvertDto sRealityAdvertDto, SRealtyHandlers service) =>
            {
                return Results.Ok(sRealityAdvertDto);
            });
        group.MapGet("/get/{id:Guid}", async (Guid id, SRealtyHandlers service) =>
        {
            var property = await service.GetByIdAsync(id);
            if (property == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(property);
        }).WithName("GetSRealtyById").WithOpenApi();
        group.MapGet("/get/rkid/{rkId}", () => Results.Ok("Hello")).WithName("GetSRealtyByRkId").WithOpenApi();
        
        group.MapGet("/get/all", async (SRealtyHandlers service) =>
        {
            var properties = await service.GetAllAsync();
            return Results.Ok(properties);
        }).WithName("GetAllSRealty").WithOpenApi();
    }
}