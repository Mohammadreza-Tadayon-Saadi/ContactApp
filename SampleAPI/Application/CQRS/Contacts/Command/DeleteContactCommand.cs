using Application.Core;
using Application.DTOs.Common;

namespace Application.CQRS.Contacts.Command;

public record DeleteContactCommand(int ContactId)
    : IRequest<OperationResult<ContextTransactionDto<int>>>;
