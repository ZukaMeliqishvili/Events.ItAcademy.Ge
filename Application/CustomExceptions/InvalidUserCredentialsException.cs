using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class InvalidUserCredentialsException : Exception
    {
        public InvalidUserCredentialsException()
        {
        }

        public InvalidUserCredentialsException(string message)
          : base(message)
        {
        }

        public InvalidUserCredentialsException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
