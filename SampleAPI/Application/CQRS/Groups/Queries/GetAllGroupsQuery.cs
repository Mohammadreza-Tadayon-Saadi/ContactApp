using Application.Core;
using Application.DTOs.Groups;

namespace Application.CQRS.Groups.Queries;

public record GetAllGroupsQuery
    : IRequest<OperationResult<List<GroupDataDto>>>;
