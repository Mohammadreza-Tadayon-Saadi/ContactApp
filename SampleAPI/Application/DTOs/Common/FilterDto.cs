namespace Application.DTOs.Common;

public class FilterDto<TEntity, TKey>(IQueryable<TEntity> entities, MetaDataDto metaData)
	where TEntity : Entity<TKey>
	where TKey : struct
{
	public IQueryable<TEntity> Entities { get; set; } = entities;

	public MetaDataDto MetaData { get; init; } = metaData;
};

public class FilterDto<TEntity>(IQueryable<TEntity> Entities, MetaDataDto MetaData)
	: FilterDto<TEntity, int>(Entities, MetaData)
	where TEntity : Entity;