using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class InvalidReferenceException : Exception
    {
        public InvalidReferenceException()
        {
        }

        public InvalidReferenceException(string message)
          : base(message)
        {
        }

        public InvalidReferenceException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
