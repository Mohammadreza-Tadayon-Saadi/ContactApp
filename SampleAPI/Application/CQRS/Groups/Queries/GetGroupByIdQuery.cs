using Application.Core;
using Application.DTOs.Groups;

namespace Application.CQRS.Groups.Queries;

public record GetGroupByIdQuery(int GroupId)
    : IRequest<OperationResult<GroupDataDto>>;
