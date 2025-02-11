namespace Application.DTOs.Common;

public record QueryingSearchDto
{
	private string _searchField = string.Empty;
	public string SearchField
	{
		get => _searchField;
		set => _searchField = value.HasValue() ? value.ToCapitalLetter() : string.Empty;
	}

	public string SearchValue { get; set; } = string.Empty;
}