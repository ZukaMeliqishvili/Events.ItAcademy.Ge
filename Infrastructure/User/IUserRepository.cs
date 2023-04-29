﻿using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.User
{
    public interface IUserRepository:IBaseRepository<ApplicationUser>
    {
    }
}
