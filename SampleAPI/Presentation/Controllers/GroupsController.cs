using API.Controllers.Common;
using Application.CQRS.Groups.Queries;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

[ApiVersion(1)]
public class GroupsController(IMediator mediator)
    : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var request = new GetAllGroupsQuery();
        var result = await _mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest();
    }

    [HttpGet("{groupId:int}")]
    public async Task<IActionResult> Get(int groupId, CancellationToken cancellationToken = default)
    {
        var request = new GetGroupByIdQuery(groupId);
        var result = await _mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest();
    }
}
