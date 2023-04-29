using Domain.BoockedTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure._BookedTicket
{
    public interface IBookedTicketRepository : IBaseRepository<BookedTicket>
    {
        Task<List<BookedTicket>> GetAllExpiredAsync(CancellationToken cancellationToken);
        Task<BookedTicket> GetByUserAndEventIdAsync(string userId, int eventId, CancellationToken cancellationToken);
        void RemoveRange(List<BookedTicket> bookedTickets);
    }
}
