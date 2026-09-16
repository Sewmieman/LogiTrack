using System.Text;

using Asp.Versioning;

using FluentValidation;

using LogiTrack.Api;
using LogiTrack.Api.Hubs;
using LogiTrack.Application.Common.Behaviors;
using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Customers.Commands.CreateCustomer;
using LogiTrack.Application.Notifications;
using LogiTrack.Infrastructure.Authentication;
using LogiTrack.Infrastructure.Identity;
using LogiTrack.Infrastructure.Notifications;
using LogiTrack.Infrastructure.Persistence;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Scalar.AspNetCore;


// ============================================================
// CREATE BUILDER
// ============================================================

var builder = WebApplication.CreateBuilder(args);


// ============================================================
// DATABASE
// ============================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "DefaultConnection is missing from appsettings.json.");
}

builder.Services.AddDbContext<LogiTrackDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IApplicationDbContext>(
    provider =>
        provider.GetRequiredService<LogiTrackDbContext>());


// ============================================================
// ASP.NET CORE IDENTITY
// ============================================================

builder.Services
    .AddIdentityCore<LogiTrackUser>(options =>
    {
        // User settings
        options.User.RequireUniqueEmail = true;

        // Password settings
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;

        // Lockout settings
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(15);

        options.Lockout.AllowedForNewUsers = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LogiTrackDbContext>()
    .AddSignInManager();


// ============================================================
// JWT AUTHENTICATION
// ============================================================

var jwtKey =
    builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "Jwt:Key is missing from appsettings.json.");
}

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
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();


// ============================================================
// OPENAPI / SCALAR
// ============================================================

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion =
            new ApiVersion(1, 0);

        options.AssumeDefaultVersionWhenUnspecified =
            true;

        options.ReportApiVersions =
            true;

        options.ApiVersionReader =
            new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat =
            "'v'VVV";

        options.SubstituteApiVersionInUrl =
            true;
    })
    .AddOpenApi();


// ============================================================
// RATE LIMITING
// ============================================================

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode =
        StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter(
        "writes",
        limiter =>
        {
            limiter.PermitLimit = 30;

            limiter.Window =
                TimeSpan.FromMinutes(1);

            limiter.QueueLimit = 0;
        });
});


// ============================================================
// APPLICATION SERVICES
// ============================================================

builder.Services.AddSingleton<IIdempotencyStore,
    InMemoryIdempotencyStore>();

builder.Services.AddProblemDetails();


// ============================================================
// JWT TOKEN SERVICE
// ============================================================

builder.Services.AddScoped<IJwtTokenService,
    JwtTokenService>();


// ============================================================
// SIGNALR
// ============================================================

builder.Services.AddSignalR();

builder.Services.AddSingleton<
    IDeliveryNotificationService,
    SignalRDeliveryNotificationService>();


// ============================================================
// CONTROLLERS
// ============================================================

builder.Services.AddControllers(options =>
{
    options.Filters.Add<
        LogiTrack.Api.AuditLogFilter>();
});


// ============================================================
// MEDIATR
// ============================================================

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateCustomerCommand).Assembly);

    cfg.AddOpenBehavior(
        typeof(LoggingBehavior<,>));

    cfg.AddOpenBehavior(
        typeof(ValidationBehavior<,>));
});


// ============================================================
// FLUENT VALIDATION
// ============================================================

builder.Services.AddValidatorsFromAssembly(
    typeof(CreateCustomerCommand).Assembly);


// ============================================================
// OPENAPI
// ============================================================
//
// Do NOT use:
//     builder.Services.AddOpenApi("v1");
//     builder.Services.AddOpenApi("v2");
//
// when using Asp.Versioning.OpenApi.
//
// AddOpenApi() is registered through the API-versioning
// integration above.
//

// ============================================================
// CORS
// ============================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Angular",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


// ============================================================
// BUILD APPLICATION
// ============================================================

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext =
            services.GetRequiredService<LogiTrackDbContext>();

        await dbContext.Database.MigrateAsync();

        await IdentitySeeder.SeedAsync(
            services);
    }
    catch (Exception ex)
    {
        var logger =
            services.GetRequiredService<
                ILogger<Program>>();

        logger.LogError(
            ex,
            "An error occurred while migrating/seeding the database.");

        throw;
    }
}

   if (app.Environment.IsDevelopment())
{
    app.MapOpenApi()
       .WithDocumentPerVersion();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("LogiTrack API")
            .WithTheme(ScalarTheme.Default);
    });
}


// ============================================================
// ERROR HANDLING
// ============================================================

app.UseMiddleware<
    LogiTrack.Api.Middleware.ExceptionHandlingMiddleware>();


app.UseCors("Angular");

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapHub<LogiTrack.Api.Hubs.DeliveryHub>(
    "/hubs/delivery");
app.MapControllers();
app.Run();


// ============================================================
// REQUIRED FOR TEST PROJECTS
// ============================================================

public partial class Program;