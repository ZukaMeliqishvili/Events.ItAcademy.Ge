using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Models.Request
{
    public class UserLoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
