using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application._BookedTicket.Models.Request
{
    public class BookedTicketRequestModel
    {
        public string UserId { get; set; }

        public int EventId { get; set; }

        public DateTime BookedTill { get; set; }
    }
}
