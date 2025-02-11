using Application.Core;
using Application.DTOs.Common;

namespace Application.CQRS.Contacts.Command;

internal sealed class DeleteContactCommandHandler(IContactsServices contactsServices)
    : IRequestHandler<DeleteContactCommand, OperationResult<ContextTransactionDto<int>>>
{
    private readonly IContactsServices _contactsServices = contactsServices;

    public async Task<OperationResult<ContextTransactionDto<int>>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
       var result = await _contactsServices.DeleteAsync(c => c.Id == request.ContactId, cancellationToken: cancellationToken);
        return result;
    }
}
