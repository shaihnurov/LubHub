using LubHub.Application.Common.Interfaces;
using LubHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LubHub.Persistence.Repositories;

/// <summary>
/// Generic base repository providing common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class BaseRepository<T>(AppDbContext dbContext) : IBaseRepository<T> where T : class
{
    /// <summary>
    /// Database context instance accessible to derived repositories
    /// </summary>
    protected readonly AppDbContext DbContext = dbContext;

    /// <summary>
    /// Gets an entity by its ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity if found, otherwise null</returns>
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await DbContext.Set<T>().FindAsync([id], cancellationToken);

    /// <summary>
    /// Gets all entities of type T
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Read-only list of all entities</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbContext.Set<T>().ToListAsync(cancellationToken);

    /// <summary>
    /// Adds a new entity to the database
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing entity in the database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes an entity from the database
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}