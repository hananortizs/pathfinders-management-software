using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Infrastructure.Data;
using Pms.Backend.Api.Configuration;
using Pms.Backend.Api.Infrastructure;
using Pms.Backend.Api.Extensions;
using Pms.Backend.Api.Filters;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pms.Backend.Api;

/// <summary>
/// Entry point for the PMS Backend API application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure API settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add services to the container
builder.Services.AddControllers(options =>
{
    // Add kebab-case routing convention
    options.Conventions.Add(new KebabCaseRoutingConvention());

    // Add global route prefix convention
    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
    if (!string.IsNullOrEmpty(apiSettings?.RoutePrefix))
    {
        options.Conventions.Add(new GlobalRoutePrefixConvention(apiSettings.RoutePrefix));
    }

    // Add global ModelState validation filter
    options.Filters.Add<ModelValidationFilter>();
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Handle circular references
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore null values
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddPmsDbContext(builder.Configuration);

// Add application services
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IUnitOfWork, Pms.Backend.Infrastructure.Repositories.UnitOfWork>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IHierarchyService, Pms.Backend.Application.Services.Hierarchy.HierarchyService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IHierarchyQueryService, Pms.Backend.Application.Services.Hierarchy.HierarchyQueryService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IMemberService, Pms.Backend.Application.Services.Members.MemberService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IMembershipService, Pms.Backend.Application.Services.Membership.MembershipService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IAssignmentService, Pms.Backend.Application.Services.Assignments.AssignmentService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IApprovalDelegateService, Pms.Backend.Application.Services.Assignments.ApprovalDelegateService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IExportService, Pms.Backend.Application.Services.ExportService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.HierarchyMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.MemberMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.MembershipMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.AssignmentMappingProfile));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" };
        var allowedMethods = builder.Configuration.GetSection("Cors:AllowedMethods").Get<string[]>() ?? new[] { "GET", "POST", "PUT", "DELETE" };
        var allowedHeaders = builder.Configuration.GetSection("Cors:AllowedHeaders").Get<string[]>() ?? new[] { "*" };

        policy.WithOrigins(allowedOrigins)
              .WithMethods(allowedMethods)
              .WithHeaders(allowedHeaders)
              .AllowCredentials();
    });
});

// Rate limiting will be implemented in future MVP
// builder.Services.AddRateLimiter(options => { ... });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");

// Add response standardization middleware
app.UseResponseStandardization();

// Rate limiting will be implemented in future MVP
// app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PmsDbContext>();
    try
    {
        context.Database.EnsureCreated();
        Log.Information("Database connection successful");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while connecting to the database");
        throw;
    }
}

Log.Information("PMS Backend API started successfully");
Log.Information("🚀 API is running at: http://localhost:5000");
Log.Information("📚 Swagger UI available at: http://localhost:5000/swagger");
Log.Information("🔧 Environment: {Environment}", app.Environment.EnvironmentName);
Log.Information("💾 Database: PostgreSQL (Docker)");

app.Run();
    }
}
