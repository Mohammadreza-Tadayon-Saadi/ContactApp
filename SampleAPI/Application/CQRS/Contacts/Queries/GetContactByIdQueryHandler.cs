using Application.Core;
using Application.DTOs.Contacts;

namespace Application.CQRS.Contacts.Queries;

internal sealed class GetContactByIdQueryHandler(IContactsServices contactsServices)
    : IRequestHandler<GetContactByIdQuery, OperationResult<ContactDataDto>>
{
    private readonly IContactsServices _contactsServices = contactsServices;

    public async Task<OperationResult<ContactDataDto>> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        => await _contactsServices.GetByIdAsync(request.ContactId, cancellationToken);
}
