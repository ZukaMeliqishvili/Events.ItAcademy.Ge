using Infrastructure._ArchivedEvent;
using Infrastructure._BookedTicket;
using Infrastructure._Event;
using Infrastructure.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUnitOfWork
    {
        IEventRepository Event { get; }

        IUserRepository User { get; }

        IArchivedEventRepository ArchivedEvent { get; }

        IBookedTicketRepository BookedTicket { get; }

        Task SaveAsync();
    }
}
