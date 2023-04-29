using Domain._Event;
using Domain.BoockedTicket;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class ApplicationUser : IdentityUser
    {
        public List<Event> Events { get; set; }

        public List<ArchivedEvent> ArchivedEvents { get; set; }

        public List<BookedTicket> BookedTicket { get; set; }
    }
}
