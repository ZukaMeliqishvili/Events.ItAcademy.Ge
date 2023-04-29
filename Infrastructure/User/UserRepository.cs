using Domain.User;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.User
{
    public class UserRepository :BaseRepository<ApplicationUser>,IUserRepository
    {
        private readonly ApplicationDbContext db;

        public UserRepository(ApplicationDbContext db): base(db)
        {
            this.db = db;
        }
    }
}
