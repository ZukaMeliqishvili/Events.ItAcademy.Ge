using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class TicketAlreadyBookedException : Exception
    {
        public TicketAlreadyBookedException()
        {
        }

        public TicketAlreadyBookedException(string message)
          : base(message)
        {
        }

        public TicketAlreadyBookedException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
