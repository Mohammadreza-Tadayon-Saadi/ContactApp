using Application.Contracts.Services.Common;
using Application.DTOs.Contacts;
using Infrastructure.Context;
using Infrastructure.Entities.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Services;

public class ContactsServices(DataBaseContext context) 
    : BaseServices<Contact>(context), IContactsServices
{
    public async Task<List<ContactDataDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await ContactDataDto.ProjectTo(TableNoTracking).ToListAsync(cancellationToken);

    public async Task<ContactDataDto> GetByIdAsync(int contactId, CancellationToken cancellationToken = default)
        => await ContactDataDto.ProjectTo(TableNoTracking.Where(c => c.Id == contactId))
                .SingleOrDefaultAsync(cancellationToken);

    public async Task<Contact> GetAsync(int contactId, CancellationToken cancellationToken = default)
        => await (Table.Where(c => c.Id == contactId))
                .SingleOrDefaultAsync(cancellationToken);
}
