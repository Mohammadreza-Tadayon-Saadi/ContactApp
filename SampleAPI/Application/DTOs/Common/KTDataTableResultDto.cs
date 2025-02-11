namespace Application.DTOs.Common;

public record class KTDataTableResultDto<TData>(IEnumerable<TData> Data, MetaDataDto MetaData);