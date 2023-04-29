using Domain._Event;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure._Event
{
    public class EventRepository:BaseRepository<Event>,IEventRepository
    {
        private readonly ApplicationDbContext db;

        public EventRepository(ApplicationDbContext db)
          : base(db)
        {
            this.db = db;
        }

        public async Task<List<Event>> GetAllActiveEvents(CancellationToken cancellationToken)
        {
            IQueryable<Event> query = db.Events.Where(x => x.IsConfirmed == true);
            return await query.ToListAsync(cancellationToken);
        }
        public async Task<List<Event>> GetAllEventsByUser( CancellationToken cancellationToken, string UserId)
        {
            IQueryable<Event> query = db.Events.Where(x => x.UserId == UserId);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<Event>> GetAllFinishedEvents(CancellationToken cancellationToken)
        {
            IQueryable<Event> query = db.Events.Where(x => x.IsConfirmed == true && x.EndDate < DateTime.Now);
            return await query.ToListAsync(cancellationToken);
        }

        public void RemoveRange(List<Event> events)
        {
            if (events == null)
                return;
            db.RemoveRange(events);
        }

        public async Task<List<Event>> GetAllUnconfirmedEvents(CancellationToken cancellationToken)
        {
            IQueryable<Event> query = this.db.Events.Where<Event>((Expression<Func<Event, bool>>)(x => x.IsConfirmed == false));
            return await query.ToListAsync(cancellationToken);
        }
    }
}
