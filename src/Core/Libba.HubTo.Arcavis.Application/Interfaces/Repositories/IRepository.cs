using Libba.HubTo.Arcavis.Domain.Entities;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Interfaces.Repositories;

/// <summary>
/// Defines the contract for a generic repository for entities.
/// </summary>
/// <typeparam name="T">The type of the entity, which must be a class derived from BaseEntity.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Gets an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities matching the given predicate.
    /// </summary>
    /// <param name="predicate">The filter condition.</param>
    /// <returns>A collection of entities that satisfy the condition.</returns>
    Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity that matches the given predicate, or null if none found.
    /// </summary>
    /// <param name="predicate">The filter condition.</param>
    /// <returns>The first matching entity or null.</returns>
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an entity with the given unique identifier exists. This is more efficient than GetByIdAsync for existence checks.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>True if the entity exists, otherwise false.</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if any entity matching the given predicate exists. This is more efficient than GetFirstOrDefaultAsync for existence checks.
    /// </summary>
    /// <param name="predicate">The filter condition.</param>
    /// <returns>True if any entity satisfies the condition, otherwise false.</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the total number of entities.
    /// </summary>
    /// <returns>The total count of entities.</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of entities matching the given predicate.
    /// </summary>
    /// <param name="predicate">The filter condition.</param>
    /// <returns>The count of entities that satisfy the condition.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a range of new entities.
    /// </summary>
    /// <param name="entities">The list of entities to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddRangeAsync(IList<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an existing entity as modified. 
    /// The actual update happens when SaveChangesAsync is called on the DbContext.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Marks an existing entity for deletion. 
    /// The actual deletion happens when SaveChangesAsync is called on the DbContext.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Persists all changes made in the repository to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveAsync(CancellationToken cancellationToken = default);
}
