using Domain._Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure._Event
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<List<Event>> GetAllActiveEvents(CancellationToken cancellationToken);
        Task<List<Event>> GetAllEventsByUser(CancellationToken cancellationToken, string UserId);
        Task<List<Event>> GetAllFinishedEvents(CancellationToken cancellationToken);
        Task<List<Event>> GetAllUnconfirmedEvents(CancellationToken cancellationToken);
        void RemoveRange(List<Event> events);
    }
}
