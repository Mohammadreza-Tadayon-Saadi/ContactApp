using Application.DTOs.Common;
using Infrastructure.Entities.Contacts;

namespace Application.DTOs.Contacts;

public class ContactDataDto : BaseDto<ContactDataDto, Contact>
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public int Group { get; set; }

    public override void CustomMappings(IMappingExpression<Contact, ContactDataDto> mapping)
    {
        mapping.ForMember(dto => dto.Group, opt => opt.MapFrom(c => c.GroupId));
    }
}
