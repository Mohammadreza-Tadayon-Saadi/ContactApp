using API.Controllers.Common;
using Application.CQRS.Contacts.Command;
using Application.CQRS.Contacts.Queries;
using Application.DTOs.Contacts;
using Asp.Versioning;
using Infrastructure.Entities.Contacts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiVersion(1)]
public class ContactsController(IMediator mediator)
    : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var request = new GetAllContactsQuery();
        var result = await _mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest();
    }

    [HttpGet("{contactId:int}")]
    public async Task<IActionResult> Get(int contactId, CancellationToken cancellationToken = default)
    {
        var request = new GetContactByIdQuery(contactId);
        var result = await _mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Add(ContactDataDto model, CancellationToken cancellationToken = default)
    {
        var request = new AddOrEditContactCommand(model);
        var result = await _mediator.Send(request, cancellationToken);

        var contacts = await _mediator.Send(new GetAllContactsQuery(), cancellationToken);

        return result.IsSuccess ? Ok(contacts.Value) : BadRequest();
    }

    [HttpPut("{contactId:int}")]
    public async Task<IActionResult> Update(int contactId, ContactDataDto model, CancellationToken cancellationToken = default)
    {
        model.Id= contactId;
        var request = new AddOrEditContactCommand(model);
        var result = await _mediator.Send(request, cancellationToken);

        var contacts = await _mediator.Send(new GetAllContactsQuery(), cancellationToken);

        return result.IsSuccess ? Ok(contacts.Value) : BadRequest();
    }

    [HttpDelete("{contactId:int}")]
    public async Task<IActionResult> Update(int contactId, CancellationToken cancellationToken = default)
    {
        var request = new DeleteContactCommand(contactId);
        var result = await _mediator.Send(request, cancellationToken);

        var contacts = await _mediator.Send(new GetAllContactsQuery(), cancellationToken);

        return result.IsSuccess ? Ok(contacts.Value) : BadRequest();
    }
}