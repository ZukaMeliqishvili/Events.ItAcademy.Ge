using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{

    public class EventIsAlreadyActiveException : Exception
    {
        public EventIsAlreadyActiveException()
        {
        }

        public EventIsAlreadyActiveException(string message)
          : base(message)
        {
        }

        public EventIsAlreadyActiveException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
