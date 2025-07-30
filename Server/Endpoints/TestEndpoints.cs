using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Database.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/test", GetTestAsync)
            .WithName(nameof(GetTestAsync))
            .WithOpenApi();
        
        app.MapGet("/secure", GetSecureAsync)
            .RequireAuthorization()
            .WithName(nameof(GetSecureAsync))
            .WithOpenApi();
        
        app.MapGet("/login", GetLoginAsync)
            .WithName(nameof(GetLoginAsync))
            .WithOpenApi();
    }

    public static async Task<IResult> GetTestAsync()
    {
        
        return Results.Ok("Hello, World!");
    }
    public static async Task<IResult> GetSecureAsync()
    {
        return Results.Ok("Secure endpoint accessed!");
    }
    public static async Task<IResult> GetLoginAsync()
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "http://localhost:5094/",
            Audience = "http://localhost:5094/",
            Expires = DateTime.UtcNow.AddHours(1),
             SigningCredentials = 
                new SigningCredentials(
                    new SymmetricSecurityKey("a9ee59cf6f8bbd8bbe2485119bdb7b7334db052ac99a1a69a04c0efdd892fece"u8.ToArray()),
                    SecurityAlgorithms.HmacSha512Signature),
        };
        var tokeHandler = new JwtSecurityTokenHandler();
        var token = tokeHandler.CreateToken(tokenDescriptor);
        return Results.Ok(
            new
            {
                token = tokeHandler.WriteToken(token),
                expiration = token.ValidTo
            });
    }
}