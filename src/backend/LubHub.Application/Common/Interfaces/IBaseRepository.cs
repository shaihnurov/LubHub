namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Generic base repository interface for common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Gets an entity by its ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Entity if found, otherwise null</returns>
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Read-only list of all entities</returns>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="ct">Cancellation token</param>
    Task AddAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="ct">Cancellation token</param>
    Task UpdateAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="ct">Cancellation token</param>
    Task DeleteAsync(T entity, CancellationToken ct = default);
}