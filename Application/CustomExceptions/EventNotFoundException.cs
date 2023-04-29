using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException()
        {
        }

        public EventNotFoundException(string message)
          : base(message)
        {
        }

        public EventNotFoundException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
