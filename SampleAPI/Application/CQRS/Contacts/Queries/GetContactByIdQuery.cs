using Application.Core;
using Application.DTOs.Contacts;

namespace Application.CQRS.Contacts.Queries;

public record GetContactByIdQuery(int ContactId)
    : IRequest<OperationResult<ContactDataDto>>;
