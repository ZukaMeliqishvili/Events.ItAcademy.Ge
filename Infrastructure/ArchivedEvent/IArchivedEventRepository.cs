using Domain._Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure._ArchivedEvent
{
    public interface IArchivedEventRepository : IBaseRepository<ArchivedEvent>
    {
        Task AddRange(List<ArchivedEvent> events, CancellationToken cancellationToken);
    }
}
