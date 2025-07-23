namespace Server.Database.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/test/{id}", GetTestAsync)
            .WithName(nameof(GetTestAsync))
            .WithOpenApi();
    }

    public static async Task<IResult> GetTestAsync(int id)
    {
        Console.WriteLine(id);
        return Results.Ok("Hello, World!");
    }
}