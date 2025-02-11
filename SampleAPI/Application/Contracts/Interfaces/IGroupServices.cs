using Application.Contracts.Interfaces.Common;
using Application.DTOs.Groups;
using Infrastructure.Entities.Contacts;
using Infrastructure.Entities.Groups;

namespace Application.Contracts.Interfaces;

public interface IGroupServices : IBaseServices<Group>
{
    Task<List<GroupDataDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GroupDataDto> GetByIdAsync(int groupId, CancellationToken cancellationToken = default);
}
