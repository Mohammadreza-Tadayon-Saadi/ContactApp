using Application.Contracts.Interfaces.Common;
using Application.Core;
using Application.Core.Exceptions;
using Application.DTOs.Common;
using Dapper;
using Infrastructure.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Application.Contracts.Services;

[ServiceLifetime(ServiceLifetime.Transient)]
public class DapperUtilities(IConfiguration _configuration) : IDapperUtilities
{
    private IDbConnection CreateConnection()
        => new SqlConnection(_configuration.GetConnectionString(ConnectionStrings.ContactsConnection));

    public async Task<OperationResult<IEnumerable<TDto>>> GetQueriesAsync<TDto>(string query, object? param = null) where TDto : class
    {
        using var connection = CreateConnection();
        try
        {
            var result = await connection.QueryAsync<TDto>(query, param);
            return OperationResult.Success(result);
        }
        catch (TimeoutException)
        {
            return OperationResult.Failure<IEnumerable<TDto>>(DomainErrors.Entity.Timeout());
        }
        catch (SqlException ex)
        {
            var message = ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
            return OperationResult.Failure<IEnumerable<TDto>>(new Error("Exception", message.Message));
        }
        catch (Exception)
        {
            return OperationResult.Failure<IEnumerable<TDto>>(DomainErrors.Entity.Unknown());
        }
    }

    public async Task<OperationResult<TDto>> GetQueryAsync<TDto>(string query, object? param = null, bool isSingle = false) where TDto : class
    {
        using var connection = CreateConnection();
        try
        {
            var result = await connection.QueryAsync<TDto>(query, param);
            return isSingle ? result.Single() : result.SingleOrDefault();
        }
        catch (TimeoutException)
        {
            return OperationResult.Failure<TDto>(DomainErrors.Entity.Timeout());
        }
        catch (SqlException ex)
        {
            var message = ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
            return OperationResult.Failure<TDto>(new Error("Exception", message.Message));
        }
        catch (Exception)
        {
            return OperationResult.Failure<TDto>(DomainErrors.Entity.Unknown());
        }
    }

    public async Task<OperationResult<T>> GetStructQueryAsync<T>(string query, object? param = null) where T : struct
    {
        using var connection = CreateConnection();
        try
        {
            var result = (await connection.QueryAsync<T>(query, param)).Single();
            return result;
        }
        catch (TimeoutException)
        {
            return OperationResult.Failure<T>(DomainErrors.Entity.Timeout());
        }
        catch (SqlException ex)
        {
            var message = ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
            return OperationResult.Failure<T>(new Error("Exception", message.Message));
        }
        catch (Exception)
        {
            return OperationResult.Failure<T>(DomainErrors.Entity.Unknown());
        }
    }

    public async Task<OperationResult<IEnumerable<T>>> GetStructQueriesAsync<T>(string query, object? param = null) where T : struct
    {
        using var connection = CreateConnection();
        try
        {
            var result = (await connection.QueryAsync<T>(query, param));
            return OperationResult.Success(result);
        }
        catch (TimeoutException)
        {
            return OperationResult.Failure<IEnumerable<T>>(DomainErrors.Entity.Timeout());
        }
        catch (SqlException ex)
        {
            var message = ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
            return OperationResult.Failure<IEnumerable<T>>(new Error("Exception", message.Message));
        }
        catch (Exception)
        {
            return OperationResult.Failure<IEnumerable<T>>(DomainErrors.Entity.Unknown());
        }
    }

    public async Task<OperationResult<IEnumerable<TModel>>> GetQueryWithRelatedAsync<TModel, TRelated, TKey>(string query, Func<TModel, TRelated, TModel> map, string splitOn)
        where TModel : Entity<TKey>
        where TRelated : Entity<TKey>
        where TKey : struct
    {
        using var connection = CreateConnection();
        try
        {
            var result = await connection.QueryAsync(query, map, splitOn: splitOn);
            return OperationResult.Success(result);
        }
        catch (TimeoutException)
        {
            return OperationResult.Failure<IEnumerable<TModel>>(DomainErrors.Entity.Timeout());
        }
        catch (SqlException ex)
        {
            var message = ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
            return OperationResult.Failure<IEnumerable<TModel>>(new Error("Exception", message.Message));
        }
        catch (Exception)
        {
            return OperationResult.Failure<IEnumerable<TModel>>(DomainErrors.Entity.Unknown());
        }
    }

    public async Task<ContextTransactionDto> ExecuteAsync(Func<IDbConnection, Task> action)
    {
        using var connection = CreateConnection();
        try
        {
            await action(connection);
            return new ContextTransactionDto(ContextTransactionStatus.Success);
        }
        catch (TimeoutException)
        {
            return new ContextTransactionDto(ContextTransactionStatus.Timeout);
        }
        catch (SqlException ex)
        {
            return ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
        }
        catch (Exception)
        {
            return new ContextTransactionDto(ContextTransactionStatus.Failed);
        }
    }

    public async Task<ContextTransactionDto> ExecuteAsync(string query, object? param = null)
    {
        using var connection = CreateConnection();
        try
        {
            await connection.ExecuteAsync(query, param);
            return new ContextTransactionDto(ContextTransactionStatus.Success);
        }
        catch (TimeoutException)
        {
            return new ContextTransactionDto(ContextTransactionStatus.Timeout);
        }
        catch (SqlException ex)
        {
            return ExceptionToContextTransaction(ex, ex.Errors.Count, ex.Errors[0].Message);
        }
        catch (Exception)
        {
            return new ContextTransactionDto(ContextTransactionStatus.Failed);
        }
    }

    private static ContextTransactionDto ExceptionToContextTransaction(Exception ex, int count, string firstMessage)
    {
        var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;

        if (message.HasPersianCharacter())
            return new ContextTransactionDto(ContextTransactionStatus.ServerError, count > 1 ? firstMessage : message);

        if (message.Contains("DELETE statement conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("DELETE statement conflicted with the Reference", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.DeleteRelation);
        if (message.Contains("conflicted with the FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("conflicted with the Reference", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.ForeignKey);
        if (message.Contains("insert duplicate key", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.AlreadyExists);
        if (message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("timestamp", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("timestamp", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.Concurrency);
        if (message.Contains("Cannot insert the value NULL", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.ServerError);
        if (message.Contains("Timeout", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.Timeout);
        if (message.Contains("unique index", StringComparison.OrdinalIgnoreCase))
            return new ContextTransactionDto(ContextTransactionStatus.AlreadyExists);

        return new ContextTransactionDto(ContextTransactionStatus.Failed);
    }
}