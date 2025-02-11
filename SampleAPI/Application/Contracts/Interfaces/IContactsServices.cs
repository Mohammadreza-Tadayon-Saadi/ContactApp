using Application.Contracts.Interfaces.Common;
using Application.DTOs.Contacts;
using Infrastructure.Entities.Contacts;

namespace Application.Contracts.Interfaces;

public interface IContactsServices : IBaseServices<Contact>
{
    Task<List<ContactDataDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ContactDataDto> GetByIdAsync(int contactId, CancellationToken cancellationToken = default);
    Task<Contact> GetAsync(int contactId, CancellationToken cancellationToken = default);
}
