using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException()
        {
        }

        public InvalidRequestException(string message)
          : base(message)
        {
        }

        public InvalidRequestException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
