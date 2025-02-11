namespace Application.DTOs.Common;

public record SortinationDto
{
	private string _type = "desc";
	private string _field = string.Empty;

	public string Field 
	{
		get => _field;
		set => _field = value.HasValue() ? value.ToCapitalLetter() : string.Empty;
	}
	public string Type
	{
		get => _type;
		set
		{
			if (value.HasValue()) 
			{ 
				var val = value.ToLower();
				if (val != "desc" && val != "asc")
					_type = "desc";

				else _type = val;
			}
		}
	}
}