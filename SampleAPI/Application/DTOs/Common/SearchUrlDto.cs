namespace Application.DTOs.Common;

public record SearchUrlDto
{
    public string EnTitle { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}