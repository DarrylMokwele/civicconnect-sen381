using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Application.Abstractions;
using Application.Notifications;
using Infrastructure.Notifications;
using Application.ServiceRequests;
using Infrastructure.ServiceRequests;
using Application.Locations;
using Infrastructure.Location;
using WebApi.Health;
using WebApi.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CivicConnectDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CivicConnectDatabase")));

builder.Services.AddScoped<
    INotificationService,
    NotificationService>();

builder.Services.AddScoped<
    ServiceRequestStatusChangedHandler>();

builder.Services.AddScoped<
    IStatusTransitionPolicy,
    StatusTransitionPolicy>();

builder.Services.AddScoped<
    IServiceRequestLifecycleService,
    ServiceRequestLifecycleService>();

builder.Services.AddScoped<
    ILocationResolver,
    ExternalLocationResolver>();

// Operational health checks.
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "live" })
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" });

// Authentication is used by the administrator console.
// The demo credentials are configuration-driven; use a secret store/environment variables for real deployment.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "CivicConnect.Admin";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Database lifecycle is configuration-driven so production does not
// automatically mutate schema or seed development data.
var applyMigrations = builder.Configuration.GetValue("Database:ApplyMigrations", false);
var seedData = builder.Configuration.GetValue("Database:SeedData", false);

if (applyMigrations || seedData)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CivicConnectDbContext>();

    if (applyMigrations)
    {
        await dbContext.Database.MigrateAsync();
    }

    if (seedData)
    {
        await DbInitializer.SeedAsync(dbContext);
    }
}

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();

app.Map("/error", (HttpContext context, ILogger<Program> logger) =>
{
    logger.LogError("Unhandled exception. CorrelationId={CorrelationId}", context.TraceIdentifier);
    return Results.Problem(
        statusCode: StatusCodes.Status500InternalServerError,
        title: "An unexpected error occurred.",
        detail: "The request could not be completed. Use the correlation ID when reporting the problem.");
});

// Liveness proves the process is running. Readiness includes the database.
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();


// Expose the entry point to the HTTP integration test host.
public partial class Program { }
