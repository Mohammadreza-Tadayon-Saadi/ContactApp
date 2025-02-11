using Application.Core;
using Application.DTOs.Common;
using Application.DTOs.Contacts;

namespace Application.CQRS.Contacts.Command;

public record AddOrEditContactCommand(ContactDataDto Contact)
    : IRequest<OperationResult<ContextTransactionDto<int>>>;
