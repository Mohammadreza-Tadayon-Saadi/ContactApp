using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Common;

public record ImportDataDto(IFormFile ExcelFile);