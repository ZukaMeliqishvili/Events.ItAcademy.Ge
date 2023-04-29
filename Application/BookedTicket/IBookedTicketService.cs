using Application._BookedTicket.Models.Request;
using Domain.BoockedTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application._BookedTicket
{
    public interface IBookedTicketService
    {
        Task CreateAsync(BookedTicketRequestModel entity, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<List<BookedTicket>> GetAllAsync(CancellationToken cancellationToken);
        Task RemoveExpiredBookedTickets(CancellationToken cancellationToken);
    }
}
