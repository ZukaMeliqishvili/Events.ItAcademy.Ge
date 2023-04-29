using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CustomExceptions
{
    public class UpdateDateExceededException : Exception
    {
        public UpdateDateExceededException()
        {
        }

        public UpdateDateExceededException(string message)
          : base(message)
        {
        }

        public UpdateDateExceededException(string message, Exception ex)
          : base(message, ex)
        {
        }
    }
}
