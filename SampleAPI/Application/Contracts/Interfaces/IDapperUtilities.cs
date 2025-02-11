using Application.Contracts.Interfaces.Common;
using Application.Core;
using Application.DTOs.Common;
using System.Data;

namespace Application.Contracts.Interfaces;

public interface IDapperUtilities : IInjectableService
{
	Task<OperationResult<IEnumerable<TDto>>> GetQueriesAsync<TDto>(string query, object? param = null) where TDto : class;
	Task<OperationResult<TDto>> GetQueryAsync<TDto>(string query, object? param = null, bool isSingle = false) where TDto : class;
	Task<OperationResult<T>> GetStructQueryAsync<T>(string query, object? param = null) where T : struct;
	Task<OperationResult<IEnumerable<T>>> GetStructQueriesAsync<T>(string query, object? param = null) where T : struct;
	Task<OperationResult<IEnumerable<TModel>>> GetQueryWithRelatedAsync<TModel, TRelated, TKey>(string query, Func<TModel, TRelated, TModel> map, string splitOn) 
		where TModel : Entity<TKey>
		where TRelated : Entity<TKey>
		where TKey : struct;
	Task<ContextTransactionDto> ExecuteAsync(Func<IDbConnection, Task> action);
	Task<ContextTransactionDto> ExecuteAsync(string query, object? param = null);
};