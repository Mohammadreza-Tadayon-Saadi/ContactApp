using Application.DTOs.Common;
using SharpCompress;

namespace Application.Common;

public static class IQuerableExtentions
{
	public static FilterDto<TEntity, TKey> ApplyFilter<TEntity, TKey>(this IQueryable<TEntity> entities,
		IEnumerable<QueryingSearchDto> queries, PaginationDto pagination, SortinationDto sortination, bool applyConditions = true)
		where TEntity : Entity<TKey>
		where TKey : struct
	{
		var result = entities;

		if(applyConditions)
		{
            try
            {
                if (queries is not null && queries.Any())
                    queries.ForEach(query =>
                    {
                        if (query.SearchValue.HasValue())
                            result = result.WhereProperty(query.SearchField, query.SearchValue, true);
                    });
                else
                    queries = new List<QueryingSearchDto>();
            }
            catch (Exception) { }
        }
        
		var totalRecord = result.Count();
		var pageCount = totalRecord / pagination.ItemsPerPage;
		if (totalRecord % pagination.ItemsPerPage != 0) pageCount++;


		var metaData = new MetaDataDto
		{
			SearchData = queries,
			CurrentPage = pagination.CurrentPage,
			TotalPage = pageCount,
			PerPage = pagination.ItemsPerPage,
			TotalRecord = totalRecord,
		};

		if (totalRecord > 1 && sortination.Field.HasValue())
		{
			var orderByResult = result.OrderByProperty(sortination.Field, sortination.Type == "desc");
			if (orderByResult.IsSuccess)
				result = orderByResult.Value;
			else result = result.OrderByProperty("Id", sortination.Type == "desc").Value;
		}
		else result = result.OrderByDescending(u => u.Id);

		return new FilterDto<TEntity, TKey>(result, metaData);
	}

	public static FilterDto<TEntity, int> ApplyFilter<TEntity>(this IQueryable<TEntity> entities,
		IEnumerable<QueryingSearchDto> queries, PaginationDto pagination, SortinationDto sortination, bool applyConditions = true)
		where TEntity : Entity
			=> ApplyFilter<TEntity, int>(entities, queries, pagination, sortination, applyConditions);
}