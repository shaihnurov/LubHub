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
    /// <param name="ct">Cancellation token</param>
    /// <returns>Entity if found, otherwise null</returns>
    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await DbContext.Set<T>().FindAsync([id], ct);

    /// <summary>
    /// Gets all entities of type T
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Read-only list of all entities</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await DbContext.Set<T>().ToListAsync(ct);

    /// <summary>
    /// Adds a new entity to the database
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="ct">Cancellation token</param>
    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await DbContext.AddAsync(entity, ct);
        await DbContext.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Updates an existing entity in the database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="ct">Cancellation token</param>
    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        DbContext.Update(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Deletes an entity from the database
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="ct">Cancellation token</param>
    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        DbContext.Remove(entity);
        await DbContext.SaveChangesAsync(ct);
    }
}