using Application.DTOs.Common;

namespace Application.Contracts.Interfaces.Common;

public interface IAdminServices<TEntity, TKey, TDataTableDto, TAddOrEditDto>
        where TEntity : Entity<TKey>
        where TKey : struct
        where TDataTableDto : new()
        where TAddOrEditDto : new()
{
    Task<bool> IsExistsAsync(TKey id, bool? getActive = null, CancellationToken cancellationToken = default);
    
    Task<TEntity> GetByIdAsync(TKey id, bool withTracking = true, bool? getActive = null,
         CancellationToken cancellationToken = default);

    Task<KTDataTableResultDto<TDataTableDto>> GetKTDataTableInfoAsync(IEnumerable<QueryingSearchDto> queries, PaginationDto pagination, SortinationDto sortination, CancellationToken cancellationToken = default);
    
    Task<TAddOrEditDto> GetForAddOrEditAsync(TKey id, bool? getActive = null, CancellationToken cancellationToken = default);
};

public interface IAdminServices<TEntity, TDataTableDto, TAddOrEditDto> : IAdminServices<TEntity, int, TDataTableDto, TAddOrEditDto>
        where TEntity : Entity
        where TDataTableDto : new()
        where TAddOrEditDto : new();