using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class OutOfTIcketsException : Exception
    {
        public OutOfTIcketsException()
        {
        }

        public OutOfTIcketsException(string message)
          : base(message)
        {
        }

        public OutOfTIcketsException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
