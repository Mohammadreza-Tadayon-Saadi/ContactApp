using Application.Core;
using Application.DTOs.Contacts;

namespace Application.CQRS.Contacts.Queries;

internal sealed class GetAllContactsQueryHandler(IContactsServices contactsServices)
    : IRequestHandler<GetAllContactsQuery, OperationResult<List<ContactDataDto>>>
{
    private readonly IContactsServices _contactsServices = contactsServices;

    public async Task<OperationResult<List<ContactDataDto>>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        => await _contactsServices.GetAllAsync(cancellationToken);
}
