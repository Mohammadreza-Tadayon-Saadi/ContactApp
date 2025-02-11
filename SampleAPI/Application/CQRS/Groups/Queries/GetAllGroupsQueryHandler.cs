using Application.Core;
using Application.DTOs.Groups;

namespace Application.CQRS.Groups.Queries;

internal sealed class GetAllGroupsQueryHandler (IGroupServices groupServices)
    : IRequestHandler<GetAllGroupsQuery, OperationResult<List<GroupDataDto>>>
{
    private readonly IGroupServices _groupServices = groupServices;

    public async Task<OperationResult<List<GroupDataDto>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        => await _groupServices.GetAllAsync(cancellationToken);
}
