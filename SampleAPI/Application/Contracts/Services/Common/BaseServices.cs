using Application.Contracts.Interfaces.Common;
using Application.DTOs.Common;
using Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Contracts.Services.Common;

public abstract class BaseServices<TEntity, TKey> : IBaseServices<TEntity, TKey>
    where TKey : struct
    where TEntity : Entity<TKey>
{
    #region ConstructorInjection

    protected readonly DataBaseContext _context;
    protected DbSet<TEntity> Entity { get; }
    protected virtual IQueryable<TEntity> Table => Entity;
    protected virtual IQueryable<TEntity> TableNoTracking => Entity.AsNoTracking();

    public BaseServices(DataBaseContext context)
    {
        _context = context;
        Entity = _context.Set<TEntity>();
    }

    #endregion

    public async Task<ContextTransactionDto<TKey>> AddAsync(TEntity entity, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default)
        => await ExecuteInTransactionAsync(async () => await Entity.AddAsync(entity, cancellationToken), withSaveChanges, configureAwait, cancellationToken);

    public async Task<ContextTransactionDto<TKey>> AddAsync(IEnumerable<TEntity> entities, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default)
        => await ExecuteInTransactionAsync(async () => await Entity.AddRangeAsync(entities, cancellationToken), withSaveChanges, configureAwait, cancellationToken);

    public async Task<ContextTransactionDto<TKey>> DeleteAsync(TEntity entity, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default)
        => await ExecuteInTransactionAsync(() => Entity.Remove(entity), withSaveChanges, configureAwait, cancellationToken);

    public async Task<ContextTransactionDto<TKey>> DeleteAsync(IEnumerable<TEntity> entities, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default)
        => await ExecuteInTransactionAsync(() => Entity.RemoveRange(entities), withSaveChanges, configureAwait, cancellationToken);

    public async Task<ContextTransactionDto<TKey>> DeleteAsync(Expression<Func<TEntity, bool>> expression, bool configureAwait = false, CancellationToken cancellationToken = default)
        => await ExecuteInTransactionAsync(async () => await Entity.Where(expression).ExecuteDeleteAsync(cancellationToken), withSaveChanges: false, configureAwait, cancellationToken);

    public async Task<int> SaveChangesAsync(bool configureAwait = false, CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(configureAwait);

    public int SaveChanges() => _context.SaveChanges();

    public async Task<ContextTransactionDto<TKey>> ExecuteInTransactionAsync(TransactionalDelegateAsync transactionalDelegate, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            await transactionalDelegate();

            if (withSaveChanges)
                await SaveChangesAsync(configureAwait, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Success);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            var message = (ex.InnerException is null) ? (ex.Message.HasPersianCharacter() ? ex.Message : null) : (ex.InnerException.Message.HasPersianCharacter() ? ex.InnerException.Message : null);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Concurrency, message);
        }
        catch (TimeoutException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            var message = (ex.InnerException is null) ? (ex.Message.HasPersianCharacter() ? ex.Message : null) : (ex.InnerException.Message.HasPersianCharacter() ? ex.InnerException.Message : null);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Timeout, message);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ExceptionToContextTransaction(ex);
        }
        catch (SqlException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Failed);
        }
    }

    public async Task<ContextTransactionDto<TKey>> ExecuteInTransactionAsync(TransactionalDelegate transactionalDelegate, bool withSaveChanges = true, bool configureAwait = false, CancellationToken cancellationToken = default)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            transactionalDelegate();

            if (withSaveChanges)
                await SaveChangesAsync(configureAwait, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Success);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            var message = (ex.InnerException is null) ? (ex.Message.HasPersianCharacter() ? ex.Message : null) : (ex.InnerException.Message.HasPersianCharacter() ? ex.InnerException.Message : null);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Concurrency, message);
        }
        catch (TimeoutException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            var message = (ex.InnerException is null) ? (ex.Message.HasPersianCharacter() ? ex.Message : null) : (ex.InnerException.Message.HasPersianCharacter() ? ex.InnerException.Message : null);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Timeout, message);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ExceptionToContextTransaction(ex);
        }
        catch (SqlException ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Failed);
        }
    }

    public void ChangeEntityState(TEntity entity, EntityState entityState)
        => _context.Entry(entity).State = entityState;

    private static ContextTransactionDto<TKey> ExceptionToContextTransaction(Exception ex, int count, string firstMessage)
    {
        var message = (ex.InnerException is null) ? ex.Message : ex.InnerException.Message;

        if (message.HasPersianCharacter())
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.ServerError, count > 1 ? firstMessage : message);

        if (message.Contains("DELETE statement conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("DELETE statement conflicted with the Reference", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.DeleteRelation);
        if (message.Contains("conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("conflicted with the Reference", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.ForeignKey);
        if (message.Contains("insert duplicate key", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.AlreadyExists);
        if (message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("timestamp", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("timestamp", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Concurrency);
        if (message.Contains("Cannot insert the value NULL", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.ServerError);
        if (message.Contains("Timeout", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Timeout);
        if (message.Contains("unique index", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.AlreadyExists);

        return new ContextTransactionDto<TKey>(ContextTransactionStatus.Failed);
    }

    private static ContextTransactionDto<TKey> ExceptionToContextTransaction(Exception ex)
    {
        var message = (ex.InnerException is null) ? ex.Message : ex.InnerException.Message;

        if (message.HasPersianCharacter())
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.ServerError, message);

        if (message.Contains("DELETE statement conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("DELETE statement conflicted with the Reference", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.DeleteRelation);
        if (message.Contains("conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("conflicted with the Reference", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.ForeignKey);
        if (message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("timestamp", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("timestamp", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Concurrency);
        if (message.Contains("Cannot insert the value NULL", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.ServerError);
        if (message.Contains("Timeout", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.Timeout);
        if (message.Contains("unique index", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto<TKey>(ContextTransactionStatus.AlreadyExists);

        return new ContextTransactionDto<TKey>(ContextTransactionStatus.Failed);
    }
}

public abstract class BaseServices<TEntity>(DataBaseContext context) 
    : BaseServices<TEntity, int>(context)
    where TEntity : Entity;