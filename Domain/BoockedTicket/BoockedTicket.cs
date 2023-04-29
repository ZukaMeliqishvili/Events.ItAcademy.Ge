using Domain._Event;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BoockedTicket
{
    public class BookedTicket
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public DateTime BookedTill { get; set; }
    }
}
