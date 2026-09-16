using System.Text;
<<<<<<< HEAD
using LogiTrack.Api;
=======

>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
using FluentValidation;

using LogiTrack.Application.Common.Behaviors;
using LogiTrack.Application.Notifications;
using LogiTrack.Infrastructure.Notifications;
using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Customers.Commands.CreateCustomer;

using LogiTrack.Infrastructure.Authentication;
using LogiTrack.Infrastructure.Identity;
using LogiTrack.Infrastructure.Persistence;
<<<<<<< HEAD

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
=======
using Asp.Versioning;
using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Scalar.AspNetCore;
<<<<<<< HEAD
using LogiTrack.Api.Hubs;
using DeliveryHub = LogiTrack.Api.Hubs.DeliveryHub;
=======
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Database
// =====================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LogiTrackDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<LogiTrackDbContext>());

// =====================================================
// ASP.NET Core Identity
// =====================================================

builder.Services
    .AddIdentityCore<LogiTrackUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LogiTrackDbContext>()
    .AddSignInManager();

// =====================================================
// JWT Authentication
// =====================================================

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is missing.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });

builder.Services.AddAuthorization();

<<<<<<< HEAD
// TmsApi-inspired API versioning and rate limiting
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("writes", limiter =>
    {
        limiter.PermitLimit = 30;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueLimit = 0;
    });
});
builder.Services.AddSingleton<IIdempotencyStore, InMemoryIdempotencyStore>();
builder.Services.AddProblemDetails();

=======
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
// =====================================================
// JWT Token Service
// =====================================================

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDeliveryNotificationService, SignalRDeliveryNotificationService>();

// =====================================================
// Controllers
// =====================================================

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogiTrack.Api.AuditLogFilter>();
});

// =====================================================
// MediatR
// =====================================================

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateCustomerCommand).Assembly);

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// =====================================================
// FluentValidation
// =====================================================

builder.Services.AddValidatorsFromAssembly(
    typeof(CreateCustomerCommand).Assembly);

// =====================================================
// OpenAPI / Scalar
// =====================================================

<<<<<<< HEAD
builder.Services.AddOpenApi();

=======
builder.Services.AddOpenApi("v1", options =>
{
    options.ShouldInclude = description =>
        description.GroupName == "v1";
});

builder.Services.AddOpenApi("v2", options =>
{
    options.ShouldInclude = description =>
        description.GroupName == "v2";
});
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
// =====================================================
// CORS
// =====================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// =====================================================
// Build Application
// =====================================================

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}
// =====================================================
// Development Tools
// =====================================================

if (app.Environment.IsDevelopment())
{
<<<<<<< HEAD
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("LogiTrack API")
            .WithTheme(ScalarTheme.Default);
    });
=======
   app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options
        .WithTitle("LogiTrack API")
        .WithTheme(ScalarTheme.Default)
        .AddDocument("v1", "API Version 1.0")
        .AddDocument("v2", "API Version 2.0");
});
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
}

// =====================================================
// Middleware Pipeline
// =====================================================

app.UseMiddleware<
    LogiTrack.Api.Middleware.ExceptionHandlingMiddleware>();

app.UseCors("Angular");
<<<<<<< HEAD
app.UseRateLimiter();
=======
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapHub<DeliveryHub>("/hubs/delivery").RequireCors("Angular");

// =====================================================
// Controllers
// =====================================================

app.MapControllers();

app.Run();

public partial class Program;