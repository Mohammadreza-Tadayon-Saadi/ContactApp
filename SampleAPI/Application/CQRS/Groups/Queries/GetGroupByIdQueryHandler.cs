using Application.Core;
using Application.DTOs.Groups;

namespace Application.CQRS.Groups.Queries;

internal sealed class GetGroupByIdQueryHandler(IGroupServices groupServices)
    : IRequestHandler<GetGroupByIdQuery, OperationResult<GroupDataDto>>
{
    private readonly IGroupServices _groupServices = groupServices;

    public async Task<OperationResult<GroupDataDto>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        => await _groupServices.GetByIdAsync(request.GroupId, cancellationToken);
}
