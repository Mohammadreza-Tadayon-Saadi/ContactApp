using Application.Core;
using Application.DTOs.Contacts;

namespace Application.CQRS.Contacts.Queries;

public record GetAllContactsQuery
    : IRequest<OperationResult<List<ContactDataDto>>>;
