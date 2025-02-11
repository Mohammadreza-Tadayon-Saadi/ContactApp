namespace Application.DTOs.Common;

public record MetaDataDto
{
    public IEnumerable<QueryingSearchDto>? SearchData { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
    public int PerPage { get; set; }
    public int TotalRecord { get; set; }
}