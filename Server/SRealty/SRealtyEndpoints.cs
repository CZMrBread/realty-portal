using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Server.SRealty;

public static class SRealtyEndpoints
{
    public static void MapSRealtyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/srealty").WithTags("SRealty");

        group.MapPost("create", CreateSRealtyAdvert)
            .WithName(nameof(CreateSRealtyAdvert))
            .AddOpenApiOperationTransformer((operation, context, ct) => { return Task.CompletedTask; });
        group.MapPut("update/{id:Guid}", UpdateSRealtyAdvert)
            .WithName(nameof(UpdateSRealtyAdvert))
            .AddOpenApiOperationTransformer((operation, context, ct) => { return Task.CompletedTask; });
        group.MapPut("update/rkid/{rkid}", UpdateSRealtyAdvertByRkId)
            .WithName(nameof(UpdateSRealtyAdvertByRkId))
            .AddOpenApiOperationTransformer((operation, context, ct) => { return Task.CompletedTask; });
        group.MapGet("/get/{id:Guid}", GetSRealtyAdvert)
            .WithName(nameof(GetSRealtyAdvert))
            .AddOpenApiOperationTransformer((operation, context, ct) => { return Task.CompletedTask; });
        group.MapGet("/get/rkid/{rkId}", GetSRealtyAdvertByRkId)
            .WithName(nameof(GetSRealtyAdvertByRkId))
            .AddOpenApiOperationTransformer((operation, context, ct) => { return Task.CompletedTask; });
        group.MapGet("/get/all", GetAllSRealtyAdverts)
            .WithName(nameof(GetAllSRealtyAdverts))
            .AddOpenApiOperationTransformer((operation, context, ct) => { return Task.CompletedTask; });
    }

    internal static async Task<IResult> GetSRealtyAdvert(Guid id)
    {
        return TypedResults.Ok();
    }

    internal static async Task<IResult> GetSRealtyAdvertByRkId(string rkId)
    {
        return TypedResults.Ok();
    }

    internal static async Task<IResult> CreateSRealtyAdvert()
    {
        return TypedResults.Ok();
    }

    internal static async Task<IResult> UpdateSRealtyAdvert(Guid id)
    {
        return TypedResults.Ok();
    }

    internal static async Task<IResult> UpdateSRealtyAdvertByRkId(string rkId)
    {
        return TypedResults.Ok();
    }

    internal static async Task<IResult> GetAllSRealtyAdverts([FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 50)] int pageSize = 20)
    {
        return TypedResults.Ok();
    }
}