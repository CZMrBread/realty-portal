using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Server.Database;
using Server.Endpoints.SRealty;
using Server.Entities.Users;
using Server.Endpoints.User;
using Server.Entities.SRealtyRealty;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnectionString"]);
});

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddScoped<SRealtyHandlers>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy => 
        policy.RequireRole(ApplicationRole.SuperAdmin));
    options.AddPolicy("AdminOrAbove", policy => 
        policy.RequireRole(ApplicationRole.SuperAdmin, ApplicationRole.Admin));
    options.AddPolicy("RealtyAgencyAdminOrAbove", policy => 
        policy.RequireRole(ApplicationRole.SuperAdmin, ApplicationRole.Admin, ApplicationRole.RealtyAgencyAdmin));
});

builder.Services.AddScoped<JwtTokenGenerator>();

var app = builder.Build();
using (var scope = app.Services.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}
using (var scope = app.Services.CreateScope()) {
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    foreach (var role in ApplicationRole.AllRoles) {
        if (await roleManager.RoleExistsAsync(role))
            continue;
        await roleManager.CreateAsync(new ApplicationRole { Name = role });
    }
}


app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var apiGroup = app.MapGroup("api");
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

apiGroup.MapUserEndpoints();
apiGroup.MapSRealtyEndpoints();
app.Run();