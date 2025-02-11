using Application.Contracts.Services.Common;
using Application.DTOs.Groups;
using Infrastructure.Context;
using Infrastructure.Entities.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Services;

public class GroupServices(DataBaseContext context) 
    : BaseServices<Group>(context), IGroupServices
{
    public async Task<List<GroupDataDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await GroupDataDto.ProjectTo(TableNoTracking).ToListAsync(cancellationToken);
    public async Task<GroupDataDto> GetByIdAsync(int groupId, CancellationToken cancellationToken = default)
        => await GroupDataDto.ProjectTo(TableNoTracking.Where(g => g.Id == groupId)).SingleOrDefaultAsync(cancellationToken);
}
