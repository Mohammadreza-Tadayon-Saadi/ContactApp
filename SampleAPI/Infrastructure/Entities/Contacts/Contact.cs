using Infrastructure.Entities.Common;
using Infrastructure.Entities.Groups;

namespace Infrastructure.Entities.Contacts;

public sealed class Contact : Entity
{
    public string FullName { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public int GroupId { get; set; }

    public Group GroupContact { get; set; }
}
