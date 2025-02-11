using Application.Core;
using Application.DTOs.Common;

namespace Application.CQRS.Contacts.Command;

internal sealed class AddContactCommandHandler(IContactsServices contactsServices)
    : IRequestHandler<AddOrEditContactCommand, OperationResult<ContextTransactionDto<int>>>
{
    private readonly IContactsServices _contactsServices = contactsServices;

    public async Task<OperationResult<ContextTransactionDto<int>>> Handle(AddOrEditContactCommand request, CancellationToken cancellationToken)
    {
        if (request.Contact.Id == 0)
        {
            var entity = request.Contact.ToEntity();
            entity.GroupId = request.Contact.Group;

            var result = await _contactsServices.AddAsync(entity, cancellationToken: cancellationToken);

            return result;
        }

        var contact = await _contactsServices.GetAsync(request.Contact.Id, cancellationToken);
        var updateResult = await _contactsServices.ExecuteInTransactionAsync(() =>
        {
            request.Contact.ToEntity(contact);
            contact.GroupId = request.Contact.Group;
        }, cancellationToken: cancellationToken);

        return updateResult;
    }
}