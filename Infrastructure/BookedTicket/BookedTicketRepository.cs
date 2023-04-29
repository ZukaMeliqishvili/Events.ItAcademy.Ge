using Domain.BoockedTicket;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure._BookedTicket
{
    public class BookedTicketRepository:BaseRepository<BookedTicket>,IBookedTicketRepository
    {
        private readonly ApplicationDbContext _db;

        public BookedTicketRepository(ApplicationDbContext db)
          : base(db)
        {
            _db = db;
        }

        public async Task<BookedTicket> GetByUserAndEventIdAsync(string userId,int eventId,CancellationToken cancellationToken)
        {
            var query = _db.BookedTickets.Where(x=>x.UserId==userId && x.EventId==eventId);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<List<BookedTicket>> GetAllExpiredAsync(CancellationToken cancellationToken)
        {
            IQueryable<BookedTicket> query =_db.BookedTickets.Where(x => x.BookedTill < DateTime.Now);
            return await query.ToListAsync(cancellationToken);
        }

        public void RemoveRange(List<BookedTicket> bookedTickets)
        {
            _db.BookedTickets.RemoveRange(bookedTickets);
        }
    }
}
