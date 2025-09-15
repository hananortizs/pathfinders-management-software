using Microsoft.EntityFrameworkCore.Storage;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions and repositories
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly PmsDbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    /// <summary>
    /// Initializes a new instance of the UnitOfWork
    /// </summary>
    /// <param name="context">The database context</param>
    public UnitOfWork(PmsDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    /// <summary>
    /// Gets a repository for the specified entity type
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <returns>Repository instance</returns>
    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<T>(_context);
        }

        return (IRepository<T>)_repositories[type];
    }

    /// <summary>
    /// Saves all changes made in this unit of work
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the unit of work
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}
