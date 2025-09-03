using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pms.Backend.Api;
using Pms.Backend.Infrastructure.Data;
using System.Net.Http;
using Xunit;

namespace Pms.Backend.Tests.Integration;

/// <summary>
/// Classe base para testes de integração que configura o ambiente de teste
/// com banco de dados em memória e cliente HTTP para testes de API
/// </summary>
public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly PmsDbContext DbContext;
    protected readonly IServiceScope ServiceScope;

    protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
    {
        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove o DbContext real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PmsDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adiciona o DbContext em memória
                services.AddDbContext<PmsDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                });

                // Configura logging para testes
                services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
            });

            builder.UseEnvironment("Testing");
        });

        Client = Factory.CreateClient();
        ServiceScope = Factory.Services.CreateScope();
        DbContext = ServiceScope.ServiceProvider.GetRequiredService<PmsDbContext>();
        
        // Garante que o banco seja criado
        DbContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Limpa o banco de dados após cada teste
    /// </summary>
    protected virtual void CleanupDatabase()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Salva as alterações no banco de dados
    /// </summary>
    protected virtual async Task SaveChangesAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        ServiceScope?.Dispose();
        Client?.Dispose();
        GC.SuppressFinalize(this);
    }
}
