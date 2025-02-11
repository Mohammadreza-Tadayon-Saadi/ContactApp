using Application.DTOs.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Application.Contracts.Interfaces.Common;

public delegate Task TransactionalDelegateAsync();
public delegate void TransactionalDelegate();

public interface IBaseServices<TEntity> : IBaseServices<TEntity, int> where TEntity : Entity { }
public interface IBaseServices<TEntity, TKey> : IInjectableService
        where TEntity : Entity<TKey>
        where TKey : struct
{
    /// <summary>
    /// Add One Entities In DataBase
    /// </summary>
    Task<ContextTransactionDto<TKey>> AddAsync(TEntity entity, bool withSaveChanges = true,
        bool configureAwait = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add Multiple Entities In DataBase
    /// </summary>
    Task<ContextTransactionDto<TKey>> AddAsync(IEnumerable<TEntity> entities, bool withSaveChanges = true,
        bool configureAwait = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete Entity From DataBase
    /// </summary>
    Task<ContextTransactionDto<TKey>> DeleteAsync(TEntity entity, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete Multiple Entities From DataBase
    /// </summary>
    Task<ContextTransactionDto<TKey>> DeleteAsync(IEnumerable<TEntity> entities, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete Entity From DataBase (Bulk Operation).The operation will causes a single SQL DELETE
    /// </summary>
    Task<ContextTransactionDto<TKey>> DeleteAsync(Expression<Func<TEntity, bool>> expression, bool configureAwait = false, CancellationToken cancellationToken = default);
    
    Task<ContextTransactionDto<TKey>> ExecuteInTransactionAsync(TransactionalDelegateAsync transactionalDelegate, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default);
    
    Task<ContextTransactionDto<TKey>> ExecuteInTransactionAsync(TransactionalDelegate transactionalDelegate, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(bool configureAwait = false, CancellationToken cancellationToken = default);
    
    int SaveChanges();
    
    void ChangeEntityState(TEntity entity, EntityState entityState);
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class ServiceLifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}

public interface IInjectableService;