using Application.DTOs.Common;
using Infrastructure.Entities.Groups;

namespace Application.DTOs.Groups;

public class GroupDataDto : BaseDto<GroupDataDto, Group>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
