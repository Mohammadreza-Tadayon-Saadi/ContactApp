using Infrastructure.Entities.Common;
using Infrastructure.Entities.Contacts;

namespace Infrastructure.Entities.Groups;

public sealed class Group : Entity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Contact> Contacts { get; set; } = [];
}
