using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Infrastructure.Data;
using Pms.Backend.Api.Configuration;
using Pms.Backend.Api.Infrastructure;
using Pms.Backend.Api.Extensions;
using Pms.Backend.Api.Filters;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Services.Seed;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;

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
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure API settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

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
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Case insensitive property matching
        options.JsonSerializerOptions.AllowTrailingCommas = true; // Permitir vírgulas finais
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip; // Pular comentários
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PMS Backend API",
        Version = "v1",
        Description = "API para gerenciamento de membros e hierarquia da Igreja Adventista"
    });

    // Configurar autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add database context
builder.Services.AddPmsDbContext(builder.Configuration);

// Add application services
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IUnitOfWork, Pms.Backend.Infrastructure.Repositories.UnitOfWork>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IHierarchyService, Pms.Backend.Application.Services.Hierarchy.HierarchyService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IHierarchyQueryService, Pms.Backend.Application.Services.Hierarchy.HierarchyQueryService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IMemberService, Pms.Backend.Application.Services.Members.MemberService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IMemberValidationService, Pms.Backend.Application.Services.Members.MemberValidationService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IMembershipService, Pms.Backend.Application.Services.Membership.MembershipService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IAssignmentService, Pms.Backend.Application.Services.Assignments.AssignmentService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IApprovalDelegateService, Pms.Backend.Application.Services.Assignments.ApprovalDelegateService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IRoleCatalogService, Pms.Backend.Application.Services.Assignments.RoleCatalogService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IExportService, Pms.Backend.Application.Services.ExportService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IAddressService, Pms.Backend.Application.Services.Address.AddressService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IAddressValidationService, Pms.Backend.Infrastructure.Services.AddressValidationService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IContactValidationService, Pms.Backend.Infrastructure.Services.ContactValidationService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IContactService, Pms.Backend.Infrastructure.Services.ContactService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IAllocationService, Pms.Backend.Application.Services.Membership.AllocationService>();
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.ILoggingService, Pms.Backend.Infrastructure.Services.LoggingService>();

// Registrar validadores FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Pms.Backend.Application.Validators.ApprovalDelegate.CreateApprovalDelegateDtoValidator>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.HierarchyMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.MemberMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.MembershipMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.AddressMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.AssignmentMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.ContactMappingProfile));
builder.Services.AddAutoMapper(typeof(Pms.Backend.Application.Mappings.AllocationMappingProfile));

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add Hierarchy Code Service
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IHierarchyCodeService, Pms.Backend.Application.Services.Hierarchy.HierarchyCodeService>();

// Add Seed Service
builder.Services.AddScoped<ISeedService, SeedService>();

// Add Health Service
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IHealthService, Pms.Backend.Application.Services.Health.HealthService>();

// Add Reports Service
builder.Services.AddScoped<Pms.Backend.Application.Interfaces.IReportsService, Pms.Backend.Application.Services.Reports.ReportsService>();

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PMS Backend API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "PMS Backend API";
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();
    });
}

app.UseHttpsRedirection();

// Add exception handling middleware (should be early in pipeline)
app.UseExceptionHandling();

app.UseCors("DefaultPolicy");

// Add response standardization middleware
app.UseResponseStandardization();

// Add JWT Authentication middleware
app.UseJwtAuthentication();

// Rate limiting will be implemented in future MVP
// app.UseRateLimiter();

app.UseAuthentication();
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

        // Execute seed data
        var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
        var seedResult = await seedService.SeedAllAsync();
        if (seedResult.IsSuccess)
        {
            Log.Information("Seed data executed successfully: {Message}", seedResult.Message);
        }
        else
        {
            Log.Warning("Seed data execution failed: {Message}", seedResult.Message);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while connecting to the database or executing seed data");
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
