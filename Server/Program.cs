using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgent;
using Server.Features.Ruian;
using Server.Features.SRealty;
using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Photo;
using Server.Features.User;
using Server.Infrastructure.Authentication;
using Server.Infrastructure.Database;
using Shared.RealtyAgent;
using Shared.User;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.AddNpgsqlDbContext<AppDbContext>("sqldata");

// the only cache in the application: whole responses, in the Redis instance Aspire registers as "cache"
builder.Services.AddOutputCache();
builder.AddRedisOutputCache("cache");

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddRoles<ApplicationRole>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            IssuerSigningKey = JwtSigningKey.Create(builder.Configuration),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(UserPolicies.SuperAdminOnly, policy =>
        policy.RequireRole(UserRoles.SuperAdmin))
    // the token says whether the caller may act as an agent; which agency they act for is read from the database
    .AddPolicy(AgentPolicies.AgentOnly, policy =>
        policy.RequireAssertion(context => context.User.GetAgentRole() is not null))
    .AddPolicy(AgentPolicies.AgencyAdminOnly, policy =>
        policy.RequireAssertion(context => context.User.GetAgentRole() == AgentRoleEnum.AgencyAdmin));

builder.Services.AddScoped<AccessTokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RealtyAgencyService>();
builder.Services.AddScoped<RealtyAgentService>();
builder.Services.AddScoped<RuianService>();
builder.Services.AddOptions<RuianOptions>()
    .BindConfiguration(RuianOptions.SectionName);
builder.Services.AddHttpClient<RuianAddressDownloader>(client => client.Timeout = TimeSpan.FromMinutes(10));
builder.Services.AddScoped<RuianAddressImporter>();
builder.Services.AddScoped<AdvertService>();
builder.Services.AddOptions<PhotoStorageOptions>()
    .BindConfiguration(PhotoStorageOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<IPhotoStorage, FilePhotoStorage>();
builder.Services.AddScoped<PhotoService>();

var app = builder.Build();
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

// the register sync copies through PostgreSQL; the tests run on SQLite and do not need it
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.IsNpgsql())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        await RuianImporter.ImportAsync(dbContext, logger);
    }
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    foreach (var role in UserRoles.All)
    {
        if (await roleManager.RoleExistsAsync(role))
            continue;
        await roleManager.CreateAsync(new ApplicationRole { Name = role });
    }
}


app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

var apiGroup = app.MapGroup("api");
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


apiGroup.MapUserEndpoints();
apiGroup.MapRealtyAgencyEndpoints();
apiGroup.MapRealtyAgentEndpoints();
apiGroup.MapRuianEndpoints();
apiGroup.MapSRealtyEndpoints();
app.Run();

namespace Server
{
    /// <summary>Entry point, made visible for the integration tests.</summary>
    public partial class Program;
}
